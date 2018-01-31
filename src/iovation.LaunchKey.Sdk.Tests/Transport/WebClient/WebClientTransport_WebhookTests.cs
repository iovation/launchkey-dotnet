using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
	/// <summary>
	/// webhooks test, separated from main tests because that was getting cluttered.
	/// </summary>
	[TestClass]
	public class WebClientTransport_WebhookTests
	{
		private Mock<IHttpClient> MakeMockHttpClient(string responseBody = "ok", int responseCode = 200)
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

		private ITransport MakeMockedTransport(IHttpClient httpClient, IJsonEncoder jsonService, JwtClaimsRequest request)
		{
			var crypto = new Mock<ICrypto>();
			crypto.Setup(p => p.LoadRsaPublicKey(It.IsAny<string>()))
				.Returns(new RSACryptoServiceProvider());

			crypto.Setup(p => p.Sha256(It.IsAny<byte[]>())).Returns(new byte[] { 255 });
			crypto.Setup(p => p.Sha384(It.IsAny<byte[]>())).Returns(new byte[] { 255 });
			crypto.Setup(p => p.Sha512(It.IsAny<byte[]>())).Returns(new byte[] { 255 });
			crypto.Setup(p => p.DecryptRSA(It.IsAny<byte[]>(), It.IsAny<RSA>())).Returns(new byte[] { 50, 50, 50 });

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
					ExpiresAt = new DateTime(2100, 1, 1),
					IssuedAt = new DateTime(2017, 1, 1),
					Issuer = "my issuer",
					NotBefore = new DateTime(2017, 1, 1),
					Request = request
				});
			jwtService.Setup(p => p.GetJWTData(It.IsAny<string>())).Returns(new JwtData("my iss", TestConsts.DefaultServiceEntity.ToString(), TestConsts.DefaultServiceEntity.ToString(), "key id"));

			var keyMap = new EntityKeyMap();
			keyMap.AddKey(TestConsts.DefaultServiceEntity, "key id", new RSACryptoServiceProvider());
			keyMap.AddKey(TestConsts.DefaultServiceEntity, "key",    new RSACryptoServiceProvider());

			return new WebClientTransport(
				httpClient,
				crypto.Object,
				new HashCache(),
				"https://api.launchkey.com",
				TestConsts.DefaultServiceEntity,
				jwtService.Object,
				jweService.Object,
				3600,
				3600,
				keyMap,
				jsonService
			);
		}

		private Mock<IJsonEncoder> MakeMockedJsonService()
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
					AuthorizationRequestId = TestConsts.DefaultAuthenticationId,
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

		[TestMethod]
		public void HandleServerSentEvent_ShouldHandleAuthPackage()
		{
			var transport = MakeMockedTransport(MakeMockHttpClient().Object, MakeMockedJsonService().Object, new JwtClaimsRequest
			{
				ContentHash = "ff", 
				ContentHashAlgorithm = "S256",
				Method = "POST",
				Path = "/webhook"
			});

			var response = transport.HandleServerSentEvent(new Dictionary<string, List<string>>
			{
				{"X-IOV-JWT", new List<string> { "jwt" }},
				{"Content-Type", new List<string> {"application/jose" }}
			}, "body");

			Assert.IsTrue(response is ServerSentEventAuthorizationResponse);
		}

		[TestMethod]
		public void HandleServerSentEvent_ShouldHandleSessionEnd()
		{
			var transport = MakeMockedTransport(MakeMockHttpClient().Object, MakeMockedJsonService().Object, new JwtClaimsRequest
			{
				ContentHash = "ff",
				ContentHashAlgorithm = "S256",
				Method = "POST",
				Path = "/webhook"
			});
			var pkey = new RSACryptoServiceProvider();
			var jwtService = new JwtService(new UnixTimeConverter(), "lka", new Dictionary<string, RSA>
				{
					{"key", pkey}
				},
				"key", 5);

			var reqId = Guid.NewGuid();
			var jwt = jwtService.Encode(reqId.ToString("D"),
				TestConsts.DefaultServiceEntity.ToString(),
				TestConsts.DefaultServiceEntity.ToString(),
				DateTime.Now,
				"POST",
				"/webhook",
				"S256",
				"ff");

			var response = transport.HandleServerSentEvent(new Dictionary<string, List<string>>
			{
				{"X-IOV-JWT", new List<string> { jwt }},
				{"Content-Type", new List<string> {"application/json" }}
			}, "body");

			Assert.IsTrue(response is ServerSentEventUserServiceSessionEnd);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidRequestException))]
		public void HandleServerSentEvent_PrivateClaims_ShouldValidateHash()
		{
			var transport = MakeMockedTransport(MakeMockHttpClient().Object, MakeMockedJsonService().Object, new JwtClaimsRequest
			{
				ContentHash = "fe", /* our mock crypto lib will always produce 'ff' */
				ContentHashAlgorithm = "S256",
				Method = "POST",
				Path = "/webhook"
			});

			var response = transport.HandleServerSentEvent(new Dictionary<string, List<string>>
			{
				{"X-IOV-JWT", new List<string> { "my-jwt" }},
				{"Content-Type", new List<string> {"application/json" }}
			}, "body");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidRequestException))]
		public void HandleServerSentEvent_PrivateClaims_ShouldValidateMethod()
		{
			// tests whether the POST and GET mismatch gets caught
			var transport = MakeMockedTransport(MakeMockHttpClient().Object, MakeMockedJsonService().Object, new JwtClaimsRequest
			{
				ContentHash = "ff", /* our mock crypto lib will always produce 'ff' */
				ContentHashAlgorithm = "S256",
				Method = "POST",
				Path = "/webhook"
			});

			var response = transport.HandleServerSentEvent(new Dictionary<string, List<string>>
			{
				{"X-IOV-JWT", new List<string> { "my-jwt" }},
				{"Content-Type", new List<string> {"application/json" }}
			}, "body", "GET", "/webhook");
		}


		[TestMethod]
		[ExpectedException(typeof(InvalidRequestException))]
		public void HandleServerSentEvent_PrivateClaims_ShouldValidatePath()
		{
			// tests whether the path mismatch gets caught
			var transport = MakeMockedTransport(MakeMockHttpClient().Object, MakeMockedJsonService().Object, new JwtClaimsRequest
			{
				ContentHash = "ff", /* our mock crypto lib will always produce 'ff' */
				ContentHashAlgorithm = "S256",
				Method = "POST",
				Path = "/webhook"
			});

			var response = transport.HandleServerSentEvent(new Dictionary<string, List<string>>
			{
				{"X-IOV-JWT", new List<string> { "my-jwt" }},
				{"Content-Type", new List<string> {"application/json" }}
			}, "body", "POST", "/webhookwrong");
		}

		[TestMethod]
		public void HandleServerSentEvent_PrivateClaims_ShouldIgnoreNullParameters()
		{
			// tests whether the path mismatch gets caught
			var transport = MakeMockedTransport(MakeMockHttpClient().Object, MakeMockedJsonService().Object, new JwtClaimsRequest
			{
				ContentHash = "ff", /* our mock crypto lib will always produce 'ff' */
				ContentHashAlgorithm = "S256",
				Method = "POST",
				Path = "/webhook"
			});

			var response = transport.HandleServerSentEvent(new Dictionary<string, List<string>>
			{
				{"X-IOV-JWT", new List<string> { "my-jwt" }},
				{"Content-Type", new List<string> {"application/json" }}
			}, "body", null, null);

			Assert.IsTrue(response is ServerSentEventUserServiceSessionEnd);
		}
	}
}
