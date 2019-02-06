using System;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class ServiceV3AuthsGetResponseDeviceTests
    {
        [TestMethod]
        public void ShouldDeserialize()
        {
            var json = "{\"response\":true,\"auth_request\":\"8c3c0268-f692-11e7-bd2e-7692096aba47\",\"device_id\":\"Yrkt\"," +
                        "\"service_pins\":[\"8074\",\"8132\",\"5551\",\"7576\",\"7276\"]}";
            var o = JsonConvert.DeserializeObject<ServiceV3AuthsGetResponseDevice>(json);

            Assert.AreEqual(true, o.Response);
            Assert.AreEqual(Guid.Parse("8c3c0268-f692-11e7-bd2e-7692096aba47"), o.AuthorizationRequestId);
            Assert.AreEqual("Yrkt", o.DeviceId);
            Assert.AreEqual("8074", o.ServicePins[0]);
            Assert.AreEqual("8132", o.ServicePins[1]);
            Assert.AreEqual("5551", o.ServicePins[2]);
            Assert.AreEqual("7576", o.ServicePins[3]);
            Assert.AreEqual("7276", o.ServicePins[4]);
        }
    }
}
