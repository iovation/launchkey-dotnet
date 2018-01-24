using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using iovation.LaunchKey.Sdk.Cache;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Crypto.Jwe;
using iovation.LaunchKey.Sdk.Crypto.Jwt;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Time;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using iovation.LaunchKey.Sdk.Transport.WebClient;

namespace iovation.LaunchKey.Sdk
{
	/// <summary>
	/// This class creates the various factories used for interacting with the LaunchKey API.
	/// Creating an instance of this class directly is not necessary -- see <see cref="FactoryFactoryBuilder"/>, which provides
	/// useful defaults.
	/// </summary>
	public class FactoryFactory
	{
		private readonly ICrypto _crypto;
		private readonly IHttpClient _httpClient;
		private readonly ICache _keyCache;
		private readonly string _apiBaseUrl;
		private readonly string _apiIdentifier;
		private readonly int _requestExpireSeconds;
		private readonly int _offsetTtl;
		private readonly int _currentPublicKeyTtl;
		private readonly EntityKeyMap _entityKeyMap;

		/// <summary>
		/// Create an instance
		/// </summary>
		/// <param name="crypto">The crypto provider to use</param>
		/// <param name="httpClient">The HTTP client to use</param>
		/// <param name="keyCache">The caching provider to use for storing public keys</param>
		/// <param name="apiBaseUrl">The API base URL to use</param>
		/// <param name="apiIdentifier">The API audience identifier to use</param>
		/// <param name="requestExpireSeconds">The default expire time for requests in-flight</param>
		/// <param name="offsetTtl">The amount of time to cache our server-to-client time-drift</param>
		/// <param name="currentPublicKeyTtl">The amount of time to cache the server's public key</param>
		/// <param name="entityKeyMap">A list of private keys tied to entities</param>
		public FactoryFactory(ICrypto crypto, IHttpClient httpClient, ICache keyCache, string apiBaseUrl, string apiIdentifier, int requestExpireSeconds, int offsetTtl, int currentPublicKeyTtl, EntityKeyMap entityKeyMap)
		{
			_crypto = crypto;
			_httpClient = httpClient;
			_keyCache = keyCache;
			_apiBaseUrl = apiBaseUrl;
			_apiIdentifier = apiIdentifier;
			_requestExpireSeconds = requestExpireSeconds;
			_offsetTtl = offsetTtl;
			_currentPublicKeyTtl = currentPublicKeyTtl;
			_entityKeyMap = entityKeyMap;
		}

		/// <summary>
		/// Creates a factory using service credentials. Allows interacting with just services, as service credentials have no child services or directories.
		/// </summary>
		/// <param name="serviceId">The unique service ID of the service</param>
		/// <param name="privateKeyPem">The private key to use. Should be the key itself -- not a path.</param>
		/// <returns>A configured ServiceFactory, ready to create ServiceClients</returns>
		public ServiceFactory MakeServiceFactory(string serviceId, string privateKeyPem)
		{
			var key = _crypto.LoadRsaPrivateKey(privateKeyPem);
			var keys = new Dictionary<string, RSA>();
			var id = new EntityIdentifier(EntityType.Service, Guid.Parse(serviceId));
			var fingerprint = _crypto.GeneratePublicKeyFingerprintFromPrivateKey(key);
			keys.Add(fingerprint, key);
			return new ServiceFactory(MakeTransport(id, keys, fingerprint), Guid.Parse(serviceId));
		}

		/// <summary>
		/// Creates a factory using directory credentials. Allows interacting with the directory itself and any child services within the directory.
		/// </summary>
		/// <param name="directoryId">The unique directory ID of the directory</param>
		/// <param name="privateKeyPem">The private key to use. Should be the key itself -- not a path.</param>
		/// <returns></returns>
		public DirectoryFactory MakeDirectoryFactory(string directoryId, string privateKeyPem)
		{
			var key = _crypto.LoadRsaPrivateKey(privateKeyPem);
			var keys = new Dictionary<string, RSA>();
			var id = new EntityIdentifier(EntityType.Directory, Guid.Parse(directoryId));
			var fingerprint = _crypto.GeneratePublicKeyFingerprintFromPrivateKey(key);
			keys.Add(fingerprint, key);
			return new DirectoryFactory(MakeTransport(id, keys, fingerprint), Guid.Parse(directoryId));
		}

		/// <summary>
		/// Creates a factory using organization-level credentials. allow interacting with any directories or services within the organization.
		/// </summary>
		/// <param name="organizationId">The unique organization ID</param>
		/// <param name="privateKeyPem">The private key to use. Should be the key itself -- not a path.</param>
		/// <returns></returns>
		public OrganizationFactory MakeOrganizationFactory(string organizationId, string privateKeyPem)
		{
			var key = _crypto.LoadRsaPrivateKey(privateKeyPem);
			var keys = new Dictionary<string, RSA>();
			var id = new EntityIdentifier(EntityType.Organization, Guid.Parse(organizationId));
			var fingerprint = _crypto.GeneratePublicKeyFingerprintFromPrivateKey(key);
			keys.Add(fingerprint, key);
			return new OrganizationFactory(MakeTransport(id, keys, fingerprint), Guid.Parse(organizationId));
		}

		private ITransport MakeTransport(
			EntityIdentifier issuer, 
			Dictionary<string, RSA> privateKeys, 
			string currentPrivateKey)
		{
			foreach (var key in privateKeys)
			{
				_entityKeyMap.AddKey(issuer, key.Key, key.Value);
			}
			return new WebClientTransport(
				_httpClient,
				_crypto,
				_keyCache,
				_apiBaseUrl,
				issuer,
				new JwtService(new UnixTimeConverter(), _apiIdentifier, privateKeys, currentPrivateKey, _requestExpireSeconds),
				new JweService(privateKeys[currentPrivateKey]),
				_offsetTtl,
				_currentPublicKeyTtl,
				_entityKeyMap,
				new JsonNetJsonEncoder()
			);
		}
	}
}
