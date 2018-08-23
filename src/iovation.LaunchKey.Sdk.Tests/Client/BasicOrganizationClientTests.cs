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
			mockTransport.Verify(
				x => x.OrganizationV3ServicesPost(It.Is<ServicesPostRequest>(
						p => p.Active == true
							&& p.Name == "service name"
							&& p.Description == "desc"
							&& p.CallbackUrl == callbackUrl
							&& p.Icon == iconUrl
					),
					It.IsAny<EntityIdentifier>()), Times.Once());

			Assert.AreEqual(returnedGuid, response);
		}
	}
}
