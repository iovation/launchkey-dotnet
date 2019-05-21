using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class ServerSentEventAuthorizationResponseTests
    {
        public AuthPolicy.AuthMethod CreateAuthTransportMethod(string method, bool? set, bool active, bool allowed, bool supported, bool? userRequired, bool? passed, bool? error)
        {
            var authMethod = new AuthPolicy.AuthMethod
            {
                Method = method,
                Active = active,
                Set = set,
                Allowed = allowed,
                Supported = supported,
                UserRequired = userRequired,
                Passed = passed,
                Error = error
            };
            return authMethod;

        }

        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            var requestingEntity = new EntityIdentifier(EntityType.Directory, Guid.NewGuid());
            var serviceId = Guid.NewGuid();
            var authorizationRequestId = Guid.NewGuid();
            var devicePins = new[] { "1", "2" };

            var authPolicy = new AuthPolicy.JWEAuthPolicy
            {
                Requirement = "types",
                Types = new List<string> { "possession" }
            };

            var authMethods = new AuthPolicy.AuthMethod[7]
            {
                CreateAuthTransportMethod("wearables", false, false, true, true, null, null, null),
                CreateAuthTransportMethod("geofencing", null, true, true, true, null, null, null),
                CreateAuthTransportMethod("locations", true, true, true, true, true, false, false),
                CreateAuthTransportMethod("pin_code", true, true, true, true, true, null, null),
                CreateAuthTransportMethod("circle_code", false, false, true, true, null, null, null),
                CreateAuthTransportMethod("face", false, false, true, true, null, null, null),
                CreateAuthTransportMethod("fingerprint", false, false, true, true, null, null, null)
            };

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
                authPolicy,
                authMethods
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
            Assert.AreEqual("possession", o.AuthPolicy.Types[0]);
            Assert.AreEqual(0, o.AuthPolicy.Amount);
            Assert.AreEqual("types", o.AuthPolicy.Requirement);
            Assert.IsNotNull(o.AuthMethods);
        }

        [TestMethod]
        public void Constructor_ShouldSetProperties_WithNullAuthMethodInsight()
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
