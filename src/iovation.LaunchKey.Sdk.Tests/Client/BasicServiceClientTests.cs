using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Webhook;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using serviceDomain = iovation.LaunchKey.Sdk.Domain.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace iovation.LaunchKey.Sdk.Tests.Client
{
    [TestClass]
    public class BasicServiceClientTests
    {
        [TestMethod]
        public void Authorize_ShouldCallTransport()
        {
            var mockTransport = new Mock<ITransport>();
            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            mockTransport.Setup(p =>
                p.ServiceV3AuthsPost(It.Is<ServiceV3AuthsPostRequest>(x => x.Username == "user"),
                It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsPostResponse { AuthRequest = TestConsts.DefaultAuthenticationId })
                .Verifiable();

            var response = client.Authorize("user");

            mockTransport.Verify();
            Assert.AreEqual(TestConsts.DefaultAuthenticationId.ToString("D"), response);
        }

        [TestMethod]
        public void Authorize_ShouldCallTransportWithContext()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsPost(
                It.Is<ServiceV3AuthsPostRequest>(req => req.Username == "name" && req.Context == "buy"),
                It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsPostResponse
                {
                    AuthRequest = TestConsts.DefaultAuthenticationId
                })
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            client.Authorize("name", "buy");

            mockTransport.Verify();
        }

        [TestMethod]
        public void Authorize_ShouldCallTransportWithPolicy()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsPost(
                    It.Is<ServiceV3AuthsPostRequest>(req => req.Username == "name" && req.Context == "buy" && req.AuthPolicy.MinimumRequirements[0].Any == 2),
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsPostResponse
                {
                    AuthRequest = TestConsts.DefaultAuthenticationId
                })
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            client.Authorize("name", "buy", new Sdk.Domain.Service.AuthPolicy(requiredFactors: 2));

            mockTransport.Verify();
        }

        [TestMethod]
        public void CreateAuthorizationRequest_ShouldCallTransport()
        {
            var mockTransport = new Mock<ITransport>();
            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            mockTransport.Setup(p =>
                p.ServiceV3AuthsPost(It.Is<ServiceV3AuthsPostRequest>(x => x.Username == "user"),
                It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsPostResponse { AuthRequest = TestConsts.DefaultAuthenticationId, PushPackage = "Push Package" })
                .Verifiable();

            var response = client.CreateAuthorizationRequest("user");

            mockTransport.Verify();
            Assert.AreEqual(TestConsts.DefaultAuthenticationId.ToString("D"), response.Id);
            Assert.AreEqual("Push Package", response.PushPackage);
        }

        [TestMethod]
        public void CreateAuthorizationRequest_ShouldCallTransportWithContext()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsPost(
                It.Is<ServiceV3AuthsPostRequest>(req => req.Username == "name" && req.Context == "buy"),
                It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsPostResponse
                {
                    AuthRequest = TestConsts.DefaultAuthenticationId
                })
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            client.CreateAuthorizationRequest("name", "buy");

            mockTransport.Verify();
        }

        [TestMethod]
        public void CreateAuthorizationRequest_ShouldCallTransportWithTitle()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsPost(
                It.Is<ServiceV3AuthsPostRequest>(req => req.Username == "name" && req.Title == "title"),
                It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsPostResponse
                {
                    AuthRequest = TestConsts.DefaultAuthenticationId
                })
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            client.CreateAuthorizationRequest("name", title: "title");

            mockTransport.Verify();
        }

        [TestMethod]
        public void CreateAuthorizationRequest_ShouldCallTransportWithTtl()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsPost(
                It.Is<ServiceV3AuthsPostRequest>(req => req.Username == "name" && req.TTL == 999),
                It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsPostResponse
                {
                    AuthRequest = TestConsts.DefaultAuthenticationId
                })
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            client.CreateAuthorizationRequest("name", ttl: 999);

            mockTransport.Verify();
        }

        [TestMethod]
        public void CreateAuthorizationRequest_ShouldCallTransportWithPolicy()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsPost(
                    It.Is<ServiceV3AuthsPostRequest>(req => req.Username == "name" && req.Context == "buy" && req.AuthPolicy.MinimumRequirements[0].Any == 2),
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsPostResponse
                {
                    AuthRequest = TestConsts.DefaultAuthenticationId
                })
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            client.CreateAuthorizationRequest("name", "buy", new Sdk.Domain.Service.AuthPolicy(requiredFactors: 2));

            mockTransport.Verify();
        }

        [TestMethod]
        public void CreateAuthorizationRequest_ShouldCallTransportWithPushTitleAndBody()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsPost(
                    It.Is<ServiceV3AuthsPostRequest>(req => req.Username == "name" && req.PushTitle == "Push Title" && req.PushBody == "Push Body"),
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsPostResponse
                {
                    AuthRequest = TestConsts.DefaultAuthenticationId
                })
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            client.CreateAuthorizationRequest("name", pushTitle: "Push Title", pushBody: "Push Body");

            mockTransport.Verify();
        }

        [TestMethod]
        public void CancelAuthorizationRequest_ShouldCallTransportWithProperEntityIdentifierAndAuthRequestId()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsDelete(
                    It.IsAny<Guid>(),
                    It.IsAny<EntityIdentifier>()))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            client.CancelAuthorizationRequest("aba6609a-a7e9-428d-b766-b69ecbcb65f6");

            mockTransport.Verify(p => p.ServiceV3AuthsDelete(
                    Guid.Parse("aba6609a-a7e9-428d-b766-b69ecbcb65f6"), 
                    new EntityIdentifier(EntityType.Service, TestConsts.DefaultServiceId)
                ));
        }

        [TestMethod]
        public void GetAuthorizationResponse_ShouldReturnNullIfTransportDoes()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();
            Assert.IsTrue(response == null);
        }

        [TestMethod]
        public void GetAuthorizationResponse_ShouldReturnTransportData()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "APPROVED", "Denial Reason", null, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(response.AuthorizationRequestId, TestConsts.DefaultAuthenticationId.ToString("D"));
            Assert.AreEqual(response.Authorized, true);
            Assert.AreEqual(response.DeviceId, "deviceid");
            Assert.AreEqual(response.DevicePins[0], "123");
            Assert.AreEqual(response.DevicePins[1], "abc");
            Assert.AreEqual(response.OrganizationUserHash, "ohash");
            Assert.AreEqual(response.ServiceUserHash, "shash");
            Assert.AreEqual(response.UserPushId, "push");
            Assert.AreEqual(Sdk.Domain.Service.AuthorizationResponseType.AUTHORIZED, response.Type);
            Assert.AreEqual(Sdk.Domain.Service.AuthorizationResponseReason.APPROVED, response.Reason);
            Assert.AreEqual("Denial Reason", response.DenialReason);
            Assert.AreEqual(false, response.Fraud);
        }

        [TestMethod]
        public void GetAuthorizationResponse_ShouldReturnFraudTrueForFraudulentReason()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", null, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(true, response.Fraud);
        }

        [TestMethod]
        public void SessionStart_ShouldCallTransport()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3SessionsPost(
                    It.Is<ServiceV3SessionsPostRequest>(
                        r => r.AuthRequest == TestConsts.DefaultAuthenticationId
                        && r.Username == "user"
                    ),
                    It.IsAny<EntityIdentifier>()))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            client.SessionStart("user", TestConsts.DefaultAuthenticationId.ToString("D"));
            mockTransport.Verify();
        }

        [TestMethod]
        public void SessionEnd_ShouldCallTransport()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3SessionsDelete(
                    It.Is<ServiceV3SessionsDeleteRequest>(
                        r => r.Username == "user"
                    ),
                    It.IsAny<EntityIdentifier>()))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            client.SessionEnd("user");
            mockTransport.Verify();
        }

        [TestMethod]
        public void HandleWebhook_AuthPackage_ShouldReturnProperObject()
        {
            var testHeaders = new Dictionary<string, List<string>>();
            var testBody = "body";

            var geofences = new AuthPolicy.Location[1];
            geofences[0] = new AuthPolicy.Location("Test Geo", 200, 36.120825, -115.157216);

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 2,
                Geofences = geofences
            };

            var testResponse = new ServerSentEventAuthorizationResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "devid", new[] { "1234" }, "AUTHORIZED", "APPROVED", "Denial Reason", authPolicy, null);
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.HandleServerSentEvent(testHeaders, testBody, null, null)).Returns(testResponse).Verifiable();
            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.HandleWebhook(testHeaders, testBody);

            // verify the call worked
            mockTransport.Verify();

            // verify we got the right response
            Assert.IsTrue(response is AuthorizationResponseWebhookPackage);

            var authResponse = ((AuthorizationResponseWebhookPackage)response).AuthorizationResponse;
            // verify response contents
            Assert.AreEqual(authResponse.AuthorizationRequestId, testResponse.AuthorizationRequestId.ToString("D"));
            Assert.AreEqual(authResponse.Authorized, testResponse.Response);
            Assert.AreEqual(authResponse.DeviceId, testResponse.DeviceId);
            Assert.AreEqual(authResponse.DevicePins.Count, testResponse.DevicePins.Length);
            Assert.AreEqual(authResponse.OrganizationUserHash, "ohash");
            Assert.AreEqual(authResponse.ServiceUserHash, "shash");
            Assert.AreEqual(authResponse.UserPushId, "push");
            Assert.AreEqual(Sdk.Domain.Service.AuthorizationResponseType.AUTHORIZED, authResponse.Type);
            Assert.AreEqual(Sdk.Domain.Service.AuthorizationResponseReason.APPROVED, authResponse.Reason);
            Assert.AreEqual("Denial Reason", authResponse.DenialReason);
            Assert.AreEqual(false, authResponse.Fraud);
            Assert.AreEqual(2, authResponse.AuthPolicy.RequiredFactors);
            Assert.AreEqual(1, authResponse.AuthPolicy.Locations.Count);
            Assert.AreEqual(200, authResponse.AuthPolicy.Locations[0].Radius);
            Assert.AreEqual(36.120825, authResponse.AuthPolicy.Locations[0].Latitude);
            Assert.AreEqual(-115.157216, authResponse.AuthPolicy.Locations[0].Longitude);
            Assert.AreEqual(null, authResponse.AuthPolicy.RequireInherenceFactor);
            Assert.AreEqual(null, authResponse.AuthPolicy.RequireKnowledgeFactor);
            Assert.AreEqual(null, authResponse.AuthPolicy.RequirePosessionFactor);
            Assert.AreEqual(null, authResponse.AuthMethods);
        }

        [TestMethod]
        public void HandleWebhook_AuthPackage_NoAMIPresentShouldReturnNullForAuthMethodAndAuthPolicy()
        {
            var testHeaders = new Dictionary<string, List<string>>();
            var testBody = "body";
            var testResponse = new ServerSentEventAuthorizationResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "devid", new[] { "1234" }, "AUTHORIZED", "APPROVED", "Denial Reason", null, null);
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.HandleServerSentEvent(testHeaders, testBody, null, null)).Returns(testResponse).Verifiable();
            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.HandleWebhook(testHeaders, testBody);

            // verify the call worked
            mockTransport.Verify();

            // verify we got the right response
            Assert.IsTrue(response is AuthorizationResponseWebhookPackage);

            var authResponse = ((AuthorizationResponseWebhookPackage)response).AuthorizationResponse;
            // verify response contents
            Assert.AreEqual(null, authResponse.AuthPolicy);
            Assert.AreEqual(null, authResponse.AuthMethods);
        }

        [TestMethod]
        public void HandleWebhook_AuthPackage_ShouldSetFraudTrueForFraudulentReason()
        {
            var testHeaders = new Dictionary<string, List<string>>();
            var testBody = "body";
            var testResponse = new ServerSentEventAuthorizationResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "devid", new[] { "1234" }, "DENIED", "FRAUDULENT", "Denial Reason", null, null);
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.HandleServerSentEvent(testHeaders, testBody, null, null)).Returns(testResponse).Verifiable();
            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.HandleWebhook(testHeaders, testBody);

            // verify the call worked
            mockTransport.Verify();

            // verify we got the right response
            Assert.IsTrue(response is AuthorizationResponseWebhookPackage);

            var authResponse = ((AuthorizationResponseWebhookPackage)response).AuthorizationResponse;
            Assert.AreEqual(true, authResponse.Fraud);
        }

        [TestMethod]
        public void HandleWebhook_SessionEnd_ShouldReturnProperObject()
        {
            var testHeaders = new Dictionary<string, List<string>>();
            var testBody = "body";
            var testResponse = new ServerSentEventUserServiceSessionEnd { ApiTime = TestConsts.DefaultTime, UserHash = "uhash" };
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.HandleServerSentEvent(testHeaders, testBody, null, null)).Returns(testResponse).Verifiable();
            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.HandleWebhook(testHeaders, testBody);

            // verify the call worked
            mockTransport.Verify();

            // verify we got the right response
            Assert.IsTrue(response is ServiceUserSessionEndWebhookPackage);

            var package = ((ServiceUserSessionEndWebhookPackage)response);
            // verify response contents
            Assert.AreEqual(package.LogoutRequested, TestConsts.DefaultTime);
            Assert.AreEqual(package.ServiceUserHash, "uhash");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestException))]
        public void HandleWebhook_ShouldThrowOnUnexpectedResult()
        {
            var testHeaders = new Dictionary<string, List<string>>();
            var testBody = "body";
            var testResponse = new Mock<IServerSentEvent>().Object;
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.HandleServerSentEvent(testHeaders, testBody, null, null)).Returns(testResponse).Verifiable();
            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);

            client.HandleWebhook(testHeaders, testBody);
        }

        [TestMethod]
        public void AuthMethodInsight_MissingAuthPolicyShouldBeNull()
        {
            var mockTransport = new Mock<ITransport>();
            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", null, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(null, response.AuthPolicy);
            Assert.AreEqual(null, response.AuthMethods);
        }

        [TestMethod]
        public void AuthMethodInsight_GeofencesShouldMatchTransport()
        {
            var mockTransport = new Mock<ITransport>();

            var geofences = new AuthPolicy.Location[2];
            geofences[0] = new AuthPolicy.Location("Work", 150, 36.083548, -115.157517);
            geofences[1] = new AuthPolicy.Location("Home", 100, 40.55, -90.12);

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 2,
                Geofences = geofences
            };

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", authPolicy, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(150, response.AuthPolicy.Locations[0].Radius);
            Assert.AreEqual(36.083548, response.AuthPolicy.Locations[0].Latitude);
            Assert.AreEqual(-115.157517, response.AuthPolicy.Locations[0].Longitude);
            Assert.AreEqual(100, response.AuthPolicy.Locations[1].Radius);
            Assert.AreEqual(40.55, response.AuthPolicy.Locations[1].Latitude);
            Assert.AreEqual(-90.12, response.AuthPolicy.Locations[1].Longitude);
        }

        [TestMethod]
        public void AuthMethodInsight_EmptyGeofencesShouldMatchTransport()
        {
            var mockTransport = new Mock<ITransport>();

            var geofences = new AuthPolicy.Location[0];

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 2,
                Geofences = geofences
            };

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", authPolicy, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            List<Sdk.Domain.Service.Location> emptyLocation = new List<Sdk.Domain.Service.Location>();


            CollectionAssert.AreEqual(emptyLocation, response.AuthPolicy.Locations);
            Assert.AreEqual(0, response.AuthPolicy.Locations.Count);
        }

        [TestMethod]
        public void AuthMethodInsight_TypesRequirementShouldMatchTransport()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "types",
                Types = new List<string> { "knowledge", "inherence" }
            };

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", authPolicy, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(true, response.AuthPolicy.RequireInherenceFactor);
            Assert.AreEqual(true, response.AuthPolicy.RequireKnowledgeFactor);
            Assert.AreEqual(false, response.AuthPolicy.RequirePosessionFactor);
            Assert.AreEqual(null, response.AuthPolicy.RequiredFactors);
        }

        [TestMethod]
        public void AuthMethodInsight_TypesRequirementWithAmountShouldNotSetAmount()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "types",
                Amount = 3,
                Types = new List<string> { "inherence" }
            };

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", authPolicy, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(true, response.AuthPolicy.RequireInherenceFactor);
            Assert.AreEqual(false, response.AuthPolicy.RequireKnowledgeFactor);
            Assert.AreEqual(false, response.AuthPolicy.RequirePosessionFactor);
            Assert.AreEqual(null, response.AuthPolicy.RequiredFactors);
        }

        [TestMethod]
        public void AuthMethodInsight_InvalidTypeShouldNotFail()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "types",
                Types = new List<string> { "knowledge", "invalid" }
            };

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", authPolicy, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(false, response.AuthPolicy.RequireInherenceFactor);
            Assert.AreEqual(true, response.AuthPolicy.RequireKnowledgeFactor);
            Assert.AreEqual(false, response.AuthPolicy.RequirePosessionFactor);
        }

        [TestMethod]
        public void AuthMethodInsight_InvalidRequirementShouldNotFail()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "invalid",
                Types = new List<string> { "knowledge", "inherence" },
                Amount = 2
            };

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", authPolicy, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(null, response.AuthPolicy.RequiredFactors);
            Assert.AreEqual(null, response.AuthPolicy.RequireInherenceFactor);
            Assert.AreEqual(null, response.AuthPolicy.RequireKnowledgeFactor);
            Assert.AreEqual(null, response.AuthPolicy.RequirePosessionFactor);
        }

        [TestMethod]
        public void AuthMethodInsight_AmountShouldMatchTransport()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 3
            };

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", authPolicy, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(3, response.AuthPolicy.RequiredFactors);
            Assert.AreEqual(null, response.AuthPolicy.RequireInherenceFactor);
            Assert.AreEqual(null, response.AuthPolicy.RequireKnowledgeFactor);
            Assert.AreEqual(null, response.AuthPolicy.RequirePosessionFactor);
        }

        [TestMethod]
        public void AuthMethodInsight_TypeShouldNotFail()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "types",
                Types = new List<string> { "knowledge", "invalid" }
            };

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", authPolicy, null))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(null, response.AuthPolicy.RequiredFactors);
            Assert.AreEqual(false, response.AuthPolicy.RequireInherenceFactor);
            Assert.AreEqual(true, response.AuthPolicy.RequireKnowledgeFactor);
            Assert.AreEqual(false, response.AuthPolicy.RequirePosessionFactor);
        }

        public AuthPolicy.AuthMethod CreateAuthTransportMethod( string method, bool? set, bool active, bool allowed, bool supported, bool? userRequired, bool? passed, bool? error)
        {
            var authMethod = new AuthPolicy.AuthMethod();
            authMethod.Method = method;
            authMethod.Active = active;
            authMethod.Set = set;
            authMethod.Allowed = allowed;
            authMethod.Supported = supported;
            authMethod.UserRequired = userRequired;
            authMethod.Passed = passed;
            authMethod.Error = error;
            return authMethod;

        }

        [TestMethod]
        public void AuthMethodInsight_1_UnknownAuthMethodShouldNotThrowError()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = null
            };

            var authMethods = new AuthPolicy.AuthMethod[8];
            authMethods[0] = CreateAuthTransportMethod("wearables", false, false, true, true, null, null, null);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", true, true, true, true, true, true, false);
            authMethods[3] = CreateAuthTransportMethod("pin_code", true, true, true, true, true, true, false);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, null, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", false, false, true, true, null, null, null);
            authMethods[7] = CreateAuthTransportMethod("something_new", false, false, true, true, null, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "AUTHORIZED", "FRAUDULENT", "Denial Reason", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            var authMethod = response.AuthMethods[7];
            Assert.AreEqual(serviceDomain.AuthMethodType.OTHER, authMethod.Method);
            Assert.AreEqual(false, authMethod.Set);
            Assert.AreEqual(false, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(null, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);
        }

        [TestMethod]
        public void AuthMethodInsight_2_LocationFailedUncheckedPincode()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = null
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", false, false, true, true, null, null, null);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", true, true, true, true, true, false, false);
            authMethods[3] = CreateAuthTransportMethod("pin_code", true, true, true, true, true, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, null, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", false, false, true, true, null, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "AUTHENTICATION", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.AUTHENTICATION, response.Reason);

            var authMethod = response.AuthMethods[2];
            Assert.AreEqual(serviceDomain.AuthMethodType.LOCATIONS, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(false, authMethod.Passed);
            Assert.AreEqual(false, authMethod.Error);

            authMethod = response.AuthMethods[3];
            Assert.AreEqual(serviceDomain.AuthMethodType.PIN_CODE, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            List<serviceDomain.Location> emptyLocation = new List<serviceDomain.Location>();
            CollectionAssert.AreEqual(emptyLocation, response.AuthPolicy.Locations);
        }

        [TestMethod]
        public void AuthMethodInsight_3_PossessionFailureUncheckedCircleCode()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "types",
                Types = new List<string> { "possession" }
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", false, false, true, true, null, null, null);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", false, false, true, true, null, null, null);
            authMethods[3] = CreateAuthTransportMethod("pin_code", false, false, true, true, null, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", true, true, true, true, false, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", false, false, true, true, null, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "POLICY", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.POLICY, response.Reason);

            var authMethod = response.AuthMethods[4];
            Assert.AreEqual(serviceDomain.AuthMethodType.CIRCLE_CODE, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(false, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            List<Sdk.Domain.Service.Location> emptyLocation = new List<Sdk.Domain.Service.Location>();
            CollectionAssert.AreEqual(emptyLocation, response.AuthPolicy.Locations);
        }


        [TestMethod]
        public void AuthMethodInsight_4_AmountFailureUncheckedFingerprint()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 2
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", false, false, true, true, null, null, null);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", false, false, true, true, null, null, null);
            authMethods[3] = CreateAuthTransportMethod("pin_code", false, false, true, true, null, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, false, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", true, true, true, true, true, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "POLICY", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.POLICY, response.Reason);

            var authMethod = response.AuthMethods[6];
            Assert.AreEqual(serviceDomain.AuthMethodType.FINGERPRINT, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            List<Sdk.Domain.Service.Location> emptyLocation = new List<Sdk.Domain.Service.Location>();
            CollectionAssert.AreEqual(emptyLocation, response.AuthPolicy.Locations);
        }

        [TestMethod]
        public void AuthMethodInsight_5_AmountSuccessFailedWearableSensorUncheckedFingerprint()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 2
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", true, true, true, true, true, null, true);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", false, false, true, true, null, null, null);
            authMethods[3] = CreateAuthTransportMethod("pin_code", false, false, true, true, null, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, false, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", true, true, true, true, true, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "SENSOR", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.SENSOR, response.Reason);

            var authMethod = response.AuthMethods[0];
            Assert.AreEqual(serviceDomain.AuthMethodType.WEARABLES, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(true, authMethod.Error);

            authMethod = response.AuthMethods[6];
            Assert.AreEqual(serviceDomain.AuthMethodType.FINGERPRINT, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            List<Sdk.Domain.Service.Location> emptyLocation = new List<Sdk.Domain.Service.Location>();
            CollectionAssert.AreEqual(emptyLocation, response.AuthPolicy.Locations);
        }

        [TestMethod]
        public void AuthMethodInsight_6_RequiredAmountFailedWearableUncheckedLocationUncheckedFingerprint()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 2
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", true, true, true, true, false, false, false);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", true, true, true, true, false, null, null);
            authMethods[3] = CreateAuthTransportMethod("pin_code", false, false, true, true, null, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, false, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", true, true, true, true, true, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "AUTHENTICATION", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.AUTHENTICATION, response.Reason);

            var authMethod = response.AuthMethods[0];
            Assert.AreEqual(serviceDomain.AuthMethodType.WEARABLES, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(false, authMethod.UserRequired);
            Assert.AreEqual(false, authMethod.Passed);
            Assert.AreEqual(false, authMethod.Error);


            authMethod = response.AuthMethods[2];
            Assert.AreEqual(serviceDomain.AuthMethodType.LOCATIONS, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(false, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            authMethod = response.AuthMethods[6];
            Assert.AreEqual(serviceDomain.AuthMethodType.FINGERPRINT, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            List<Sdk.Domain.Service.Location> emptyLocation = new List<Sdk.Domain.Service.Location>();
            CollectionAssert.AreEqual(emptyLocation, response.AuthPolicy.Locations);
        }

        [TestMethod]
        public void AuthMethodInsight_7_SuccessfulFingerprintSuccessfulLocationUncheckedWearable()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 2
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", true, true, true, true, false, null, null);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", true, true, true, true, true, true, false);
            authMethods[3] = CreateAuthTransportMethod("pin_code", false, false, true, true, null, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, false, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", true, true, true, true, true, true, false);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "AUTHENTICATION", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.AUTHENTICATION, response.Reason);

            var authMethod = response.AuthMethods[0];
            Assert.AreEqual(serviceDomain.AuthMethodType.WEARABLES, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(false, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);


            authMethod = response.AuthMethods[2];
            Assert.AreEqual(serviceDomain.AuthMethodType.LOCATIONS, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(true, authMethod.Passed);
            Assert.AreEqual(false, authMethod.Error);

            authMethod = response.AuthMethods[6];
            Assert.AreEqual(serviceDomain.AuthMethodType.FINGERPRINT, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(true, authMethod.Passed);
            Assert.AreEqual(false, authMethod.Error);

            List<Sdk.Domain.Service.Location> emptyLocation = new List<Sdk.Domain.Service.Location>();
            CollectionAssert.AreEqual(emptyLocation, response.AuthPolicy.Locations);
        }

        [TestMethod]
        public void AuthMethodInsight_8_RequiredAmountPassedGeofenceFailedAmountSkippedFaceSkippedPin()
        {
            var mockTransport = new Mock<ITransport>();

            var geofences = new AuthPolicy.Location[1];
            geofences[0] = new AuthPolicy.Location("Work", 150, 36.083548, -115.157517);

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 3,
                Geofences = geofences
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", false, false, true, true, null, null, null);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", false, false, true, true, null, null, null);
            authMethods[3] = CreateAuthTransportMethod("pin_code", true, true, true, true, true, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, null, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", true, true, true, true, true, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", false, false, true, true, null, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "POLICY", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.POLICY, response.Reason);

            var authMethod = response.AuthMethods[1];
            Assert.AreEqual(serviceDomain.AuthMethodType.GEOFENCING, authMethod.Method);
            Assert.AreEqual(null, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(null, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);


            authMethod = response.AuthMethods[3];
            Assert.AreEqual(serviceDomain.AuthMethodType.PIN_CODE, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            authMethod = response.AuthMethods[5];
            Assert.AreEqual(serviceDomain.AuthMethodType.FACE, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            serviceDomain.Location location = new serviceDomain.Location(150,36.083548,-115.157517, "");

            Assert.AreEqual(location, response.AuthPolicy.Locations[0]);
        }

        [TestMethod]
        public void AuthMethodInsight_9_RequiredAmountFailedGeofenceUncheckedFaceUncheckedPin()
        {
            var mockTransport = new Mock<ITransport>();

            var geofences = new AuthPolicy.Location[1];
            geofences[0] = new AuthPolicy.Location("Work", 150, 36.083548, -115.157517);

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 2,
                Geofences = geofences
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", false, false, true, true, null, null, null);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", false, false, true, true, null, null, null);
            authMethods[3] = CreateAuthTransportMethod("pin_code", true, true, true, true, true, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, null, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", true, true, true, true, true, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", false, false, true, true, null, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "AUTHENTICATION", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.AUTHENTICATION, response.Reason);

            var authMethod = response.AuthMethods[1];
            Assert.AreEqual(serviceDomain.AuthMethodType.GEOFENCING, authMethod.Method);
            Assert.AreEqual(null, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(null, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);


            authMethod = response.AuthMethods[3];
            Assert.AreEqual(serviceDomain.AuthMethodType.PIN_CODE, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            authMethod = response.AuthMethods[5];
            Assert.AreEqual(serviceDomain.AuthMethodType.FACE, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            serviceDomain.Location location = new serviceDomain.Location(150, 36.083548, -115.157517, "");

            Assert.AreEqual(location, response.AuthPolicy.Locations[0]);
        }

        [TestMethod]
        public void AuthMethodInsight_10_LocationFailureUncheckedFingerprintPassedGeofence()
        {
            var mockTransport = new Mock<ITransport>();

            var geofences = new AuthPolicy.Location[1];
            geofences[0] = new AuthPolicy.Location("Work", 150, 36.083548, -115.157517);

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = null,
                Geofences = geofences
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", false, false, true, true, null, null, null);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, true, false);
            authMethods[2] = CreateAuthTransportMethod("locations", true, true, true, true, true, false, false);
            authMethods[3] = CreateAuthTransportMethod("pin_code", false, false, true, true, null, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, null, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", true, true, true, true, true, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "AUTHENTICATION", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.AUTHENTICATION, response.Reason);

            var authMethod = response.AuthMethods[1];
            Assert.AreEqual(serviceDomain.AuthMethodType.GEOFENCING, authMethod.Method);
            Assert.AreEqual(null, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(null, authMethod.UserRequired);
            Assert.AreEqual(true, authMethod.Passed);
            Assert.AreEqual(false, authMethod.Error);


            authMethod = response.AuthMethods[2];
            Assert.AreEqual(serviceDomain.AuthMethodType.LOCATIONS, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(false, authMethod.Passed);
            Assert.AreEqual(false, authMethod.Error);

            authMethod = response.AuthMethods[6];
            Assert.AreEqual(serviceDomain.AuthMethodType.FINGERPRINT, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(true, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            serviceDomain.Location location = new serviceDomain.Location(150, 36.083548, -115.157517, "");

            Assert.AreEqual(location, response.AuthPolicy.Locations[0]);
        }

        [TestMethod]
        public void AuthMethodInsight_11_RequiredPossessionFailureUncheckedPinUncheckedCircleCode()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "types",
                Types = new List<string> { "possession" }
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", false, false, true, true, null, null, null);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", false, false, true, true, null, null, null);
            authMethods[3] = CreateAuthTransportMethod("pin_code", true, true, true, true, false, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", true, true, true, true, false, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", false, false, true, true, null, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "POLICY", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.POLICY, response.Reason);

            var authMethod = response.AuthMethods[3];
            Assert.AreEqual(serviceDomain.AuthMethodType.PIN_CODE, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(false, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);


            authMethod = response.AuthMethods[4];
            Assert.AreEqual(serviceDomain.AuthMethodType.CIRCLE_CODE, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(false, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            List<Sdk.Domain.Service.Location> emptyLocation = new List<Sdk.Domain.Service.Location>();
            CollectionAssert.AreEqual(emptyLocation, response.AuthPolicy.Locations);
        }

        [TestMethod]
        public void AuthMethodInsight_12_Required_Amount_1_Failed_Wearable_Sensor_Unchecked_Location()
        {
            var mockTransport = new Mock<ITransport>();

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "amount",
                Amount = 1
            };

            var authMethods = new AuthPolicy.AuthMethod[7];
            authMethods[0] = CreateAuthTransportMethod("wearables", true, true, true, true, false, null, true);
            authMethods[1] = CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null);
            authMethods[2] = CreateAuthTransportMethod("locations", true, true, true, true, false, null, null);
            authMethods[3] = CreateAuthTransportMethod("pin_code", false, false, true, true, null, null, null);
            authMethods[4] = CreateAuthTransportMethod("circle_code", false, false, true, true, null, null, null);
            authMethods[5] = CreateAuthTransportMethod("face", false, false, true, true, null, null, null);
            authMethods[6] = CreateAuthTransportMethod("fingerprint", false, false, true, true, null, null, null);

            mockTransport.Setup(p => p.ServiceV3AuthsGet(
                    TestConsts.DefaultAuthenticationId,
                    It.IsAny<EntityIdentifier>()))
                .Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { "123", "abc" }, "FAILED", "SENSOR", "32", authPolicy, authMethods))
                .Verifiable();

            var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
            var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

            mockTransport.Verify();

            Assert.AreEqual(serviceDomain.AuthorizationResponseType.FAILED, response.Type);
            Assert.AreEqual(serviceDomain.AuthorizationResponseReason.SENSOR, response.Reason);

            var authMethod = response.AuthMethods[0];
            Assert.AreEqual(serviceDomain.AuthMethodType.WEARABLES, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(false, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(true, authMethod.Error);


            authMethod = response.AuthMethods[2];
            Assert.AreEqual(serviceDomain.AuthMethodType.LOCATIONS, authMethod.Method);
            Assert.AreEqual(true, authMethod.Set);
            Assert.AreEqual(true, authMethod.Active);
            Assert.AreEqual(true, authMethod.Allowed);
            Assert.AreEqual(true, authMethod.Supported);
            Assert.AreEqual(false, authMethod.UserRequired);
            Assert.AreEqual(null, authMethod.Passed);
            Assert.AreEqual(null, authMethod.Error);

            List<Sdk.Domain.Service.Location> emptyLocation = new List<Sdk.Domain.Service.Location>();
            CollectionAssert.AreEqual(emptyLocation, response.AuthPolicy.Locations);
        }

    }

}
