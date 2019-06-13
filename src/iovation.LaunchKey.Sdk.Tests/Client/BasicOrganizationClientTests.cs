using System;
using System.Collections.Generic;
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
                .Returns(new ServicesPostResponse { Id = returnedGuid });

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
            var responseObject = client.GetServices(new List<Guid> { serviceId, serviceId2 });

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
            var serviceObject = new ServicesGetResponse.Service { Active = true, CallbackUrl = callbackUrl, Description = "one service", Name = "my service", Icon = iconUrl, Id = Guid.NewGuid() };

            mockTransport.Setup(p => p.OrganizationV3ServicesGet(It.IsAny<EntityIdentifier>()))
                .Returns(new ServicesGetResponse(new List<ServicesGetResponse.Service> { serviceObject }))
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

        [TestMethod]
        public void CreateDirectory_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var returnedId = Guid.NewGuid();
            mockTransport.Setup(p =>
                    p.OrganizationV3DirectoriesPost(
                        It.Is<OrganizationV3DirectoriesPostRequest>(x => x.Name == "x"),
                        It.IsAny<EntityIdentifier>()))
                .Returns(new OrganizationV3DirectoriesPostResponse { Id = returnedId })
                .Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);
            var response = client.CreateDirectory("x");

            Assert.AreEqual(returnedId, response);
        }

        [TestMethod]
        public void UpdateDirectory_ShouldCallTransportWithCorrectParams()
        {
            var mock = new Mock<ITransport>();
            mock.Setup(p =>
                p.OrganizationV3DirectoriesPatch(
                    It.Is<OrganizationV3DirectoriesPatchRequest>(
                        x => x.Active == true
                            && x.AndroidKey == "a"
                            && x.IosP12 == "i"
                            && x.DirectoryId == TestConsts.DefaultDirectoryId
                        ),
                    It.IsAny<EntityIdentifier>()))
                .Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mock.Object);

            client.UpdateDirectory(TestConsts.DefaultDirectoryId, true, "a", "i", TestConsts.DefaultWebhookUrl);
            mock.Verify();
        }

        [TestMethod]
        public void GetDirectory_ShouldCallTransportWithCorrectParams()
        {
            var mock = new Mock<ITransport>();
            var did = Guid.NewGuid();
            var sdkid = Guid.NewGuid();
            var sid = Guid.NewGuid();
            mock.Setup(
                m => m.OrganizationV3DirectoriesListPost(
                    It.IsAny<OrganizationV3DirectoriesListPostRequest>(),
                    It.IsAny<EntityIdentifier>()
                ))
                .Returns(new OrganizationV3DirectoriesListPostResponse(new List<OrganizationV3DirectoriesListPostResponse.Directory>
                {
                    new OrganizationV3DirectoriesListPostResponse.Directory
                    {
                        Active = true,
                        AndroidKey = "a",
                        Id = did,
                        IosCertificateFingerprint = "i",
                        Name = "n",
                        SdkKeys = new List<Guid>
                        {
                            sdkid
                        },
                        ServiceIds = new List<Guid>
                        {
                            sid
                        }
                    }
                }));


            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mock.Object);
            var response = client.GetDirectory(Guid.NewGuid());

            Assert.IsTrue(response != null);
            Assert.IsTrue(response.Active);
            Assert.IsTrue(response.AndroidKey == "a");
            Assert.IsTrue(response.IosCertificateFingerprint == "i");
            Assert.IsTrue(response.Name == "n");
            Assert.IsTrue(response.SdkKeys.Count == 1);
            Assert.IsTrue(response.SdkKeys[0] == sdkid);
            Assert.IsTrue(response.ServiceIds.Count == 1);
            Assert.IsTrue(response.ServiceIds[0] == sid);
        }


        [TestMethod]
        public void GetDirectories_ShouldCallTransportWithCorrectParams()
        {
            var mock = new Mock<ITransport>();
            var did = Guid.NewGuid();
            var sdkid = Guid.NewGuid();
            var sid = Guid.NewGuid();
            mock.Setup(
                    m => m.OrganizationV3DirectoriesListPost(
                        It.IsAny<OrganizationV3DirectoriesListPostRequest>(),
                        It.IsAny<EntityIdentifier>()
                    ))
                .Returns(new OrganizationV3DirectoriesListPostResponse(new List<OrganizationV3DirectoriesListPostResponse.Directory>
                {
                    new OrganizationV3DirectoriesListPostResponse.Directory
                    {
                        Active = true,
                        AndroidKey = "a",
                        Id = did,
                        IosCertificateFingerprint = "i",
                        Name = "n",
                        SdkKeys = new List<Guid>
                        {
                            sdkid
                        },
                        ServiceIds = new List<Guid>
                        {
                            sid
                        }
                    }
                }));


            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mock.Object);
            var response = client.GetDirectories(new List<Guid>() { Guid.NewGuid() });

            Assert.IsTrue(response != null);
            Assert.IsTrue(response.Count == 1);
            Assert.IsTrue(response[0].Active);
            Assert.IsTrue(response[0].AndroidKey == "a");
            Assert.IsTrue(response[0].IosCertificateFingerprint == "i");
            Assert.IsTrue(response[0].Name == "n");
            Assert.IsTrue(response[0].SdkKeys.Count == 1);
            Assert.IsTrue(response[0].SdkKeys[0] == sdkid);
            Assert.IsTrue(response[0].ServiceIds.Count == 1);
            Assert.IsTrue(response[0].ServiceIds[0] == sid);
        }

        [TestMethod]
        public void GetAllDirectories_ShouldCallTransportWithCorrectParams()
        {
            var mock = new Mock<ITransport>();
            var did = Guid.NewGuid();
            var sdkid = Guid.NewGuid();
            var sid = Guid.NewGuid();
            mock.Setup(m => m.OrganizationV3DirectoriesGet(It.IsAny<EntityIdentifier>()))
                .Returns(new OrganizationV3DirectoriesGetResponse(new List<OrganizationV3DirectoriesGetResponse.Directory>
                {
                    new OrganizationV3DirectoriesGetResponse.Directory
                    {
                        Active = true,
                        AndroidKey = "a",
                        Id = did,
                        IosCertificateFingerprint = "i",
                        Name = "n",
                        WebhookUrl = TestConsts.DefaultWebhookUrl,
                        SdkKeys = new List<Guid>
                        {
                            sdkid
                        },
                        ServiceIds = new List<Guid>
                        {
                            sid
                        }
                    }
                }));

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mock.Object);
            var response = client.GetAllDirectories();

            Assert.IsTrue(response != null);
            Assert.IsTrue(response.Count == 1);
            Assert.IsTrue(response[0].Active);
            Assert.IsTrue(response[0].AndroidKey == "a");
            Assert.IsTrue(response[0].IosCertificateFingerprint == "i");
            Assert.IsTrue(response[0].WebhookUrl == TestConsts.DefaultWebhookUrl);
            Assert.IsTrue(response[0].Name == "n");
            Assert.IsTrue(response[0].SdkKeys.Count == 1);
            Assert.IsTrue(response[0].SdkKeys[0] == sdkid);
            Assert.IsTrue(response[0].ServiceIds.Count == 1);
            Assert.IsTrue(response[0].ServiceIds[0] == sid);
        }

        [TestMethod]
        public void GenerateAndAddDirectorySdkKey_InteractsWithTransport()
        {
            var mock = new Mock<ITransport>();
            var did = Guid.NewGuid();
            var orgId = Guid.NewGuid();
            var sdkKey = Guid.NewGuid();

            mock.Setup(
                    m => m.OrganizationV3DirectorySdkKeysPost(
                        It.IsAny<OrganizationV3DirectorySdkKeysPostRequest>(),
                        It.IsAny<EntityIdentifier>()
                    )
                )
                .Returns(new OrganizationV3DirectorySdkKeysPostResponse { SdkKey = sdkKey })
                .Verifiable();

            var client = new BasicOrganizationClient(orgId, mock.Object);
            var response = client.GenerateAndAddDirectorySdkKey(did);

            mock.Verify();
            Assert.AreEqual(sdkKey, response);
        }

        [TestMethod]
        public void RemoveDirectorySdkKey_InteractsWithTransport()
        {
            var mock = new Mock<ITransport>();
            var did = Guid.NewGuid();
            var orgId = Guid.NewGuid();
            var sdkKey = Guid.NewGuid();

            mock.Setup(p => p.OrganizationV3DirectorySdkKeysDelete(
                It.Is<OrganizationV3DirectorySdkKeysDeleteRequest>(e => e.DirectoryId == did && e.SdkKey == sdkKey),
                It.Is<EntityIdentifier>(e => e.Id == orgId && e.Type == EntityType.Organization)
            )).Verifiable();

            var client = new BasicOrganizationClient(orgId, mock.Object);
            client.RemoveDirectorySdkKey(did, sdkKey);

            mock.Verify();
        }

        [TestMethod]
        public void GetAllDirectorySdkKeys_InteractsWithTransport()
        {
            // setup
            var mock = new Mock<ITransport>();
            var did = Guid.NewGuid();
            var sdkKey1 = Guid.NewGuid();
            var sdkKey2 = Guid.NewGuid();
            var orgId = Guid.NewGuid();

            mock.Setup(p => p.OrganizationV3DirectorySdkKeysListPost(
                It.Is<OrganizationV3DirectorySdkKeysListPostRequest>(e => e.DirectoryId == did),
                It.Is<EntityIdentifier>(e => e.Id == orgId && e.Type == EntityType.Organization)
            ))
            .Returns(new OrganizationV3DirectorySdkKeysListPostResponse(new List<Guid> { sdkKey1, sdkKey2 }))
            .Verifiable();

            // exec
            var client = new BasicOrganizationClient(orgId, mock.Object);
            var response = client.GetAllDirectorySdkKeys(did);

            // verify
            mock.Verify();
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count == 2);
            Assert.AreEqual(sdkKey1, response[0]);
            Assert.AreEqual(sdkKey2, response[1]);
        }

        [TestMethod]
        public void AddServicePublicKey_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var serviceId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.OrganizationV3ServiceKeysPost(It.Is<ServiceKeysPostRequest>(x =>
                    x.Active == true
                    && x.Expires == new DateTime(2020, 1, 1).ToUniversalTime()
                    && x.PublicKey == "keyhere"
                    && x.ServiceId == serviceId), It.IsAny<EntityIdentifier>()))
                .Returns(new KeysPostResponse { Id = "keyid" })
                .Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);

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
                .Setup(p => p.OrganizationV3ServiceKeysPatch(
                    It.Is<ServiceKeysPatchRequest>(x =>
                        x.Active == true
                        && x.Expires == new DateTime(2020, 1, 1).ToUniversalTime()
                        && x.KeyId == "keyid"
                        && x.ServiceId == serviceId
                    ),
                    TestConsts.DefaultOrganizationEntity
                )).Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);

            client.UpdateServicePublicKey(serviceId, "keyid", true, new DateTime(2020, 1, 1));

            mockTransport.Verify();
        }

        [TestMethod]
        public void RemoveServicePublicKey_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var serviceId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.OrganizationV3ServiceKeysDelete(
                    It.Is<ServiceKeysDeleteRequest>(x =>
                        x.KeyId == "keyid"
                        && x.ServiceId == serviceId
                    ),
                    TestConsts.DefaultOrganizationEntity
                )).Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);

            client.RemoveServicePublicKey(serviceId, "keyid");

            mockTransport.Verify();
        }

        [TestMethod]
        public void GetServicePublicKeys_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var serviceId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.OrganizationV3ServiceKeysListPost(
                    It.Is<ServiceKeysListPostRequest>(x => x.ServiceId == serviceId)
                    , TestConsts.DefaultOrganizationEntity))
                .Returns(new KeysListPostResponse(new List<KeysListPostResponse.Key> { new KeysListPostResponse.Key
                {
                    Active = true,
                    Created = new DateTime(2020, 1, 1),
                    Expires = new DateTime(2021, 1,1),
                    Id = "id",
                    PublicKey = "k"
                }}))
                .Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);

            var result = client.GetServicePublicKeys(serviceId);

            mockTransport.Verify();

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].Active == true);
            Assert.IsTrue(result[0].Created == new DateTime(2020, 1, 1));
            Assert.IsTrue(result[0].Expires == new DateTime(2021, 1, 1));
            Assert.IsTrue(result[0].Id == "id");
        }

        [TestMethod]
        public void GetDirectoryPublicKeys_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var directoryId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.OrganizationV3DirectoryKeysListPost(
                    It.Is<DirectoryKeysListPostRequest>(x => x.DirectoryId == directoryId)
                    , TestConsts.DefaultOrganizationEntity))
                .Returns(new KeysListPostResponse(new List<KeysListPostResponse.Key> { new KeysListPostResponse.Key
                {
                    Active = true,
                    Created = new DateTime(2020, 1, 1),
                    Expires = new DateTime(2021, 1,1),
                    Id = "id",
                    PublicKey = "k"
                }}))
                .Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);

            var result = client.GetDirectoryPublicKeys(directoryId);

            mockTransport.Verify();

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].Active == true);
            Assert.IsTrue(result[0].Created == new DateTime(2020, 1, 1));
            Assert.IsTrue(result[0].Expires == new DateTime(2021, 1, 1));
            Assert.IsTrue(result[0].Id == "id");
        }

        [TestMethod]
        public void AddDirectoryPublicKey_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var directoryId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.OrganizationV3DirectoryKeysPost(It.Is<DirectoryKeysPostRequest>(x =>
                    x.Active == true
                    && x.Expires == new DateTime(2020, 1, 1).ToUniversalTime()
                    && x.PublicKey == "keyhere"
                    && x.DirectoryId == directoryId), It.IsAny<EntityIdentifier>()))
                .Returns(new KeysPostResponse { Id = "keyid" })
                .Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);

            var response = client.AddDirectoryPublicKey(directoryId, "keyhere", true, new DateTime(2020, 1, 1));

            mockTransport.Verify();

            Assert.IsTrue(response == "keyid");
        }

        [TestMethod]
        public void UpdateDirectoryPublicKey_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var directoryId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.OrganizationV3DirectoryKeysPatch(
                    It.Is<DirectoryKeysPatchRequest>(x =>
                        x.Active == true
                        && x.Expires == new DateTime(2020, 1, 1).ToUniversalTime()
                        && x.KeyId == "keyid"
                        && x.DirectoryId == directoryId
                    ),
                    TestConsts.DefaultOrganizationEntity
                )).Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);

            client.UpdateDirectoryPublicKey(directoryId, "keyid", true, new DateTime(2020, 1, 1));

            mockTransport.Verify();
        }

        [TestMethod]
        public void RemoveDirectoryPublicKey_ShouldCallTransportWithCorrectParams()
        {
            var mockTransport = new Mock<ITransport>();
            var directoryId = Guid.NewGuid();

            mockTransport
                .Setup(p => p.OrganizationV3DirectoryKeysDelete(
                    It.Is<DirectoryKeysDeleteRequest>(x =>
                        x.KeyId == "keyid"
                        && x.DirectoryId == directoryId
                    ),
                    TestConsts.DefaultOrganizationEntity
                )).Verifiable();

            var client = new BasicOrganizationClient(TestConsts.DefaultOrgId, mockTransport.Object);

            client.RemoveDirectoryPublicKey(directoryId, "keyid");

            mockTransport.Verify();
        }
    }
}
