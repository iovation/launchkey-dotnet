using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Directory;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace iovation.LaunchKey.Sdk.Tests.Client
{
	[TestClass]
	public class BasicDirectoryClientTests
	{
		[TestMethod]
		public void LinkDevice_ShouldCallAndReturnDataFromTransportLayer()
		{
			var mockTransport = new Mock<ITransport>();
			var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);

			mockTransport.Setup(
				t => t.DirectoryV3DevicesPost(
					It.IsAny<DirectoryV3DevicesPostRequest>(),
					It.IsAny<EntityIdentifier>()
				)
			).Returns(new DirectoryV3DevicesPostResponse { Code = "code", QrCode = "qrcode" });

			var response = client.LinkDevice("user id");

			Assert.AreEqual("code", response.Code);
			Assert.AreEqual("qrcode", response.QrCode);
		}

		[TestMethod]
		public void GetLinkedDevices_ShouldCallAndReturnDataFromTransportLayer()
		{
			var mockTransport = new Mock<ITransport>();
			var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);

			mockTransport.Setup(
				t => t.DirectoryV3DevicesListPost(
					It.IsAny<DirectoryV3DevicesListPostRequest>(),
					It.IsAny<EntityIdentifier>()
				)
			).Returns(new DirectoryV3DevicesListPostResponse(new List<DirectoryV3DevicesListPostResponse.Device>
			{
				new DirectoryV3DevicesListPostResponse.Device {Name = "my phone", Id = TestConsts.DefaultDeviceId, Created = TestConsts.DefaultTime, Status = 1, Type = "IOS", Updated = TestConsts.DefaultTime}
			}));

			var response = client.GetLinkedDevices("user id");

			Assert.IsTrue(response.Count == 1);

			Assert.AreEqual("my phone", response[0].Name);
			Assert.AreEqual("IOS", response[0].Type);
			Assert.AreEqual(TestConsts.DefaultDeviceId.ToString("N"), response[0].Id);
			Assert.AreEqual(TestConsts.DefaultTime, response[0].Created);
			Assert.AreEqual(TestConsts.DefaultTime, response[0].Updated);
			Assert.AreEqual(DeviceStatus.Linked, response[0].Status);
		}

		[TestMethod]
		public void UnlinkDevice_ShouldCallAndReturnDataFromTransportLayer()
		{
			var mockTransport = new Mock<ITransport>();
			var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);

			mockTransport.Setup(
				t => t.DirectoryV3DevicesDelete(
					It.IsAny<DirectoryV3DevicesDeleteRequest>(),
					It.IsAny<EntityIdentifier>()
				)
			);

			client.UnlinkDevice("user id", TestConsts.DefaultDeviceId.ToString("N"));

			mockTransport.Verify(p => p.DirectoryV3DevicesDelete(It.IsAny<DirectoryV3DevicesDeleteRequest>(), It.IsAny<EntityIdentifier>()));
		}

		[TestMethod]
		public void GetAllServiceSessions_ShouldCallAndReturnDataFromTransportLayer()
		{
			var mockTransport = new Mock<ITransport>();
			var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);

			mockTransport.Setup(
				t => t.DirectoryV3SessionsListPost(
					It.IsAny<DirectoryV3SessionsListPostRequest>(),
					It.IsAny<EntityIdentifier>()
				)
			).Returns(new DirectoryV3SessionsListPostResponse(new List<DirectoryV3SessionsListPostResponse.Session>
			{
				new DirectoryV3SessionsListPostResponse.Session
				{
					AuthRequest = TestConsts.DefaultAuthenticationId,
					Created = TestConsts.DefaultTime,
					ServiceIcon = "url",
					ServiceId = TestConsts.DefaultServiceId,
					ServiceName = "my name"
				}
			}));

			var response = client.GetAllServiceSessions("user id");

			Assert.IsTrue(response.Count == 1);
			var s = response[0];
			Assert.AreEqual(TestConsts.DefaultAuthenticationId, s.AuthRequest);
			Assert.AreEqual(TestConsts.DefaultTime, s.Created);
			Assert.AreEqual("url", s.ServiceIcon);
			Assert.AreEqual("my name", s.ServiceName);
			Assert.AreEqual(TestConsts.DefaultServiceId, s.ServiceId);
		}

		[TestMethod]
		public void EndAllServiceSessions_ShouldCallAndReturnDataFromTransportLayer()
		{
			var mockTransport = new Mock<ITransport>();
			var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);

			mockTransport.Setup(
				t => t.DirectoryV3SessionsDelete(
					It.IsAny<DirectoryV3SessionsDeleteRequest>(),
					It.IsAny<EntityIdentifier>()
				)
			);
			client.EndAllServiceSessions("user");

			mockTransport.Verify(p => p.DirectoryV3SessionsDelete(It.IsAny<DirectoryV3SessionsDeleteRequest>(), It.IsAny<EntityIdentifier>()));
		}
	}
}
