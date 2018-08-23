using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace iovation.LaunchKey.Sdk.Tests.Client
{
	[TestClass]
    public class BasicOrganizationClientTests
    {
		[TestMethod]
		public void CreateService_ShouldCallTransportWithCorrectParams()
		{
			var mockTransport = new Mock<ITransport>();
			var callbackUrl = new Uri("http://example.com");
			var iconUrl = new Uri("http://example.com/icon");
			var returnedGuid = Guid.NewGuid();

			mockTransport.Setup(p =>
					p.OrganizationV3ServicesPost(
						It.IsAny<ServicesPostRequest>(), 
						It.IsAny<EntityIdentifier>()
					)
				)
				.Returns(new ServicesPostResponse {Id = returnedGuid });

			var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);
			var response = client.CreateService("service name", "desc", iconUrl, callbackUrl, true);

			// verify the call made it to transport with right params
			mockTransport.Verify(
				x => x.OrganizationV3ServicesPost(It.Is<ServicesPostRequest>(
						p => p.Active == true
							&& p.Name == "service name"
							&& p.Description == "desc"
							&& p.CallbackUrl == callbackUrl
							&& p.Icon == iconUrl
					),
					It.IsAny<EntityIdentifier>()), Times.Once());

			// verify result from transport made its way back out
			Assert.AreEqual(returnedGuid, response);
		}

		[TestMethod]
		public void UpdateService_ShouldCallTransportWithCorrectParams()
		{
			var mockTransport = new Mock<ITransport>();
			var callbackUrl = new Uri("http://example.com");
			var iconUrl = new Uri("http://example.com/icon");
			var serviceId = Guid.NewGuid();

			mockTransport.Setup(p =>
				p.OrganizationV3ServicesPatch(
					It.IsAny<ServicesPatchRequest>(),
					It.IsAny<EntityIdentifier>()
				)
			);

			var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);
			client.UpdateService(serviceId, "service name", "desc", iconUrl, callbackUrl, true);

			// verify the call made it to transport with right params
			mockTransport.Verify(
				x => x.OrganizationV3ServicesPatch(It.Is<ServicesPatchRequest>(
						p => p.Active == true
							&& p.Name == "service name"
							&& p.Description == "desc"
							&& p.CallbackUrl == callbackUrl
							&& p.Icon == iconUrl
							&& p.ServiceId == serviceId
					),
					It.IsAny<EntityIdentifier>()), Times.Once());

		}

		[TestMethod]
		public void GetService_ShouldCallTransportWithCorrectParams()
		{
			var mockTransport = new Mock<ITransport>();
			var callbackUrl = new Uri("http://example.com");
			var iconUrl = new Uri("http://example.com/icon");
			var serviceId = Guid.NewGuid();
			var serviceObject = new ServicesListPostResponse.Service
			{
				Id = serviceId,
				Name = "my name",
				Description = "my description",
				Active = true,
				CallbackUrl = callbackUrl,
				Icon = iconUrl
			};

			mockTransport.Setup(p =>
				p.OrganizationV3ServicesListPost(
					It.IsAny<ServicesListPostRequest>(),
					It.IsAny<EntityIdentifier>()
				)
			)
			.Returns(new ServicesListPostResponse(new List<ServicesListPostResponse.Service> { serviceObject }));

			var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);
			var responseObject = client.GetService(serviceId);

			// verify the call made it to transport with right params
			mockTransport.Verify(
				x => x.OrganizationV3ServicesListPost(It.Is<ServicesListPostRequest>(
						p => p.ServiceIds != null
							&& p.ServiceIds.Count == 1
							&& p.ServiceIds[0] == serviceId
					),
					It.IsAny<EntityIdentifier>()), Times.Once());

			// verify our code translated the objects OK
			Assert.AreEqual(responseObject.Active, serviceObject.Active);
			Assert.AreEqual(responseObject.CallbackUrl, serviceObject.CallbackUrl);
			Assert.AreEqual(responseObject.Description, serviceObject.Description);
			Assert.AreEqual(responseObject.Icon, serviceObject.Icon);
			Assert.AreEqual(responseObject.Id, serviceObject.Id);
			Assert.AreEqual(responseObject.Name, serviceObject.Name);
		}

		[TestMethod]
		public void GetServices_ShouldCallTransportWithCorrectParams()
		{
			var mockTransport = new Mock<ITransport>();
			var callbackUrl = new Uri("http://example.com");
			var iconUrl = new Uri("http://example.com/icon");
			var serviceId = Guid.NewGuid();
			var serviceId2 = Guid.NewGuid();
			var serviceObject = new ServicesListPostResponse.Service
			{
				Id = serviceId,
				Name = "my name",
				Description = "my description",
				Active = true,
				CallbackUrl = callbackUrl,
				Icon = iconUrl
			};
			var serviceObject2 = new ServicesListPostResponse.Service
			{
				Id = serviceId,
				Name = "my name 2",
				Description = "my description 2",
				Active = true,
				CallbackUrl = callbackUrl,
				Icon = iconUrl
			};

			mockTransport.Setup(p =>
					p.OrganizationV3ServicesListPost(
						It.IsAny<ServicesListPostRequest>(),
						It.IsAny<EntityIdentifier>()
					)
				)
				.Returns(new ServicesListPostResponse(new List<ServicesListPostResponse.Service> { serviceObject, serviceObject2 }));

			var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);
			var responseObject = client.GetServices(new List<Guid>{serviceId, serviceId2});

			// verify the call made it to transport with right params
			mockTransport.Verify(
				x => x.OrganizationV3ServicesListPost(It.Is<ServicesListPostRequest>(
						p => p.ServiceIds != null
							&& p.ServiceIds.Count == 2
							&& p.ServiceIds[0] == serviceId
							&& p.ServiceIds[1] == serviceId2
					),
					It.IsAny<EntityIdentifier>()), Times.Once());

			// verify our code translated the objects OK
			Assert.AreEqual(responseObject[0].Active, serviceObject.Active);
			Assert.AreEqual(responseObject[0].CallbackUrl, serviceObject.CallbackUrl);
			Assert.AreEqual(responseObject[0].Description, serviceObject.Description);
			Assert.AreEqual(responseObject[0].Icon, serviceObject.Icon);
			Assert.AreEqual(responseObject[0].Id, serviceObject.Id);
			Assert.AreEqual(responseObject[0].Name, serviceObject.Name);

			Assert.AreEqual(responseObject[1].Active, serviceObject2.Active);
			Assert.AreEqual(responseObject[1].CallbackUrl, serviceObject2.CallbackUrl);
			Assert.AreEqual(responseObject[1].Description, serviceObject2.Description);
			Assert.AreEqual(responseObject[1].Icon, serviceObject2.Icon);
			Assert.AreEqual(responseObject[1].Id, serviceObject2.Id);
			Assert.AreEqual(responseObject[1].Name, serviceObject2.Name);
		}

		[TestMethod]
		public void GetAllServices_ShouldCallTransportWithCorrectParams()
		{
			var mockTransport = new Mock<ITransport>();
			var callbackUrl = new Uri("http://example.com");
			var iconUrl = new Uri("http://example.com/icon");
			var serviceObject = new ServicesGetResponse.Service {Active = true, CallbackUrl = callbackUrl, Description = "one service", Name = "my service", Icon = iconUrl, Id = Guid.NewGuid()};

			mockTransport.Setup(p => p.OrganizationV3ServicesGet(It.IsAny<EntityIdentifier>()))
				.Returns(new ServicesGetResponse(new List<ServicesGetResponse.Service> {serviceObject}))
				.Verifiable();

			var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);
			var responseObject = client.GetAllServices();

			// verify the call made it to transport with right params
			mockTransport.Verify();

			// verify our code translated the objects OK
			Assert.AreEqual(responseObject[0].Active, serviceObject.Active);
			Assert.AreEqual(responseObject[0].CallbackUrl, serviceObject.CallbackUrl);
			Assert.AreEqual(responseObject[0].Description, serviceObject.Description);
			Assert.AreEqual(responseObject[0].Icon, serviceObject.Icon);
			Assert.AreEqual(responseObject[0].Id, serviceObject.Id);
			Assert.AreEqual(responseObject[0].Name, serviceObject.Name);
		}
	}
}
