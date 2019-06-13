using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Cache;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Crypto.Jwe;
using iovation.LaunchKey.Sdk.Crypto.Jwt;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using iovation.LaunchKey.Sdk.Transport.WebClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace iovation.LaunchKey.Sdk.Tests.Transport.WebClient
{
    [TestClass]
    public class WebClientTransport_ServiceV3AuthsGetTests
    {
        private string BaseUrl = "https://api.launchkey.com";
        private ITransport Transport;
        private Mock<ICache> PublicKeyCache;
        private Mock<IHttpClient> HttpClient;
        private Mock<IJweService> JweService;
        private Mock<ICrypto> Crypto;
        private Mock<IJsonEncoder> JsonEncoder;
        private EntityIdentifier Issuer;
        private Mock<IJwtService> JwtService;
        private Mock<EntityKeyMap> KeyMap;
        private Mock<HttpResponse> HttpResponse;
        private Mock<JwtClaims> JwtClaims;
        private Mock<JwtClaimsResponse> JwtClaimsResponse;
        private Mock<ServiceV3AuthsGetResponseDevice> DeviceResponse;
        private Mock<ServiceV3AuthsGetResponseCore> AuthsGetResponseCore;

        [TestInitialize]
        public void Initialize()
        {
            HttpResponse = new Mock<HttpResponse>();
            HttpResponse.Object.StatusCode = HttpStatusCode.NoContent;
            HttpResponse.Object.Headers = new WebHeaderCollection
            {
                ["X-IOV-JWT"] = "IOV JWT"
            };
            HttpClient = new Mock<IHttpClient>();
            HttpClient.Setup(client => client.ExecuteRequest(It.IsAny<HttpMethod>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Dictionary<string, String>>()))
                .Returns(HttpResponse.Object);
            Crypto = new Mock<ICrypto>();
            Crypto.Setup(c => c.DecryptRSA(It.IsAny<byte[]>(), It.IsAny<RSA>())).Returns(System.Text.Encoding.ASCII.GetBytes("Decrypted"));
            Crypto.Setup(c => c.Sha256(It.IsAny<byte[]>())).Returns(new byte[] { 255 });
            PublicKeyCache = new Mock<ICache>();
            PublicKeyCache.Setup(c => c.Get(It.IsAny<String>())).Returns("Public Key");
            Issuer = new EntityIdentifier(EntityType.Directory, default(Guid));
            JwtService = new Mock<IJwtService>();
            JwtService.Setup(s => s.Encode(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>()))
                .Returns("JWT Encoded");
            JwtClaims = new Mock<JwtClaims>();
            JwtClaimsResponse = new Mock<JwtClaimsResponse>();
            JwtClaimsResponse.Object.StatusCode = 204;
            JwtClaimsResponse.Object.LocationHeader = null;
            JwtClaimsResponse.Object.CacheControlHeader = null;
            JwtClaimsResponse.Object.ContentHash = "ff";
            JwtClaimsResponse.Object.ContentHashAlgorithm = "S256";

            JwtClaims.Object.Response = JwtClaimsResponse.Object;

            JwtService.Setup(s => s.Decode(It.IsAny<RSA>(), It.IsAny<string>(), It.IsAny<String>(), It.IsAny<DateTime>(), It.IsAny<String>()))
                .Returns(JwtClaims.Object);
            JwtService.Setup(s => s.GetJWTData(It.IsAny<String>()))
                .Returns(new JwtData("lka", "svc:8c3c0268-f692-11e7-bd2e-7692096aba47", "svc:8c3c0268-f692-11e7-bd2e-7692096aba47", "Key ID"));
            JweService = new Mock<IJweService>();
            JweService.Setup(s => s.Decrypt(It.IsAny<String>())).Returns("Decrypted JWE");
            JweService.Setup(s => s.Encrypt(It.IsAny<String>(), It.IsAny<RSA>(), It.IsAny<String>(), It.IsAny<String>()))
                .Returns("Encrypted JWE");
            JsonEncoder = new Mock<IJsonEncoder>();
            JsonEncoder.Setup(e => e.EncodeObject(It.IsAny<Object>())).Returns("JSON Encoded");
            JsonEncoder.Setup(e => e.DecodeObject<PublicV3PingGetResponse>(It.IsAny<String>())).Returns(new Mock<PublicV3PingGetResponse>().Object);
            AuthsGetResponseCore = new Mock<ServiceV3AuthsGetResponseCore>();
            AuthsGetResponseCore.Object.EncryptedDeviceResponse = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("Encrypted Device Response"));
            AuthsGetResponseCore.Object.JweEncryptedDeviceResponse = null;
            AuthsGetResponseCore.Object.ServiceUserHash = "Service User Hash";
            AuthsGetResponseCore.Object.OrgUserHash = "Org User Hash";
            AuthsGetResponseCore.Object.UserPushId = "User Push ID";
            AuthsGetResponseCore.Object.PublicKeyId = "Response Public Key";
            JsonEncoder.Setup(e => e.DecodeObject<ServiceV3AuthsGetResponseCore>(It.IsAny<String>())).Returns(AuthsGetResponseCore.Object);
            DeviceResponse = new Mock<ServiceV3AuthsGetResponseDevice>();
            JsonEncoder.Setup(e => e.DecodeObject<ServiceV3AuthsGetResponseDevice>(It.IsAny<String>())).Returns(DeviceResponse.Object);
            KeyMap = new Mock<EntityKeyMap>();
            KeyMap.Object.AddKey(EntityIdentifier.FromString("svc:8c3c0268-f692-11e7-bd2e-7692096aba47"), "Response Public Key", new Mock<RSA>().Object);
            Transport = new WebClientTransport(
                HttpClient.Object,
                Crypto.Object,
                PublicKeyCache.Object,
                BaseUrl,
                Issuer,
                JwtService.Object,
                JweService.Object,
                0,
                0,
                KeyMap.Object,
                JsonEncoder.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            BaseUrl = null;
            Transport = null;
            PublicKeyCache = null;
            HttpClient = null;
            JweService = null;
            Crypto = null;
            JsonEncoder = null;
            Issuer = null;
            JwtService = null;
            KeyMap = null;
            HttpResponse = null;
        }

        [TestMethod]
        public void ServiceV3AuthsGet_ShouldReturnNullIfPending()
        {
            HttpResponse.Object.StatusCode = HttpStatusCode.NoContent;
            var response = Transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
            Assert.IsNull(response);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationRequestTimedOutError))]
        public void ServiceV3AuthsGet_ShouldThrowIfTimedOut()
        {
            HttpResponse.Object.StatusCode = HttpStatusCode.RequestTimeout;
            JwtClaimsResponse.Object.StatusCode = 408;
            Transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationRequestCanceled))]
        public void ServiceV3AuthsGet_ShouldThrowAuthorizationRequestCanceledFor400AndSvc007()
        {
            HttpResponse.Object.StatusCode = HttpStatusCode.BadRequest;
            HttpResponse.Object.ResponseBody = "Encrypted JWE";
            JwtClaimsResponse.Object.StatusCode = 400;
            JsonEncoder
                .Setup(p => p.DecodeObject<Sdk.Domain.Error>(It.IsAny<string>()))
                .Returns(new Sdk.Domain.Error
                {
                    ErrorCode = "SVC-007"
                });
            Transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
        }

        [TestMethod]
        public void ServiceV3AuthsGet_ShouldCallApi()
        {
            Transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
            HttpClient.Verify(client => client.ExecuteRequest(
                HttpMethod.GET,
                BaseUrl + "/service/v3/auths/" + TestConsts.DefaultAuthenticationId,
                It.IsAny<String>(),
                It.IsAny<Dictionary<string, string>>()));
        }

        [TestMethod]
        public void ServiceV3AuthsGet_ShouldReturnJweResponseDataIfPresent()
        {
            HttpResponse.Object.StatusCode = HttpStatusCode.OK;
            JwtClaimsResponse.Object.StatusCode = 200;
            Mock<ServiceV3AuthsGetResponseDeviceJWE> deviceResponse = new Mock<ServiceV3AuthsGetResponseDeviceJWE>();
            deviceResponse.Object.DeviceId = "Device ID";
            deviceResponse.Object.ServicePins = new string[] { "PIN1", "PIN2" };
            deviceResponse.Object.Type = "AUTHORIZED";
            deviceResponse.Object.Reason = "APPROVED";
            deviceResponse.Object.DenialReason = "DEN1";
            AuthsGetResponseCore.Object.JweEncryptedDeviceResponse = "Encrypted Device JWE Response";
            JsonEncoder.Setup(d => d.DecodeObject<ServiceV3AuthsGetResponseDeviceJWE>(It.IsAny<String>())).Returns(deviceResponse.Object);
            var actual = Transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
            Assert.AreEqual("Service User Hash", actual.ServiceUserHash);
            Assert.AreEqual("Org User Hash", actual.OrganizationUserHash);
            Assert.AreEqual("User Push ID", actual.UserPushId);
            Assert.AreEqual("Device ID", actual.DeviceId);
            Assert.AreEqual("PIN1", actual.DevicePins[0]);
            Assert.AreEqual("PIN2", actual.DevicePins[1]);
            Assert.IsTrue(actual.Response);
            Assert.AreEqual("AUTHORIZED", actual.Type);
            Assert.AreEqual("APPROVED", actual.Reason);
            Assert.AreEqual("DEN1", actual.DenialReason);
        }


        [TestMethod]
        public void ServiceV3AuthsGet_ShouldReturnFalseForResponseIfJweResponseReDataIfPresentAndTypeIsNotAuthorized()
        {
            HttpResponse.Object.StatusCode = HttpStatusCode.OK;
            JwtClaimsResponse.Object.StatusCode = 200;
            Mock<ServiceV3AuthsGetResponseDeviceJWE> deviceResponse = new Mock<ServiceV3AuthsGetResponseDeviceJWE>();
            deviceResponse.Object.DeviceId = "Device ID";
            deviceResponse.Object.ServicePins = new string[] { "PIN1", "PIN2" };
            deviceResponse.Object.Type = "NOT AUTHORIZED";
            deviceResponse.Object.Reason = "APPROVED";
            deviceResponse.Object.DenialReason = "DEN1";
            AuthsGetResponseCore.Object.JweEncryptedDeviceResponse = "Encrypted Device JWE Response";
            JsonEncoder.Setup(d => d.DecodeObject<ServiceV3AuthsGetResponseDeviceJWE>(It.IsAny<String>())).Returns(deviceResponse.Object);
            var actual = Transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
            Assert.IsFalse(actual.Response);
        }

        [TestMethod]
        public void ServiceV3AuthsGet_ShouldReturnNonJweResponseIfJweResponseNotPresent()
        {
            HttpResponse.Object.StatusCode = HttpStatusCode.OK;
            JwtClaimsResponse.Object.StatusCode = 200;
            DeviceResponse.Object.DeviceId = "Device ID";
            DeviceResponse.Object.ServicePins = new string[] { "PIN1", "PIN2" };
            DeviceResponse.Object.Response = true;
            var actual = Transport.ServiceV3AuthsGet(TestConsts.DefaultAuthenticationId, TestConsts.DefaultServiceEntity);
            Assert.AreEqual("Service User Hash", actual.ServiceUserHash);
            Assert.AreEqual("Org User Hash", actual.OrganizationUserHash);
            Assert.AreEqual("User Push ID", actual.UserPushId);
            Assert.AreEqual("Device ID", actual.DeviceId);
            Assert.AreEqual("PIN1", actual.DevicePins[0]);
            Assert.AreEqual("PIN2", actual.DevicePins[1]);
            Assert.IsTrue(actual.Response);
            Assert.IsNull(actual.Type);
            Assert.IsNull(actual.Reason);
            Assert.IsNull(actual.DenialReason);
        }
    }
}
