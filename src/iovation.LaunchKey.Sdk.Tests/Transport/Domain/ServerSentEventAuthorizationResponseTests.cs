using System;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class ServerSentEventAuthorizationResponseTests
    {
        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            var requestingEntity = new EntityIdentifier(EntityType.Directory, Guid.NewGuid());
            var serviceId = Guid.NewGuid();
            var authorizationRequestId = Guid.NewGuid();
            var devicePins = new[] { "1", "2" };
            var o = new ServerSentEventAuthorizationResponse(
                requestingEntity,
                serviceId,
                "userhash",
                "orghash",
                "userpush",
                authorizationRequestId,
                true,
                "deviceId",
                devicePins,
                "Type",
                "Reason",
                "Denial Reason",
                null,
                null
            );

            Assert.AreSame(requestingEntity, o.RequestingEntity);
            Assert.AreEqual(serviceId, o.ServiceId);
            Assert.AreEqual("userhash", o.ServiceUserHash);
            Assert.AreEqual("orghash", o.OrganizationUserHash);
            Assert.AreEqual("userpush", o.UserPushId);
            Assert.AreEqual(authorizationRequestId, o.AuthorizationRequestId);
            Assert.AreEqual(true, o.Response);
            Assert.AreEqual("deviceId", o.DeviceId);
            Assert.AreSame(devicePins, o.DevicePins);
            Assert.AreEqual("Type", o.Type);
            Assert.AreEqual("Reason", o.Reason);
            Assert.AreEqual("Denial Reason", o.DenialReason);
            Assert.AreEqual(null, o.AuthPolicy);
            Assert.AreEqual(null, o.AuthMethods);
        }
    }
}
