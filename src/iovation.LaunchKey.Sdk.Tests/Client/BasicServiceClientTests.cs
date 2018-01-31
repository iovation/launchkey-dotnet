using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Webhook;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
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
				.Returns(new ServiceV3AuthsPostResponse {AuthRequest = TestConsts.DefaultAuthenticationId})
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
				.Returns(new ServiceV3AuthsGetResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "deviceid", new string[] { }))
				.Verifiable();

			var client = new BasicServiceClient(TestConsts.DefaultServiceId, mockTransport.Object);
			var response = client.GetAuthorizationResponse(TestConsts.DefaultAuthenticationId.ToString("D"));

			mockTransport.Verify();

			Assert.IsTrue(response != null);
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
			var testResponse = new ServerSentEventAuthorizationResponse(TestConsts.DefaultServiceEntity, TestConsts.DefaultServiceId, "shash", "ohash", "push", TestConsts.DefaultAuthenticationId, true, "devid", new[] {"1234"});
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
		}

		[TestMethod]
		public void HandleWebhook_SessionEnd_ShouldReturnProperObject()
		{
			var testHeaders = new Dictionary<string, List<string>>();
			var testBody = "body";
			var testResponse = new ServerSentEventUserServiceSessionEnd {ApiTime = TestConsts.DefaultTime, UserHash = "uhash"};
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
	}
}
