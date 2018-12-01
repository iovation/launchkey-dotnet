using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Cache;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Crypto.Jwe;
using iovation.LaunchKey.Sdk.Crypto.Jwt;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Time;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using iovation.LaunchKey.Sdk.Transport.WebClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace iovation.LaunchKey.Sdk.Tests.Transport.WebClient
{
	[TestClass]
	public class WebClientTransportTests
	{
		protected string BaseUrl = "https://api.launchkey.com";

		protected Mock<IHttpClient> GetBadNetworkHttpClient()
		{
			var mock = new Mock<IHttpClient>();

			mock.Setup(e => e.ExecuteRequest(
				It.IsAny<HttpMethod>(), 
				It.IsAny<string>(), 
				It.IsAny<string>(),
				It.IsAny<Dictionary<string,string>>()
			)).Throws<IOException>();

			return mock;
		}

		protected WebClientTransport GetTransport(IHttpClient httpClient)
		{
			var pkey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAkWY1n5mCrzLjys1zijQwhTeZwXMajWg7kE4KD6m7KBCtkqoO
nL6rnlRELsWpKsKujFjWIqE4V2rJEV+ctZK/xThFgEURYQ8Tl5QLJN8anp+9eot8
E9s5A8zjfBf3lq+BmzDJDOaCg+ZgmoPbx6t0t6Y76kSMuP1UoRp4B/OFB1WETDQM
rzc8vwgJb+LC9fDHYIMk3aKbYVI1FHYPC0QGw1EgfmpqYnWkhDb8y9YZMMaVAYQF
YCrELy77DBEhkGFoyIlbHJA8ZMSJgMNeY3Soq2dQRcwseKCSy0h7ugB/10+Hst2N
oDLMjXw3IevbUisQoeE32kJm0jTg2CgjfNXE4wIDAQABAoIBAHrilH2QA3hJHWbA
n1713vAoXsW1n4JVzsmWe0Bjpi8lcV1cZr8pEujctUeGfQQLx/QL/OcFtRWExezu
DwcSwQtRFMRlG1wZnuhkEcsW2Gup/D0++B3cEOLaXoT5yKJNMM/VuoYxp6sMoSbW
PIETsrCoSUkkqH6MdOM3+KxXr13TLb5uwdW+7XCuJZhqtpW1BfXyLsODfTJ9dTeK
HCbsaAc8aYb3T++I2VZ1CnjY65u5Ve7EEmKhmdAwPAkv3nAV2M/WejnhvM5WxEIF
e4XLrl/BGGHrO23iJfsh4U0JiqiRKK+pZ5/OS0IjClHOcAqXPpXlAOAl60EB8sDi
prLht2kCgYEAv8FR0IGl8+wXt+y8enj+wRw77keX60amZI0BP/6T2DG4Iifbeutz
FEjZjr9lQRucbguYQZQvNCUD3bcJhLycKjnWlLx1rFYiKr+InVGjdxDgTXytmfXv
5cwjtuPutjLmtSB6G7eyjT541nAY5eLgUsQmG6GJGjuXFbk68E/hul8CgYEAwhz7
oFX++WBG6rcJppd8ZSh+22qxM2ujb74rFkSA+ElkmLvuJWcUWCKP9NKLrSzNFC56
hPf6ahTV32W3D1yV2jH8uD13vaRl81Ibf6joyZ1NjlLzF8f80kxhTv9sBYXCFoE0
N65bv4pBOH1j5W8JU+VENG4e/yPmphwYmw3Ri/0CgYEAqzlcbOrX3O88XqetxU61
bIphmWukZBo6Ch8+tn4EVlBPM3CTszb7E2nZmnWdXH3jOQBxfF1tqQpEYX5HqZch
ezMaQMXn7XMcHz/YJWH4rSEMqSRjf87z4CZg3ba6OUdKawINByMI9MaL8C84jE4C
MOWlgZbEbgA38NXtoxgxX9ECgYBoAnehlfKlVL0rDyCQryLsYSJ4F1cTol1UhqU3
rUUdxNWuMSSNzgnMn9ha6mYFSLgqVC06ClWXecqPhUTtakKxQ4+SeP0sFGa8VgZ+
BNeXED56QGAZIgOJ+3s4hQwweVdiD6EXJMnse/wudKGOUkzBM1u0bD0XoPj5kJxu
mJzrUQKBgQCkUZ3gzA90Pf3dogYn0mGfTj4hY/+qn3tGtc0CFrzGHdIEFnJi5WlO
r+ppoKXL4tSFNDsShVSQBgEAZU9F/F7wUgCXqBMnHu574fpKakiNpeTmLnQ4JNn4
PmRoieUCtxxvmnckMGk4ub+/X4AJHb0ErqavEbIrrBNLW4ahtrJC5g==
-----END RSA PRIVATE KEY-----";
			var crypto = new BouncyCastleCrypto();
			var keyDict = new Dictionary<string, RSA>
			{
				{"my pkey", crypto.LoadRsaPrivateKey(pkey) }
			};
			var keyMap = new EntityKeyMap();
			var jwtService = new JwtService(new UnixTimeConverter(), "lka", keyDict, "my pkey", 5);
			return new WebClientTransport(
				httpClient,
				new BouncyCastleCrypto(),
				new HashCache(),
				BaseUrl,
				new EntityIdentifier(EntityType.Organization, Guid.Parse("d289298e-dacd-11e7-b097-469158467b1a")),
				jwtService,
				new JweService(keyDict["my pkey"]),
				3600,
				300,
				keyMap,
				new JsonNetJsonEncoder()
			);

		}
		
		protected string MakeUrl(string path)
		{
			return BaseUrl + path;
		}

		[TestMethod]
		[ExpectedException(typeof(CommunicationErrorException))]
		public void PublicV3PingGet_ShouldThrowCommunicationError()
		{
			// setup
			var transport = GetTransport(GetBadNetworkHttpClient().Object);

			// exec
			transport.PublicV3PingGet();
		}

		[TestMethod]
		public void PublicV3PingGet_ShouldParseResponse()
		{
			// setup
			var mock = new Mock<IHttpClient>();
			mock.Setup(
				h => h.ExecuteRequest(HttpMethod.GET, MakeUrl("/public/v3/ping"), null, null)
			).Returns(new HttpResponse
			{
				ResponseBody = "{\"api_time\": \"2017-12-09T17:51:06Z\"}",
				StatusCode = System.Net.HttpStatusCode.OK
			});
			var transport = GetTransport(mock.Object);

			// exec
			var pingResponse = transport.PublicV3PingGet();
			Assert.AreEqual(new DateTime(2017, 12, 9, 17, 51, 6, DateTimeKind.Utc), pingResponse.ApiTime);
		}

		[TestMethod]
		[ExpectedException(typeof(CommunicationErrorException))]
		public void ServiceV3AuthsPost_ShouldThrowCommunicationError()
		{
			// setup
			var transport = GetTransport(GetBadNetworkHttpClient().Object);
			var request = new ServiceV3AuthsPostRequest(null, null, null, null, null, null, null);
			var id = Guid.NewGuid();
			var entity = new EntityIdentifier(EntityType.Service, id);

			// exec
			transport.ServiceV3AuthsPost(request, entity);
		}

		[TestMethod]
		[ExpectedException(typeof(CommunicationErrorException))]
		public void PublicV3PublicKeyGet_TestNetworkHandling()
		{
			// arrange
			var transport = GetTransport(GetBadNetworkHttpClient().Object);

			// exec
			transport.PublicV3PublicKeyGet(null);
		}

		
		[TestMethod]
		public void PublicV3PublicKeyGet_TestResponseParse()
		{
			// setup
			var mock = new Mock<IHttpClient>();
			mock.Setup(
				h => h.ExecuteRequest(HttpMethod.GET, MakeUrl("/public/v3/public-key"), null, null)
			).Returns(new HttpResponse
			{
				ResponseBody = "-----BEGIN PUBLIC KEY-----\nMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAinATCdbqz0oDfcUtjzrx\nvF9JNJOrZzBNCmTUpOz/VptDWpraj040eoywD3VRklmMVFt0e77Hs34BsrhchCav\nmzlQmYYjL4zIzRX4B0l+U/PhC6p6RIL8D/TSk11u11sHtBycSOThYDeoPRuBo/Zq\ng3rVvsYdjQ56RLEgI9JkXM5xJWEPgRE2NcCMCBjEQu3icWKUsu5boo4vT33ZhOMU\nCDrajXshXvCxrp6JSb3jvoWC/lIpcDomtDnj/u9GXivsGv3Vk8YjmFlTEnr5Kb/o\n3uSlCFO9bLfEGEhlBULyOeN7m2NKFvFXqfbd4hdtVbEQWBc+te9hLfAF6n13wURk\nqF23lpEZCLcvql4mq/38u+MlgHshaOfYuGN5lPLZn4pRLUPPGS+Q1dYEVirLzWJx\n1Ztn7Ti8qe3ePbXHF2W/+9T+udhROQNv3pJsGp7dxG3WxZB2l16v2cir0nv+jZti\nJaXPf+seoEup2RckvCWhalpnUeXSJE339CkFAN1uTkvXgMWr5XRNuxBsRhz8pnLT\nTxrmsAS6Onkyjhl/+ihxJasCTpN69jmwqxSFNmStzXFz6LjqUtiPIeMdiCn9dFrD\nGb2x+XCOpvFR9q+9RPP/bZxnJPmSPbQEcrjwhLerDL9qbwgHnGYXdlM9JaYYkG5y\n2ZzlVAZOwr81Y9KxOGFq+w8CAwEAAQ==\n-----END PUBLIC KEY-----",
				StatusCode = System.Net.HttpStatusCode.OK,
				Headers = new System.Net.WebHeaderCollection()
				{
					{"X-IOV-KEY-ID", "d2:8e:16:91:39:5b:9d:24:73:0e:36:0a:9a:ef:7e:de"}
				}
			});
			var transport = GetTransport(mock.Object);

			// exec
			var response = transport.PublicV3PublicKeyGet(null);
			Assert.AreEqual(
				response.PublicKey,
				"-----BEGIN PUBLIC KEY-----\nMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAinATCdbqz0oDfcUtjzrx\nvF9JNJOrZzBNCmTUpOz/VptDWpraj040eoywD3VRklmMVFt0e77Hs34BsrhchCav\nmzlQmYYjL4zIzRX4B0l+U/PhC6p6RIL8D/TSk11u11sHtBycSOThYDeoPRuBo/Zq\ng3rVvsYdjQ56RLEgI9JkXM5xJWEPgRE2NcCMCBjEQu3icWKUsu5boo4vT33ZhOMU\nCDrajXshXvCxrp6JSb3jvoWC/lIpcDomtDnj/u9GXivsGv3Vk8YjmFlTEnr5Kb/o\n3uSlCFO9bLfEGEhlBULyOeN7m2NKFvFXqfbd4hdtVbEQWBc+te9hLfAF6n13wURk\nqF23lpEZCLcvql4mq/38u+MlgHshaOfYuGN5lPLZn4pRLUPPGS+Q1dYEVirLzWJx\n1Ztn7Ti8qe3ePbXHF2W/+9T+udhROQNv3pJsGp7dxG3WxZB2l16v2cir0nv+jZti\nJaXPf+seoEup2RckvCWhalpnUeXSJE339CkFAN1uTkvXgMWr5XRNuxBsRhz8pnLT\nTxrmsAS6Onkyjhl/+ihxJasCTpN69jmwqxSFNmStzXFz6LjqUtiPIeMdiCn9dFrD\nGb2x+XCOpvFR9q+9RPP/bZxnJPmSPbQEcrjwhLerDL9qbwgHnGYXdlM9JaYYkG5y\n2ZzlVAZOwr81Y9KxOGFq+w8CAwEAAQ==\n-----END PUBLIC KEY-----",
				"Key should match what the server returns"
			);
			Assert.AreEqual(
				response.PublicKeyFingerPrint,
				"d2:8e:16:91:39:5b:9d:24:73:0e:36:0a:9a:ef:7e:de",
				"Key thumbprint should match what the server returns"
			);
		}


		[TestMethod]
		[ExpectedException(typeof(InvalidResponseException))]
		public void PublicV3PublicKeyGet_TestMissingHeader()
		{
			// setup
			var mock = new Mock<IHttpClient>();
			mock.Setup(
				h => h.ExecuteRequest(HttpMethod.GET, MakeUrl("/public/v3/public-key"), null, null)
			).Returns(new HttpResponse
			{
				ResponseBody = "-----BEGIN PUBLIC KEY-----\nMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAinATCdbqz0oDfcUtjzrx\nvF9JNJOrZzBNCmTUpOz/VptDWpraj040eoywD3VRklmMVFt0e77Hs34BsrhchCav\nmzlQmYYjL4zIzRX4B0l+U/PhC6p6RIL8D/TSk11u11sHtBycSOThYDeoPRuBo/Zq\ng3rVvsYdjQ56RLEgI9JkXM5xJWEPgRE2NcCMCBjEQu3icWKUsu5boo4vT33ZhOMU\nCDrajXshXvCxrp6JSb3jvoWC/lIpcDomtDnj/u9GXivsGv3Vk8YjmFlTEnr5Kb/o\n3uSlCFO9bLfEGEhlBULyOeN7m2NKFvFXqfbd4hdtVbEQWBc+te9hLfAF6n13wURk\nqF23lpEZCLcvql4mq/38u+MlgHshaOfYuGN5lPLZn4pRLUPPGS+Q1dYEVirLzWJx\n1Ztn7Ti8qe3ePbXHF2W/+9T+udhROQNv3pJsGp7dxG3WxZB2l16v2cir0nv+jZti\nJaXPf+seoEup2RckvCWhalpnUeXSJE339CkFAN1uTkvXgMWr5XRNuxBsRhz8pnLT\nTxrmsAS6Onkyjhl/+ihxJasCTpN69jmwqxSFNmStzXFz6LjqUtiPIeMdiCn9dFrD\nGb2x+XCOpvFR9q+9RPP/bZxnJPmSPbQEcrjwhLerDL9qbwgHnGYXdlM9JaYYkG5y\n2ZzlVAZOwr81Y9KxOGFq+w8CAwEAAQ==\n-----END PUBLIC KEY-----",
				StatusCode = System.Net.HttpStatusCode.OK,
				Headers = new System.Net.WebHeaderCollection()
			});
			var transport = GetTransport(mock.Object);

			// exec
			var response = transport.PublicV3PublicKeyGet(null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidResponseException))]
		public void PublicV3PublicKeyGet_TestMissingBody()
		{
			// setup
			var mock = new Mock<IHttpClient>();
			mock.Setup(
				h => h.ExecuteRequest(HttpMethod.GET, MakeUrl("/public/v3/public-key"), null, null)
			).Returns(new HttpResponse
			{
				ResponseBody = null,
				StatusCode = System.Net.HttpStatusCode.OK,
				Headers = new System.Net.WebHeaderCollection()
				{
					{"X-IOV-KEY-ID", "d2:8e:16:91:39:5b:9d:24:73:0e:36:0a:9a:ef:7e:de"}
				}
			});
			var transport = GetTransport(mock.Object);

			// exec
			var response = transport.PublicV3PublicKeyGet(null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidResponseException))]
		public void PublicV3PublicKeyGet_TestEmptyBody()
		{
			// setup
			var mock = new Mock<IHttpClient>();
			mock.Setup(
				h => h.ExecuteRequest(HttpMethod.GET, MakeUrl("/public/v3/public-key"), null, null)
			).Returns(new HttpResponse
			{
				ResponseBody = "",
				StatusCode = System.Net.HttpStatusCode.OK,
				Headers = new System.Net.WebHeaderCollection()
				{
					{"X-IOV-KEY-ID", "d2:8e:16:91:39:5b:9d:24:73:0e:36:0a:9a:ef:7e:de"}
				}
			});
			var transport = GetTransport(mock.Object);

			// exec
			transport.PublicV3PublicKeyGet(null);
		}


		[TestMethod]
		public void PublicV3PublicKeyGet_ShouldCallCorrectPath()
		{
			// setup
			var mock = new Mock<IHttpClient>();
			mock.Setup(
				h => h.ExecuteRequest(HttpMethod.GET, MakeUrl("/public/v3/public-key"), null, null)
			).Returns(new HttpResponse
			{
				ResponseBody = "key here",
				StatusCode = System.Net.HttpStatusCode.OK,
				Headers = new System.Net.WebHeaderCollection()
				{
					{"X-IOV-KEY-ID", "id here"}
				}
			});
			var transport = GetTransport(mock.Object);

			transport.PublicV3PublicKeyGet(null);

			mock.Verify(p => p.ExecuteRequest(HttpMethod.GET, MakeUrl("/public/v3/public-key"), null, null));
		}

		[TestMethod]
		public void PublicV3PublicKeyGet_ShouldCallCorrectPathForCustomKeyId()
		{
			// setup
			var mock = new Mock<IHttpClient>();
			mock.Setup(
				h => h.ExecuteRequest(HttpMethod.GET, MakeUrl("/public/v3/public-key/keyid"), null, null)
			).Returns(new HttpResponse
			{
				ResponseBody = "key here",
				StatusCode = System.Net.HttpStatusCode.OK,
				Headers = new System.Net.WebHeaderCollection()
				{
					{"X-IOV-KEY-ID", "id here"}
				}
			});
			var transport = GetTransport(mock.Object);

			transport.PublicV3PublicKeyGet("keyid");

			mock.Verify(p => p.ExecuteRequest(HttpMethod.GET, MakeUrl("/public/v3/public-key/keyid"), null, null));
		}

		protected Mock<IHttpClient> MakeMockHttpClient(string responseBody = "ok", int responseCode = 200)
		{
			var mock = new Mock<IHttpClient>();
			mock.Setup(h => h.ExecuteRequest(
				It.IsAny<HttpMethod>(),
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<Dictionary<string, string>>()
			)).Returns(new HttpResponse
			{
				Headers = new System.Net.WebHeaderCollection
				{
					{"X-IOV-KEY-ID", "key id"},
					{"X-IOV-JWT", "my jwt" }
				},
				ResponseBody = responseBody,
				StatusCode = (HttpStatusCode)responseCode,
				StatusDescription = "OK"
			});
			return mock;
		}

		protected ITransport MakeMockedTransport(IHttpClient httpClient, IJsonEncoder jsonService, int responseCode = 200, string hashAlgo = "S256", string hashValue = "ff", JwtClaimsResponse response = null)
		{
			if (response == null)
			{
				response = new JwtClaimsResponse
				{
					StatusCode = responseCode,
					ContentHashAlgorithm = hashAlgo,
					ContentHash = hashValue
				};
			}

			var crypto = new Mock<ICrypto>();
			crypto.Setup(p => p.LoadRsaPublicKey(It.IsAny<string>()))
				.Returns(new RSACryptoServiceProvider());

			crypto.Setup(p => p.Sha256(It.IsAny<byte[]>())).Returns(new byte[] {255});
			crypto.Setup(p => p.Sha384(It.IsAny<byte[]>())).Returns(new byte[] {255});
			crypto.Setup(p => p.Sha512(It.IsAny<byte[]>())).Returns(new byte[] {255});
			crypto.Setup(p => p.DecryptRSA(It.IsAny<byte[]>(), It.IsAny<RSA>())).Returns(new byte[] {50, 50, 50});

			var jweService = new Mock<IJweService>();
			jweService.Setup(p => p.Decrypt(It.IsAny<string>())).Returns("dec");
			jweService.Setup(p => p.GetHeaders(It.IsAny<string>())).Returns(new Dictionary<string, string>()
			{
				{"kid", "key id"}
			});

			var jwtService = new Mock<IJwtService>();
			jwtService.Setup(p => p.Decode(It.IsAny<RSA>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()))
				.Returns(new JwtClaims
				{
					Audience = "my aud",
					ExpiresAt = new DateTime(2100,1,1),
					IssuedAt = new DateTime(2017,1,1),
					Issuer = "my issuer",
					NotBefore = new DateTime(2017, 1,1),
					Response = response
				});
			jwtService.Setup(p => p.GetJWTData(It.IsAny<string>())).Returns(new JwtData("my iss", TestConsts.DefaultServiceEntity.ToString(), DefaultSubject.ToString(), "key id"));

			var keyMap = new EntityKeyMap();
			keyMap.AddKey(DefaultSubject, "key id", new RSACryptoServiceProvider());
			keyMap.AddKey(TestConsts.DefaultServiceEntity, "key", new RSACryptoServiceProvider());
			
			return new WebClientTransport(
				httpClient,
				crypto.Object,
				new HashCache(),
				"https://api.launchkey.com",
				DefaultIssuer,
				jwtService.Object,
				jweService.Object,
				3600,
				3600,
				keyMap,
				jsonService
			);
		}

		protected Mock<IJsonEncoder> MakeMockedJsonService()
		{
			var jsonService = new Mock<IJsonEncoder>();

			jsonService
				.Setup(p => p.DecodeObject<PublicV3PingGetResponse>(It.IsAny<string>()))
				.Returns(new PublicV3PingGetResponse { ApiTime = new DateTime(2017, 1, 1) });

			jsonService
				.Setup(p => p.DecodeObject<PublicV3PublicKeyGetResponse>(It.IsAny<string>()))
				.Returns(new PublicV3PublicKeyGetResponse("key", "fingerprint"));

			jsonService
				.Setup(p => p.DecodeObject<ServiceV3AuthsGetResponseCore>(It.IsAny<string>()))
				.Returns(new ServiceV3AuthsGetResponseCore
				{
					EncryptedDeviceResponse = "blah",
					OrgUserHash = "ohash",
					PublicKeyId = "key id",
					ServiceUserHash = "shash",
					UserPushId = "push"
				});

			jsonService
				.Setup(p => p.DecodeObject<ServiceV3AuthsGetResponseDevice>(It.IsAny<string>()))
				.Returns(new ServiceV3AuthsGetResponseDevice
				{
					AuthorizationRequestId = DefaultAuthId,
					DeviceId = "arfaf",
					Response = true,
					ServicePins = new string[] { }
				});

			jsonService
				.Setup(p => p.DecodeObject<ServerSentEventUserServiceSessionEnd>(It.IsAny<string>()))
				.Returns(new ServerSentEventUserServiceSessionEnd
				{
					ApiTime = TestConsts.DefaultTime,
					UserHash = "uhash"
				});
			
			return jsonService;
		}

		private ITransport MakeMockedTransportDefault(IHttpClient httpClient)
		{
			return MakeMockedTransport(httpClient, MakeMockedJsonService().Object);
		}

		private void DoApiCallTest(Action<ITransport> callback, HttpMethod expectedMethod, string expectedPath)
		{
			var httpClient = MakeMockHttpClient();
			var transport = MakeMockedTransportDefault(httpClient.Object);
			callback(transport);
			httpClient.Verify(p => p.ExecuteRequest(expectedMethod, MakeUrl(expectedPath), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
		}

		private readonly EntityIdentifier DefaultIssuer = new EntityIdentifier(EntityType.Service, new Guid("00000000-0000-0000-0000-000000000001"));
		private readonly EntityIdentifier DefaultSubject = new EntityIdentifier(EntityType.Service, new Guid("00000000-0000-0000-0000-000000000002"));
		private readonly Guid DefaultAuthId = new Guid("00000000-0000-0000-0000-000000000003");
		private readonly Guid DefaultDeviceId = new Guid("00000000-0000-0000-0000-000000000004");

		[TestMethod]
		public void ServiceV3AuthsGet_ShouldReturnNullIfPending()
		{
			var httpClientMock = MakeMockHttpClient(responseCode: 204);
			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object, 204);
			var response = transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);

			Assert.IsNull(response);
		}
		
		[TestMethod]
		[ExpectedException(typeof(AuthorizationRequestTimedOutError))]
		public void ServiceV3AuthsGet_ShouldThrowIfTimedOut()
		{
			var httpClientMock = MakeMockHttpClient(responseCode: 408);
			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object, 408);
			transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
		}


		[TestMethod]
		public void ServiceV3AuthsPost_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.ServiceV3AuthsPost(new ServiceV3AuthsPostRequest("john", null, null, null, null, null, null), DefaultSubject),
				HttpMethod.POST,
				"/service/v3/auths"
			);
		}

		[TestMethod]
		public void ServiceV3AuthsGet_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.ServiceV3AuthsGet(DefaultAuthId, DefaultSubject),
				HttpMethod.GET, 
				"/service/v3/auths/"+DefaultAuthId
			);
		}

		[TestMethod]
		public void ServiceV3SessionsPost_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.ServiceV3SessionsPost(new ServiceV3SessionsPostRequest("my name", null), DefaultSubject),
				HttpMethod.POST,
				"/service/v3/sessions"
			);
		}

		[TestMethod]
		public void ServiceV3SessionsDelete_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.ServiceV3SessionsDelete(new ServiceV3SessionsDeleteRequest("my name"), DefaultSubject),
				HttpMethod.DELETE,
				"/service/v3/sessions"
			);
		}

		[TestMethod]
		public void DirectoryV3DevicesPost_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.DirectoryV3DevicesPost(new DirectoryV3DevicesPostRequest("my id"), DefaultSubject),
				HttpMethod.POST,
				"/directory/v3/devices"
			);
		}

		[TestMethod]
		public void DirectoryV3DevicesDelete_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.DirectoryV3DevicesDelete(new DirectoryV3DevicesDeleteRequest("my id", DefaultDeviceId), DefaultSubject),
				HttpMethod.DELETE,
				"/directory/v3/devices"
			);
		}

		[TestMethod]
		public void DirectoryV3DevicesListPost_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.DirectoryV3DevicesListPost(new DirectoryV3DevicesListPostRequest("my id"), DefaultSubject),
				HttpMethod.POST,
				"/directory/v3/devices/list"
			);
		}

		[TestMethod]
		public void DirectoryV3SessionsListPost_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.DirectoryV3SessionsListPost(new DirectoryV3SessionsListPostRequest("my id"), DefaultSubject),
				HttpMethod.POST,
				"/directory/v3/sessions/list"
			);
		}

		[TestMethod]
		public void DirectoryV3SessionsDelete_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.DirectoryV3SessionsDelete(new DirectoryV3SessionsDeleteRequest("my id"), DefaultSubject),
				HttpMethod.DELETE,
				"/directory/v3/sessions"
			);
		}

		[TestMethod]
		public void OrganizationV3ServicesPost_ShouldCallApi()
		{
			var request = new ServicesPostRequest(
				"Super service",
				"Description", 
				new Uri("http://moreimportanter.tld/icon.ico"), 
				new Uri("http://veryimportanturl.tld"),
				true
			);
			DoApiCallTest(
				t => t.OrganizationV3ServicesPost(request, TestConsts.DefaultOrganizationEntity), 
				HttpMethod.POST, 
				"/organization/v3/services"
			);
		}

		[TestMethod]
		public void OrganizationV3ServicesPatch_ShouldCallApi()
		{
			var request = new ServicesPatchRequest(TestConsts.DefaultServiceId, "Test service", "Test desc", new Uri("http://e.com/i"), new Uri("http://e.com/cb"), true);
			DoApiCallTest(
				t => t.OrganizationV3ServicesPatch(request, TestConsts.DefaultOrganizationEntity),
				HttpMethod.PATCH,
				"/organization/v3/services"
			);
		}

		[TestMethod]
		public void OrganizationV3ServicesListPost_ShouldCallApi()
		{
			var request = new ServicesListPostRequest(new List<Guid>{Guid.NewGuid()});
			DoApiCallTest(
				t => t.OrganizationV3ServicesListPost(request, TestConsts.DefaultOrganizationEntity),
				HttpMethod.POST,
				"/organization/v3/services/list"
			);
		}

		[TestMethod]
		public void OrganizationV3ServicesGet_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.OrganizationV3ServicesGet(TestConsts.DefaultOrganizationEntity),
				HttpMethod.GET,
				"/organization/v3/services"
			);
		}

		[TestMethod]
		public void OrganizationV3DirectoriesPost_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.OrganizationV3DirectoriesPost(new OrganizationV3DirectoriesPostRequest("dir name"), TestConsts.DefaultOrganizationEntity),
				HttpMethod.POST,
				"/organization/v3/directories"
			);
		}

		[TestMethod]
		public void OrganizationV3DirectoriesListPatch_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.OrganizationV3DirectoriesPatch(new OrganizationV3DirectoriesPatchRequest(TestConsts.DefaultDirectoryId, true, "android", "ioskey"), TestConsts.DefaultOrganizationEntity),
				HttpMethod.PATCH,
				"/organization/v3/directories"
			);
		}

		[TestMethod]
		public void OrganizationV3DirectoriesGet_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.OrganizationV3DirectoriesGet(TestConsts.DefaultOrganizationEntity),
				HttpMethod.GET,
				"/organization/v3/directories"
			);
		}

		[TestMethod]
		public void OrganizationV3DirectoriesListPost_ShouldCallApi()
		{
			DoApiCallTest(
				t => t.OrganizationV3DirectoriesListPost(new OrganizationV3DirectoriesListPostRequest(new List<Guid> {Guid.NewGuid()}), TestConsts.DefaultOrganizationEntity),
				HttpMethod.POST,
				"/organization/v3/directories/list"
			);
		}

		[TestMethod]
		public void DirectoryV3ServicesListPost_ShouldCallApi()
		{
			var dirId = Guid.NewGuid();

			DoApiCallTest(t => t.OrganizationV3DirectorySdkKeysListPost(
				new OrganizationV3DirectorySdkKeysListPostRequest(dirId), TestConsts.DefaultOrganizationEntity),
				HttpMethod.POST,
				"/organization/v3/directory/sdk-keys/list"
			);
		}

		[TestMethod]
		public void OrganizationV3DirectorySdkKeysPost_ShouldCallApi()
		{
			var dirId = Guid.NewGuid();

			DoApiCallTest(t => t.OrganizationV3DirectorySdkKeysPost(
				new OrganizationV3DirectorySdkKeysPostRequest(dirId), TestConsts.DefaultOrganizationEntity),
				HttpMethod.POST,
				"/organization/v3/directory/sdk-keys"
			);
		}

		[TestMethod]
		public void OrganizationV3DirectorySdkKeysDelete_ShouldCallApi()
		{
			var dirId = Guid.NewGuid();
			var keyId = Guid.NewGuid();

			DoApiCallTest(t => t.OrganizationV3DirectorySdkKeysDelete(
				new OrganizationV3DirectorySdkKeysDeleteRequest(dirId, keyId), TestConsts.DefaultOrganizationEntity),
				HttpMethod.DELETE,
				"/organization/v3/directory/sdk-keys"
			);
		}

		[TestMethod]
		public void OrganizationV3ServiceKeysListPost_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();

			DoApiCallTest(t => t.OrganizationV3ServiceKeysListPost(
				new ServiceKeysListPostRequest(svcId), TestConsts.DefaultOrganizationEntity),
				HttpMethod.POST,
				"/organization/v3/service/keys/list"
			);
		}

		[TestMethod]
		public void OrganizationV3ServiceKeysPost_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.OrganizationV3ServiceKeysPost(new ServiceKeysPostRequest(svcId, "pubkey", DateTime.MaxValue, true), TestConsts.DefaultOrganizationEntity),
				HttpMethod.POST,
				"/organization/v3/service/keys"
			);
		}

		[TestMethod]
		public void OrganizationV3ServiceKeysPatch_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.OrganizationV3ServiceKeysPatch(new ServiceKeysPatchRequest(svcId, "pubkey", DateTime.MaxValue, true), TestConsts.DefaultOrganizationEntity),
				HttpMethod.PATCH,
				"/organization/v3/service/keys"
			);
		}

		[TestMethod]
		public void OrganizationV3ServiceKeysDelete_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.OrganizationV3ServiceKeysDelete(new ServiceKeysDeleteRequest(svcId, "key"), TestConsts.DefaultOrganizationEntity),
				HttpMethod.DELETE,
				"/organization/v3/service/keys"
			);
		}

		[TestMethod]
		public void DirectoryV3ServiceKeysListPost_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();

			DoApiCallTest(t => t.DirectoryV3ServiceKeysListPost(
					new ServiceKeysListPostRequest(svcId), TestConsts.DefaultDirectoryEntity),
				HttpMethod.POST,
				"/directory/v3/service/keys/list"
			);
		}

		[TestMethod]
		public void DirectoryV3ServiceKeysPost_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.DirectoryV3ServiceKeysPost(new ServiceKeysPostRequest(svcId, "pubkey", DateTime.MaxValue, true), TestConsts.DefaultDirectoryEntity),
				HttpMethod.POST,
				"/directory/v3/service/keys"
			);
		}

		[TestMethod]
		public void DirectoryV3ServiceKeysPatch_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.DirectoryV3ServiceKeysPatch(new ServiceKeysPatchRequest(svcId, "pubkey", DateTime.MaxValue, true), TestConsts.DefaultDirectoryEntity),
				HttpMethod.PATCH,
				"/directory/v3/service/keys"
			);
		}

		[TestMethod]
		public void DirectoryV3ServiceKeysDelete_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.DirectoryV3ServiceKeysDelete(new ServiceKeysDeleteRequest(svcId, "key"), TestConsts.DefaultDirectoryEntity),
				HttpMethod.DELETE,
				"/directory/v3/service/keys"
			);
		}


		[TestMethod]
		public void OrganizationV3DirectoryKeysListPost_ShouldCallApi()
		{
			var dirId = Guid.NewGuid();

			DoApiCallTest(t => t.OrganizationV3DirectoryKeysListPost(
					new DirectoryKeysListPostRequest(dirId), TestConsts.DefaultOrganizationEntity),
				HttpMethod.POST,
				"/organization/v3/directory/keys/list"
			);
		}

		[TestMethod]
		public void OrganizationV3DirectoryKeysPost_ShouldCallApi()
		{
			var dirId = Guid.NewGuid();
			DoApiCallTest(
				t => t.OrganizationV3DirectoryKeysPost(new DirectoryKeysPostRequest(dirId, "pubkey", DateTime.MaxValue, true), TestConsts.DefaultOrganizationEntity),
				HttpMethod.POST,
				"/organization/v3/directory/keys"
			);
		}

		[TestMethod]
		public void OrganizationV3DirectoryKeysPatch_ShouldCallApi()
		{
			var dirId = Guid.NewGuid();
			DoApiCallTest(
				t => t.OrganizationV3DirectoryKeysPatch(new DirectoryKeysPatchRequest(dirId, "pubkey", DateTime.MaxValue, true), TestConsts.DefaultOrganizationEntity),
				HttpMethod.PATCH,
				"/organization/v3/directory/keys"
			);
		}

		[TestMethod]
		public void OrganizationV3DirectoryKeysDelete_ShouldCallApi()
		{
			var dirId = Guid.NewGuid();
			DoApiCallTest(
				t => t.OrganizationV3DirectoryKeysDelete(new DirectoryKeysDeleteRequest(dirId, "key"), TestConsts.DefaultOrganizationEntity),
				HttpMethod.DELETE,
				"/organization/v3/directory/keys"
			);
		}

		[TestMethod]
		public void OrganizationV3ServicePolicyPut_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.OrganizationV3ServicePolicyPut(
					new ServicePolicyPutRequest(svcId, new AuthPolicy(null,null,null,null,null,null)),
					TestConsts.DefaultOrganizationEntity
				),
				HttpMethod.PUT,
				"/organization/v3/service/policy"
			);
		}

		[TestMethod]
		public void OrganizationV3ServicePolicyItemPost_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.OrganizationV3ServicePolicyItemPost(
					new ServicePolicyItemPostRequest(svcId),
					TestConsts.DefaultOrganizationEntity
				),
				HttpMethod.POST,
				"/organization/v3/service/policy/item"
			);
		}

		[TestMethod]
		public void OrganizationV3ServicePolicyDelete_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.OrganizationV3ServicePolicyDelete(new ServicePolicyDeleteRequest(svcId), TestConsts.DefaultOrganizationEntity),
				HttpMethod.DELETE,
				"/organization/v3/service/policy"
			);
		}

		[TestMethod]
		public void DirectoryV3ServicePolicyPut_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.DirectoryV3ServicePolicyPut(
					new ServicePolicyPutRequest(svcId, new AuthPolicy(null, null, null, null, null, null)),
					TestConsts.DefaultOrganizationEntity
				),
				HttpMethod.PUT,
				"/directory/v3/service/policy"
			);
		}
		
		[TestMethod]
		public void DirectoryV3ServicePolicyItemPost_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.DirectoryV3ServicePolicyItemPost(
					new ServicePolicyItemPostRequest(svcId),
					TestConsts.DefaultOrganizationEntity
				),
				HttpMethod.POST,
				"/directory/v3/service/policy/item"
			);
		}

		[TestMethod]
		public void DirectoryV3ServicePolicyDelete_ShouldCallApi()
		{
			var svcId = Guid.NewGuid();
			DoApiCallTest(
				t => t.DirectoryV3ServicePolicyDelete(new ServicePolicyDeleteRequest(svcId), TestConsts.DefaultOrganizationEntity),
				HttpMethod.DELETE,
				"/directory/v3/service/policy"
			);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidResponseException))]
		public void PrivateClaims_VerifyResponseCode()
		{
			var httpClientMock = MakeMockHttpClient(responseCode: 200);
			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object, 404);
			transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
		}

		[TestMethod]
		public void PrivateClaims_VerifyHash256()
		{
			var httpClientMock = MakeMockHttpClient(responseCode: 204);
			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object, 204, "S256", "fe");

			try
			{
				transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
				Assert.Fail();
			}
			catch (InvalidResponseException e)
			{
				Assert.IsInstanceOfType(e.InnerException, typeof(JwtError));
				Assert.AreEqual(e.InnerException.Message, "Hash of response content does not match JWT response hash");
			}
		}

		[TestMethod]
		public void PrivateClaims_VerifyHash384()
		{
			var httpClientMock = MakeMockHttpClient(responseCode: 204);
			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object, 204, "S384", "fe");

			try
			{
				transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
				Assert.Fail();
			}
			catch (InvalidResponseException e)
			{
				Assert.IsInstanceOfType(e.InnerException, typeof(JwtError));
				Assert.AreEqual(e.InnerException.Message, "Hash of response content does not match JWT response hash");
			}
		}

		[TestMethod]
		public void PrivateClaims_VerifyHash512()
		{
			var httpClientMock = MakeMockHttpClient(responseCode: 204);
			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object, 204, "S512", "fe");

			try
			{
				transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
				Assert.Fail();
			}
			catch (InvalidResponseException e)
			{
				Assert.IsInstanceOfType(e.InnerException, typeof(JwtError));
				Assert.AreEqual(e.InnerException.Message, "Hash of response content does not match JWT response hash");
			}
		}

		[TestMethod]
		public void PrivateClaims_LocationHeaderPresentButNotInJwt_ShouldThrow()
		{
			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(h => h.ExecuteRequest(
				It.IsAny<HttpMethod>(),
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<Dictionary<string, string>>()
			)).Returns(new HttpResponse
			{
				Headers = new System.Net.WebHeaderCollection
				{
					{"X-IOV-KEY-ID", "key id"},
					{"X-IOV-JWT", "my jwt" },
					{"Location", "http://badguys.com" }
				},
				ResponseBody = "basic key data",
				StatusCode = HttpStatusCode.OK,
				StatusDescription = "OK"
			});

			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object);

			try
			{
				transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
				Assert.Fail();
			}
			catch (InvalidResponseException e)
			{
				Assert.IsInstanceOfType(e.InnerException, typeof(JwtError));
				Assert.AreEqual(e.InnerException.Message, "Location header of response content does not match JWT response location");
			}
		}

		[TestMethod]
		public void PrivateClaims_CacheControlHeaderPresentButNotInJwt_ShouldThrow()
		{
			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(h => h.ExecuteRequest(
				It.IsAny<HttpMethod>(),
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<Dictionary<string, string>>()
			)).Returns(new HttpResponse
			{
				Headers = new System.Net.WebHeaderCollection
				{
					{"X-IOV-KEY-ID", "key id"},
					{"X-IOV-JWT", "my jwt" },
					{"Cache-control", "nefarious value" }
				},
				ResponseBody = "response",
				StatusCode = HttpStatusCode.OK,
				StatusDescription = "OK"
			});

			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object);

			try
			{
				transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
				Assert.Fail();
			}
			catch (InvalidResponseException e)
			{
				Assert.IsInstanceOfType(e.InnerException, typeof(JwtError));
				Assert.AreEqual(e.InnerException.Message, "Cache-Control header of response content does not match JWT response cache");
			}
		}

		[TestMethod]
		public void PrivateClaims_CacheControlHeaderMismatch_ShouldThrow()
		{
			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(h => h.ExecuteRequest(
				It.IsAny<HttpMethod>(),
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<Dictionary<string, string>>()
			)).Returns(new HttpResponse
			{
				Headers = new System.Net.WebHeaderCollection
				{
					{"X-IOV-KEY-ID", "key id"},
					{"X-IOV-JWT", "my jwt" },
					{"Cache-control", "nefarious value" }
				},
				ResponseBody = "response",
				StatusCode = HttpStatusCode.OK,
				StatusDescription = "OK"
			});

			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object, response: new JwtClaimsResponse
			{
				CacheControlHeader = null,
				ContentHash = "ff",
				ContentHashAlgorithm = "S256"
			});

			try
			{
				transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
				Assert.Fail();
			}
			catch (InvalidResponseException e)
			{
				Assert.IsInstanceOfType(e.InnerException, typeof(JwtError));
				Assert.AreEqual(e.InnerException.Message, "Cache-Control header of response content does not match JWT response cache");
			}
		}


		[TestMethod]
		public void PrivateClaims_LocationHeaderMismatch_ShouldThrow()
		{
			var httpClientMock = new Mock<IHttpClient>();
			httpClientMock.Setup(h => h.ExecuteRequest(
				It.IsAny<HttpMethod>(),
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<Dictionary<string, string>>()
			)).Returns(new HttpResponse
			{
				Headers = new System.Net.WebHeaderCollection
				{
					{"X-IOV-KEY-ID", "key id"},
					{"X-IOV-JWT", "my jwt" },
					{"Location", "http://badguys.com" }
				},
				ResponseBody = "response",
				StatusCode = HttpStatusCode.OK,
				StatusDescription = "OK"
			});

			var httpClient = httpClientMock.Object;
			var transport = MakeMockedTransport(httpClient, MakeMockedJsonService().Object, response: new JwtClaimsResponse
			{
				CacheControlHeader = null,
				LocationHeader = "http://goodguys.com",
				ContentHash = "ff",
				ContentHashAlgorithm = "S256"
			});

			try
			{
				transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
				Assert.Fail();
			}
			catch (InvalidResponseException e)
			{
				Assert.IsInstanceOfType(e.InnerException, typeof(JwtError));
				Assert.AreEqual(e.InnerException.Message, "Location header of response content does not match JWT response location");
			}
		}
	}
}
