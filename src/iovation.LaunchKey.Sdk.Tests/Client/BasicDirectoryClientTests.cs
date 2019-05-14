using System;
using System.Collections.Generic;
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
            var expectedRequest = new DirectoryV3DevicesPostRequest("user id", 123);

            mockTransport.Setup(
                t => t.DirectoryV3DevicesPost(
                    It.IsAny<DirectoryV3DevicesPostRequest>(),
                    It.IsAny<EntityIdentifier>()
                )
            ).Returns(new DirectoryV3DevicesPostResponse { Code = "code", QrCode = "qrcode", DeviceId = "deviceID" });

            var response = client.LinkDevice("user id", 123);

            Assert.AreEqual("code", response.Code);
            Assert.AreEqual("qrcode", response.QrCode);
            mockTransport.Verify(
                x => x.DirectoryV3DevicesPost(It.Is<DirectoryV3DevicesPostRequest>(
                        r => r.Identifier == "user id"
                            && r.TTL == 123
                    ),
                    It.IsAny<EntityIdentifier>()), Times.Once());
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
            Assert.AreEqual(TestConsts.DefaultDeviceId.ToString("D"), response[0].Id);
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

            client.UnlinkDevice("user id", TestConsts.DefaultDeviceId.ToString("D"));

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

        [TestMethod]
        public void CreateService_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var callbackUrl = new Uri("http://example.com");
            var iconUrl = new Uri("http://example.com/icon");
            var returnedGuid = Guid.NewGuid();

            mockTransport.Setup(p =>
                    p.DirectoryV3ServicesPost(
                        It.IsAny<ServicesPostRequest>(),
                        It.IsAny<EntityIdentifier>()
                    )
                )
                .Returns(new ServicesPostResponse { Id = returnedGuid });

            var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);
            var response = client.CreateService("service name", "desc", iconUrl, callbackUrl, true);

            // verify the call made it to transport with right params
            mockTransport.Verify(
                x => x.DirectoryV3ServicesPost(It.Is<ServicesPostRequest>(
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
                p.DirectoryV3ServicesPatch(
                    It.IsAny<ServicesPatchRequest>(),
                    It.IsAny<EntityIdentifier>()
                )
            );

            var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);
            client.UpdateService(serviceId, "service name", "desc", iconUrl, callbackUrl, true);

            // verify the call made it to transport with right params
            mockTransport.Verify(
                x => x.DirectoryV3ServicesPatch(It.Is<ServicesPatchRequest>(
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
                p.DirectoryV3ServicesListPost(
                    It.IsAny<ServicesListPostRequest>(),
                    It.IsAny<EntityIdentifier>()
                )
            )
            .Returns(new ServicesListPostResponse(new List<ServicesListPostResponse.Service> { serviceObject }));

            var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);
            var responseObject = client.GetService(serviceId);

            // verify the call made it to transport with right params
            mockTransport.Verify(
                x => x.DirectoryV3ServicesListPost(It.Is<ServicesListPostRequest>(
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
                    p.DirectoryV3ServicesListPost(
                        It.IsAny<ServicesListPostRequest>(),
                        It.IsAny<EntityIdentifier>()
                    )
                )
                .Returns(new ServicesListPostResponse(new List<ServicesListPostResponse.Service> { serviceObject, serviceObject2 }));

            var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);
            var responseObject = client.GetServices(new List<Guid> { serviceId, serviceId2 });

            // verify the call made it to transport with right params
            mockTransport.Verify(
                x => x.DirectoryV3ServicesListPost(It.Is<ServicesListPostRequest>(
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
            var serviceObject = new ServicesGetResponse.Service { Active = true, CallbackUrl = callbackUrl, Description = "one service", Name = "my service", Icon = iconUrl, Id = Guid.NewGuid() };

            mockTransport.Setup(p => p.DirectoryV3ServicesGet(It.IsAny<EntityIdentifier>()))
                .Returns(new ServicesGetResponse(new List<ServicesGetResponse.Service> { serviceObject }))
                .Verifiable();

            var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);
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

        [TestMethod]
        public void AddServicePublicKey_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var serviceId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.DirectoryV3ServiceKeysPost(It.Is<ServiceKeysPostRequest>(x =>
                    x.Active == true
                    && x.Expires == new DateTime(2020, 1, 1).ToUniversalTime()
                    && x.PublicKey == "keyhere"
                    && x.ServiceId == serviceId), It.IsAny<EntityIdentifier>()))
                .Returns(new KeysPostResponse { Id = "keyid" })
                .Verifiable();

            var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);

            var response = client.AddServicePublicKey(serviceId, "keyhere", true, new DateTime(2020, 1, 1));

            mockTransport.Verify();

            Assert.IsTrue(response == "keyid");
        }

        [TestMethod]
        public void UpdateServicePublicKey_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var serviceId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.DirectoryV3ServiceKeysPatch(
                    It.Is<ServiceKeysPatchRequest>(x =>
                        x.Active == true
                        && x.Expires == new DateTime(2020, 1, 1).ToUniversalTime()
                        && x.KeyId == "keyid"
                        && x.ServiceId == serviceId
                    ),
                    TestConsts.DefaultDirectoryEntity
                )).Verifiable();

            var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);

            client.UpdateServicePublicKey(serviceId, "keyid", true, new DateTime(2020, 1, 1));

            mockTransport.Verify();
        }

        [TestMethod]
        public void RemoveServicePublicKey_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var serviceId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.DirectoryV3ServiceKeysDelete(
                    It.Is<ServiceKeysDeleteRequest>(x =>
                        x.KeyId == "keyid"
                        && x.ServiceId == serviceId
                    ),
                    TestConsts.DefaultDirectoryEntity
                )).Verifiable();

            var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);

            client.RemoveServicePublicKey(serviceId, "keyid");

            mockTransport.Verify();
        }

        [TestMethod]
        public void GetServicePublicKeys_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var serviceId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.DirectoryV3ServiceKeysListPost(
                    It.Is<ServiceKeysListPostRequest>(x => x.ServiceId == serviceId)
                    , TestConsts.DefaultDirectoryEntity))
                .Returns(new KeysListPostResponse(new List<KeysListPostResponse.Key> { new KeysListPostResponse.Key
                {
                    Active = true,
                    Created = new DateTime(2020, 1, 1),
                    Expires = new DateTime(2021, 1,1),
                    Id = "id",
                    PublicKey = "k"
                }}))
                .Verifiable();

            var client = new BasicDirectoryClient(TestConsts.DefaultDirectoryId, mockTransport.Object);

            var result = client.GetServicePublicKeys(serviceId);

            mockTransport.Verify();

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].Active == true);
            Assert.IsTrue(result[0].Created == new DateTime(2020, 1, 1));
            Assert.IsTrue(result[0].Expires == new DateTime(2021, 1, 1));
            Assert.IsTrue(result[0].Id == "id");
        }
    }
}
