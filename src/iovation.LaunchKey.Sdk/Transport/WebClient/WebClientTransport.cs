using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using iovation.LaunchKey.Sdk.Cache;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Crypto.Jwe;
using iovation.LaunchKey.Sdk.Crypto.Jwt;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;
using iovation.LaunchKey.Sdk.Util;

namespace iovation.LaunchKey.Sdk.Transport.WebClient
{
	/// <summary>
	/// An implementation of the ITransport interface that uses the BCL HTTP classes (WebRequest)
	/// </summary>
	public class WebClientTransport : ITransport
	{
		private readonly ICache _publicKeyCache;
		private readonly ICrypto _crypto;
		private readonly IHttpClient _httpClient;
		private readonly IJweService _jweService;
		private readonly int _offsetTtl;
		private readonly int _currentPublicKeyTtl;
		private readonly IJwtService _jwtService;
		private readonly string _baseUrl;
		private readonly EntityIdentifier _issuer;
		private readonly EntityKeyMap _keyMap;
		private readonly IJsonEncoder _jsonDecoder;
		private const string IOV_JWT_HEADER = "X-IOV-JWT";
		private const string LAUNCHKEY_CACHE_PREFIX = "LaunchKeyPublicKey:";

		// key caching
		private DateTime? _currentKeyExpires = null;
		private CachedKey _currentKey = null;

		// server time drift calculation
		private object _serverTimeLock = new object();
		private TimeSpan _serverTimeOffset = TimeSpan.Zero;
		private DateTime _serverTimeOffsetExpires = DateTime.MinValue;

		private CachedKey FetchPublicKeyWithId(string id)
		{
			try
			{
				var cacheKey = LAUNCHKEY_CACHE_PREFIX + id;
				var keyContents = _publicKeyCache.Get(cacheKey);
				var key = _crypto.LoadRsaPublicKey(keyContents);
				return new CachedKey(id, key);
			}
			catch (CacheException) { } // didn't exist
			catch (CryptographyError) { } // couldnt load it

			try
			{
				// go get it
				var serverResponse = PublicV3PublicKeyGet(id);

				// try to load it
				var key = _crypto.LoadRsaPublicKey(serverResponse.PublicKey);

				// load ok, cache it 
				_publicKeyCache.Put(
					LAUNCHKEY_CACHE_PREFIX + serverResponse.PublicKeyFingerPrint,
					serverResponse.PublicKey
				);

				// deliver it
				return new CachedKey(serverResponse.PublicKeyFingerPrint, key);
			}
			catch (CryptographyError)
			{
				// failed to load it, go get it again
				throw new InvalidResponseException("Server returned an unparseable PEM object while we tried to fetch a key");
			}
		}

		private CachedKey GetCurrentPublicKey()
		{
			if (_currentKeyExpires == null || _currentKeyExpires.Value < DateTime.Now)
			{
				// refetch
				_currentKey = FetchPublicKeyWithId(null);
				_currentKeyExpires = DateTime.Now.AddSeconds(_currentPublicKeyTtl);
			}

			return _currentKey;
		}

		private DateTime GetCurrentTime()
		{
			lock (_serverTimeLock)
			{
				if (_serverTimeOffsetExpires < DateTime.Now)
				{
					var now = DateTime.UtcNow;
					var response = PublicV3PingGet();
					_serverTimeOffset = response.ApiTime - now;
					_serverTimeOffsetExpires = DateTime.Now.AddSeconds(_offsetTtl);
				}
			}

			return DateTime.UtcNow.Add(_serverTimeOffset);
		}

		public WebClientTransport(
			IHttpClient httpClient,
			ICrypto crypto,
			ICache publicKeyCache,
			string baseUrl,
			EntityIdentifier issuer,
			IJwtService jwtService,
			IJweService jweService,
			int offsetTTL,
			int currentPublicKeyTTL,
			EntityKeyMap keyMap,
			IJsonEncoder jsonDecoder)
		{
			_httpClient = httpClient;
			_crypto = crypto;
			_publicKeyCache = publicKeyCache;
			_baseUrl = baseUrl;
			_issuer = issuer;
			_jwtService = jwtService;
			_jweService = jweService;
			_offsetTtl = offsetTTL;
			_currentPublicKeyTtl = currentPublicKeyTTL;
			_keyMap = keyMap;
			_jsonDecoder = jsonDecoder;
		}

		private string MakeUrl(string path)
		{
			return _baseUrl + path;
		}

		private HttpResponse ExecutePublicRequest(HttpMethod method, string path)
		{
			try
			{
				return _httpClient.ExecuteRequest(
					method,
					MakeUrl(path),
					null,
					null
				);
			}
			catch (IOException ex)
			{
				throw new CommunicationErrorException("A network-level error occurred, see inner exception.", ex);
			}
			catch (TimeoutException ex)
			{
				throw new CommunicationErrorException("A connection timeout occurred, see inner exception.", ex);
			}
		}

		private HttpResponse ExecuteRequest(
			HttpMethod method,
			string path,
			EntityIdentifier subjectEntity,
			object transportObject,
			List<int> httpStatusCodeWhiteList)
		{
			try
			{
				var requestUrl = MakeUrl(path);
				var requestBody = EncodeRequestBody(transportObject);

				// encrypt the payload using the API's public key
				var publicKey = GetCurrentPublicKey();
				string hashString = null;
				string hashFunction = null;
				string encryptedPayload = null;

				if (requestBody != null)
				{
					encryptedPayload = _jweService.Encrypt(
						requestBody,
						publicKey.KeyData,
						publicKey.Thumbprint,
						"application/json"
					);
					var hash = _crypto.Sha256(Encoding.UTF8.GetBytes(encryptedPayload));
					hashString = ByteArrayUtils.ByteArrayToHexString(hash);
					hashFunction = "S256";
				}

				var requestId = Guid.NewGuid().ToString("D");

				// sign the encrypted payload and turn it into a JWT token ... this is sent as the header
				var token = _jwtService.Encode(
					requestId,
					_issuer.ToString(),
					subjectEntity.ToString(),
					GetCurrentTime(),
					method.ToString(),
					path,
					hashFunction,
					hashString
				);

				var response = _httpClient.ExecuteRequest(
					method,
					requestUrl,
					encryptedPayload,
					new Dictionary<string, string>
					{
						{"Authorization", "IOV-JWT " + token}
					}
				);

				// check for errors, decode and decrypt them if needed and crash out
				ThrowForStatus(response, publicKey.KeyData, requestId, httpStatusCodeWhiteList);

				// validate the response itself
				ValidateEncryptedResponse(response, publicKey.KeyData, requestId);

				// if we're here, we've passed all validation and error handling
				return response;
			}
			catch (JwtError ex)
			{
				throw new InvalidResponseException("JWT error occurred -- see exception", ex);
			}
			catch (IOException ex)
			{
				throw new CommunicationErrorException("A network-level error occurred, see inner exception.", ex);
			}
			catch (TimeoutException ex)
			{
				throw new CommunicationErrorException("A connection timeout occurred, see inner exception.", ex);
			}
		}

		private void ValidateEncryptedResponse(HttpResponse response, RSA publicKey, string requestId)
		{
			// decode and verify the JWT header
			var jwt = ValidateResponseJWT(response, publicKey, requestId);

			// if we get here, verification checked out on the JWT level (signature, audience, etc. other registered claims)
			// here, we validate that our private claims are OK as well.
			ValidatePrivateClaims(response, jwt);
		}

		private void ThrowForStatus(HttpResponse response, RSA publicKey, string requestId, List<int> httpStatusCodeWhiteList)
		{
			var code = (int)response.StatusCode;
			var name = response.StatusDescription;

			// whitelisted HTTP code, ignore it
			if (httpStatusCodeWhiteList != null && httpStatusCodeWhiteList.Contains((int)response.StatusCode)) return;

			// server didn't like our request
			if (response.StatusCode == HttpStatusCode.BadRequest)
			{
				// no body came back ... throw generic http 400 exception
				if (string.IsNullOrWhiteSpace(response.ResponseBody))
				{
					throw new InvalidRequestException(response.StatusDescription, null, "HTTP-400");
				}
				else
				{
					ValidateEncryptedResponse(response, publicKey, requestId);
					var errorResponse = DecryptResponse<Sdk.Domain.Error>(response);
					throw InvalidRequestException.FromErrorCode(errorResponse.ErrorCode, _jsonDecoder.EncodeObject(errorResponse.ErrorDetail));
				}
			}

			// other non-valid responses. Throw an appropriate exception
			if (!(code >= 200 && code < 300))
			{
				throw CommunicationErrorException.FromStatusCode(code, $"HTTP Error: [{code}] {name}");
			}
		}

		private TResultType DecryptResponse<TResultType>(HttpResponse response)
		{
			var decryptedPayload = DecryptJweData(response.ResponseBody);

			// deserialize it from JSON to data type
			return DecodeResponse<TResultType>(decryptedPayload);
		}

		private void ValidateHash(string body, string hashAlgo, string hash)
		{
			byte[] hashBytes;
			var bodyBytes = Encoding.UTF8.GetBytes(body);

			if (hashAlgo == "S256")
			{
				hashBytes = _crypto.Sha256(bodyBytes);
			}
			else if (hashAlgo == "S384")
			{
				hashBytes = _crypto.Sha384(bodyBytes);
			}
			else if (hashAlgo == "S512")
			{
				hashBytes = _crypto.Sha512(bodyBytes);
			}
			else
			{
				throw new JwtError($"Hash of response content uses unsupported algorithm of {hashAlgo}");
			}

			var computedBodyHash = ByteArrayUtils.ByteArrayToHexString(hashBytes);
			if (computedBodyHash != hash)
			{
				throw new JwtError($"Hash of response content does not match JWT response hash");
			}
		}

		private void ValidatePrivateClaimsForServerSentRequest(string method, string path, string body, JwtClaimsRequest privateClaims)
		{
			if (privateClaims == null)
				throw new JwtError("Request did not contain mandatory private claims.");

			if (method != null && method != privateClaims.Method)
				throw new JwtError("Request method did not match the request method of the signed JWT token.");

			if (path != null && path != privateClaims.Path)
				throw new JwtError("Request path did not match the request path of the signed JWT token.");

			// if we have a body to hash OR the JWT has a hash
			if (!string.IsNullOrWhiteSpace(body) || !string.IsNullOrWhiteSpace(privateClaims.ContentHash))
			{
				ValidateHash(body, privateClaims.ContentHashAlgorithm, privateClaims.ContentHash);
			}
		}

		private void ValidatePrivateClaims(HttpResponse response, JwtClaims jwt)
		{
			// verify response information matches the JWT
			if (response.Headers[HttpResponseHeader.Location] != jwt.Response.LocationHeader)
				throw new JwtError("Location header of response content does not match JWT response location");

			if (response.Headers[HttpResponseHeader.CacheControl] != jwt.Response.CacheControlHeader)
				throw new JwtError("Cache-Control header of response content does not match JWT response cache");

			if ((int)response.StatusCode != jwt.Response.StatusCode)
			{
				throw new JwtError("Status code of response content does not match JWT response status code");
			}

			// if the response has body content, we need to validate the hash
			if (!string.IsNullOrWhiteSpace(response.ResponseBody))
			{
				ValidateHash(response.ResponseBody, jwt.Response.ContentHashAlgorithm, jwt.Response.ContentHash);
			}
		}

		private JwtClaims ValidateResponseJWT(HttpResponse response, RSA publicKey, string requestId)
		{
			var jwtHeader = response.Headers[IOV_JWT_HEADER];
			if (jwtHeader == null)
				throw new InvalidResponseException("Expected presence of JWT response header but none was found");

			return _jwtService.Decode(
				publicKey,
				_issuer.ToString(),
				requestId,
				GetCurrentTime(),
				jwtHeader
			);
		}

		private string EncodeRequestBody(object body)
		{
			try
			{
				return _jsonDecoder.EncodeObject(body);
			}
			catch (JsonEncoderException ex)
			{
				throw new InvalidRequestException("Error serializing request message; This is very likely an SDK bug. Please report to support.", ex);
			}
		}

		private TResultType DecodeResponse<TResultType>(string data)
		{
			try
			{
				return _jsonDecoder.DecodeObject<TResultType>(data);
			}
			catch (JsonEncoderException ex)
			{
				throw new InvalidResponseException("Error deserializing response", ex);
			}
		}

		private string DecryptJweData(string data)
		{
			try
			{
				return _jweService.Decrypt(data);
			}
			catch (JsonEncoderException ex)
			{
				throw new InvalidResponseException("Error decrypting response", ex);
			}
		}

		public PublicV3PingGetResponse PublicV3PingGet()
		{
			var response = ExecutePublicRequest(HttpMethod.GET, "/public/v3/ping");
			return DecodeResponse<PublicV3PingGetResponse>(response.ResponseBody);
		}

		public PublicV3PublicKeyGetResponse PublicV3PublicKeyGet(string publicKeyFingerprint)
		{
			var path = "/public/v3/public-key";
			if (!string.IsNullOrWhiteSpace(publicKeyFingerprint))
			{
				path += "/" + publicKeyFingerprint;
			}

			var response = ExecutePublicRequest(HttpMethod.GET, path);
			var keyHeader = response.Headers["X-IOV-KEY-ID"];
			if (keyHeader == null)
			{
				throw new InvalidResponseException("Public Key ID header X-IOV-KEY-ID not found in response");
			}

			if (string.IsNullOrWhiteSpace(response.ResponseBody))
			{
				throw new InvalidResponseException("Public key returned from server was empty or missing.");
			}

			return new PublicV3PublicKeyGetResponse(response.ResponseBody, keyHeader);
		}

		public ServiceV3AuthsPostResponse ServiceV3AuthsPost(ServiceV3AuthsPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(
				HttpMethod.POST,
				"/service/v3/auths",
				subject,
				request,
				null
			);
			return DecryptResponse<ServiceV3AuthsPostResponse>(response);
		}

		public ServiceV3AuthsGetResponse ServiceV3AuthsGet(Guid authRequestId, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.GET, $"/service/v3/auths/{authRequestId}", subject, null, new List<int> {408});

			if ((int)response.StatusCode == 204)
			{
				// user has not responded yet
				return null;
			}

			if ((int)response.StatusCode == 408)
			{
				throw new AuthorizationRequestTimedOutError();
			}

			try
			{
				var coreResponse = DecryptResponse<ServiceV3AuthsGetResponseCore>(response);
				var jwtHeader = response.Headers[IOV_JWT_HEADER];
				var jwtData = _jwtService.GetJWTData(jwtHeader);
				var audience = EntityIdentifier.FromString(jwtData.Audience);
				var key = _keyMap.GetKey(audience, coreResponse.PublicKeyId);

				try
				{
					var encryptedDeviceResponse = Convert.FromBase64String(coreResponse.EncryptedDeviceResponse);
					var decryptedResponse = _crypto.DecryptRSA(encryptedDeviceResponse, key);
					var decryptedResponseString = Encoding.UTF8.GetString(decryptedResponse);
					var deviceResponse = _jsonDecoder.DecodeObject<ServiceV3AuthsGetResponseDevice>(decryptedResponseString);
					return new ServiceV3AuthsGetResponse(
						audience,
						subject.Id,
						coreResponse.ServiceUserHash,
						coreResponse.OrgUserHash,
						coreResponse.UserPushId,
						deviceResponse.AuthorizationRequestId,
						deviceResponse.Response,
						deviceResponse.DeviceId,
						deviceResponse.ServicePins
					);
				}
				catch (Exception ex)
				{
					throw new CryptographyError("Error decrypting device response", ex);
				}
			}
			catch (JwtError ex)
			{
				throw new CryptographyError("Unable to parse JWT to get key info", ex);
			}
		}

		public void ServiceV3SessionsPost(ServiceV3SessionsPostRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.POST, "/service/v3/sessions", subject, request, null);
		}

		public void ServiceV3SessionsDelete(ServiceV3SessionsDeleteRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.DELETE, "/service/v3/sessions", subject, request, null);
		}

		public DirectoryV3DevicesPostResponse DirectoryV3DevicesPost(DirectoryV3DevicesPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/directory/v3/devices", subject, request, null);
			var decryptedResponse = DecryptResponse<DirectoryV3DevicesPostResponse>(response);
			return decryptedResponse;
		}

		public DirectoryV3DevicesListPostResponse DirectoryV3DevicesListPost(DirectoryV3DevicesListPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/directory/v3/devices/list", subject, request, null);
			var devices = DecryptResponse<List<DirectoryV3DevicesListPostResponse.Device>>(response);
			return new DirectoryV3DevicesListPostResponse(devices);
		}

		public void DirectoryV3DevicesDelete(DirectoryV3DevicesDeleteRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.DELETE, "/directory/v3/devices", subject, request, null);
		}

		public DirectoryV3SessionsListPostResponse DirectoryV3SessionsListPost(DirectoryV3SessionsListPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/directory/v3/sessions/list", subject, request, null);
			var sessions = DecryptResponse<List<DirectoryV3SessionsListPostResponse.Session>>(response);
			return new DirectoryV3SessionsListPostResponse(sessions);
		}

		public void DirectoryV3SessionsDelete(DirectoryV3SessionsDeleteRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.DELETE, "/directory/v3/sessions", subject, request, null);
		}

		public ServicesPostResponse OrganizationV3ServicesPost(ServicesPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/services", subject, request, null);
			var serviceResponse = DecryptResponse<ServicesPostResponse>(response);
			return serviceResponse;
		}

		public void OrganizationV3ServicesPatch(ServicesPatchRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.PATCH, "/organization/v3/services", subject, request, null);
		}

		public ServicesListPostResponse OrganizationV3ServicesListPost(ServicesListPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/services/list", subject, request, null);
			var decryptedResponse = DecryptResponse<List<ServicesListPostResponse.Service>>(response);
			return new ServicesListPostResponse(decryptedResponse);
		}

		public ServicesGetResponse OrganizationV3ServicesGet(EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.GET, "/organization/v3/services", subject, null, null);
			var decryptedResponse = DecryptResponse<List<ServicesGetResponse.Service>>(response);
			return new ServicesGetResponse(decryptedResponse);
		}

		public OrganizationV3DirectoriesPostResponse OrganizationV3DirectoriesPost(OrganizationV3DirectoriesPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/directories", subject, request, null);
			var decryptedResponse = DecryptResponse<OrganizationV3DirectoriesPostResponse>(response);
			return decryptedResponse;
		}

		public void OrganizationV3DirectoriesPatch(OrganizationV3DirectoriesPatchRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.PATCH, "/organization/v3/directories", subject, request, null);
		}

		public OrganizationV3DirectoriesGetResponse OrganizationV3DirectoriesGet(EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.GET, "/organization/v3/directories", subject, null, null);
			var decryptedResponse = DecryptResponse<List<OrganizationV3DirectoriesGetResponse.Directory>>(response);
			return new OrganizationV3DirectoriesGetResponse(decryptedResponse);
		}

		public OrganizationV3DirectoriesListPostResponse OrganizationV3DirectoriesListPost(OrganizationV3DirectoriesListPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/directories/list", subject, request, null);
			var decryptedResponse = DecryptResponse<List<OrganizationV3DirectoriesListPostResponse.Directory>>(response);
			return new OrganizationV3DirectoriesListPostResponse(decryptedResponse);
		}

		public ServicesPostResponse DirectoryV3ServicesPost(ServicesPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/directory/v3/services", subject, request, null);
			var serviceResponse = DecryptResponse<ServicesPostResponse>(response);
			return serviceResponse;
		}

		public void DirectoryV3ServicesPatch(ServicesPatchRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.PATCH, "/directory/v3/services", subject, request, null);
		}

		public ServicesListPostResponse DirectoryV3ServicesListPost(ServicesListPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/directory/v3/services/list", subject, request, null);
			var decryptedResponse = DecryptResponse<List<ServicesListPostResponse.Service>>(response);
			return new ServicesListPostResponse(decryptedResponse);
		}

		public OrganizationV3DirectorySdkKeysPostResponse OrganizationV3DirectorySdkKeysPost(OrganizationV3DirectorySdkKeysPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/directory/sdk-keys", subject, request, null);
			var decryptedResponse = DecryptResponse<OrganizationV3DirectorySdkKeysPostResponse>(response);
			return decryptedResponse;
		}

		public void OrganizationV3DirectorySdkKeysDelete(OrganizationV3DirectorySdkKeysDeleteRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.DELETE, "/organization/v3/directory/sdk-keys", subject, request, null);
		}

		public OrganizationV3DirectorySdkKeysListPostResponse OrganizationV3DirectorySdkKeysListPost(OrganizationV3DirectorySdkKeysListPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/directory/sdk-keys/list", subject, request, null);
			var decryptedResponse = DecryptResponse<List<Guid>>(response);
			return new OrganizationV3DirectorySdkKeysListPostResponse(decryptedResponse);
		}

		public ServicesGetResponse DirectoryV3ServicesGet(EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.GET, "/directory/v3/services", subject, null, null);
			var decryptedResponse = DecryptResponse<List<ServicesGetResponse.Service>>(response);
			return new ServicesGetResponse(decryptedResponse);
		}

		public KeysListPostResponse OrganizationV3ServiceKeysListPost(ServiceKeysListPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/service/keys/list", subject, request, null);
			var decryptedResponse = DecryptResponse<List<KeysListPostResponse.Key>>(response);
			return new KeysListPostResponse(decryptedResponse);
		}

		public KeysPostResponse OrganizationV3ServiceKeysPost(ServiceKeysPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/service/keys", subject, request, null);
			var decryptedResponse = DecryptResponse<KeysPostResponse>(response);
			return decryptedResponse;
		}

		public void OrganizationV3ServiceKeysPatch(ServiceKeysPatchRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.PATCH, "/organization/v3/service/keys", subject, request, null);
		}

		public void OrganizationV3ServiceKeysDelete(ServiceKeysDeleteRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.DELETE, "/organization/v3/service/keys", subject, request, null);
		}

		public KeysListPostResponse OrganizationV3DirectoryKeysListPost(DirectoryKeysListPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/directory/keys/list", subject, request, null);
			var decryptedResponse = DecryptResponse<List<KeysListPostResponse.Key>>(response);
			return new KeysListPostResponse(decryptedResponse);
		}

		public KeysPostResponse OrganizationV3DirectoryKeysPost(DirectoryKeysPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/directory/keys", subject, request, null);
			var decryptedResponse = DecryptResponse<KeysPostResponse>(response);
			return decryptedResponse;
		}

		public void OrganizationV3DirectoryKeysPatch(DirectoryKeysPatchRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.PATCH, "/organization/v3/directory/keys", subject, request, null);
		}

		public void OrganizationV3DirectoryKeysDelete(DirectoryKeysDeleteRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.DELETE, "/organization/v3/directory/keys", subject, request, null);
		}

		public KeysListPostResponse DirectoryV3ServiceKeysListPost(ServiceKeysListPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/directory/v3/service/keys/list", subject, request, null);
			var decryptedResponse = DecryptResponse<List<KeysListPostResponse.Key>>(response);
			return new KeysListPostResponse(decryptedResponse);
		}

		public KeysPostResponse DirectoryV3ServiceKeysPost(ServiceKeysPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/directory/v3/service/keys", subject, request, null);
			var decryptedResponse = DecryptResponse<KeysPostResponse>(response);
			return decryptedResponse;
		}

		public void DirectoryV3ServiceKeysPatch(ServiceKeysPatchRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.PATCH, "/directory/v3/service/keys", subject, request, null);
		}

		public void DirectoryV3ServiceKeysDelete(ServiceKeysDeleteRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.DELETE, "/directory/v3/service/keys", subject, request, null);
		}

		public void OrganizationV3ServicePolicyPut(ServicePolicyPutRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.PUT, "/organization/v3/service/policy", subject, request, null);
		}

		public AuthPolicy OrganizationV3ServicePolicyItemPost(ServicePolicyItemPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/organization/v3/service/policy/item", subject, request, null);
			return DecryptResponse<AuthPolicy>(response);
		}

		public void OrganizationV3ServicePolicyDelete(ServicePolicyDeleteRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.DELETE, "/organization/v3/service/policy", subject, request, null);
		}

		public void DirectoryV3ServicePolicyPut(ServicePolicyPutRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.PUT, "/directory/v3/service/policy", subject, request, null);
		}

		public AuthPolicy DirectoryV3ServicePolicyItemPost(ServicePolicyItemPostRequest request, EntityIdentifier subject)
		{
			var response = ExecuteRequest(HttpMethod.POST, "/directory/v3/service/policy/item", subject, request, null);
			return DecryptResponse<AuthPolicy>(response);
		}

		public void DirectoryV3ServicePolicyDelete(ServicePolicyDeleteRequest request, EntityIdentifier subject)
		{
			ExecuteRequest(HttpMethod.DELETE, "/directory/v3/service/policy", subject, request, null);
		}

		private string GetFirstHeader(Dictionary<string, List<string>> headers, string headerKey)
		{
			foreach (var header in headers)
			{
				if (string.Compare(headerKey, header.Key, StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (header.Value != null && header.Value.Count > 0)
						return header.Value[0];
				}
			}

			throw new InvalidRequestException($"{headerKey} header is missing");
		}

		public IServerSentEvent HandleServerSentEvent(Dictionary<string, List<string>> headers, string body, string method = null, string path = null)
		{
			var iovHeader = GetFirstHeader(headers, IOV_JWT_HEADER);
			var contentHeader = GetFirstHeader(headers, "Content-Type");
			try
			{
				// pull out the key id 
				var jwtData = _jwtService.GetJWTData(iovHeader);

				// get public key associated with the id. may have to fetch it!
				var publicKey = FetchPublicKeyWithId(jwtData.KeyId);

				// decode and verify the JWT.
				// note we do not store the result -- its not needed. Just want the decoder to throw an exception
				// if there is a validation issue
				// we also dont verify our private claims here, as they are not present or relevant
				var jwtObject = _jwtService.Decode(publicKey.KeyData, _issuer.ToString(), null, GetCurrentTime(), iovHeader);

				ValidatePrivateClaimsForServerSentRequest(method, path, body, jwtObject.Request);

				// if this is application/jose, its an auths response. 
				// im not sure if this is great -- maybe a better way to detect it. 
				// what happens when we have multiple webhooks types that use encrypted payloads ... ???
				if (contentHeader == "application/jose")
				{
					var requestingEntity = EntityIdentifier.FromString(jwtData.Audience);
					var bodyJweHeaders = _jweService.GetHeaders(body);
					if (!bodyJweHeaders.ContainsKey("kid"))
						throw new InvalidRequestException("JWE headers does not include a key id");
					var privateKey = _keyMap.GetKey(requestingEntity, bodyJweHeaders["kid"]);
					var decryptedBody = _jweService.Decrypt(body, privateKey);
					var core = DecodeResponse<ServiceV3AuthsGetResponseCore>(decryptedBody);
					var encryptedDeviceData = Convert.FromBase64String(core.EncryptedDeviceResponse);
					var decryptedDeviceData = _crypto.DecryptRSA(encryptedDeviceData, privateKey);
					var decryptedDeviceDataString = Encoding.UTF8.GetString(decryptedDeviceData);
					var deviceResponse = DecodeResponse<ServiceV3AuthsGetResponseDevice>(decryptedDeviceDataString);
					return new ServerSentEventAuthorizationResponse(
						requestingEntity,
						EntityIdentifier.FromString(jwtData.Subject).Id,
						core.ServiceUserHash,
						core.OrgUserHash,
						core.UserPushId,
						deviceResponse.AuthorizationRequestId,
						deviceResponse.Response,
						deviceResponse.DeviceId,
						deviceResponse.ServicePins
					);
				}
				else
				{
					return _jsonDecoder.DecodeObject<ServerSentEventUserServiceSessionEnd>(body);
				}
			}
			catch (JwtError ex)
			{
				throw new InvalidRequestException("JWT error while processing event", ex);
			}
			catch (JweException ex)
			{
				throw new InvalidRequestException("Unable to decrypt body", ex);
			}
		}
	}
}