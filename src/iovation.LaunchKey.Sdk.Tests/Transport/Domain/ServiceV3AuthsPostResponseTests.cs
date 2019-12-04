using System;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class ServiceV3AuthsPostResponseTests
    {
        [TestMethod]
        public void ShouldDeserialize_WithJustAuthRequest_SetsAuthRequest()
        {
            var json = "{\"auth_request\": \"e4629564-f693-11e7-8e81-328aef89fa8b\"}";
            var o = JsonConvert.DeserializeObject<ServiceV3AuthsPostResponse>(json);

            Assert.AreEqual(Guid.Parse("e4629564-f693-11e7-8e81-328aef89fa8b"), o.AuthRequest);
        }

        [TestMethod]
        public void ShouldDeserialize_WithJustAuthRequest_SetsPushPackageToNull()
        {
            var json = "{\"auth_request\": \"e4629564-f693-11e7-8e81-328aef89fa8b\"}";
            var o = JsonConvert.DeserializeObject<ServiceV3AuthsPostResponse>(json);

            Assert.IsNull(o.PushPackage);
        }


        [TestMethod]
        public void ShouldDeserialize_WithAuthRequestAndPushPackage_SetsAuthRequest()
        {
            var json = "{\"auth_request\": \"e4629564-f693-11e7-8e81-328aef89fa8b\", \"push_package\": \"Expected Push Package\"}";
            var o = JsonConvert.DeserializeObject<ServiceV3AuthsPostResponse>(json);

            Assert.AreEqual(Guid.Parse("e4629564-f693-11e7-8e81-328aef89fa8b"), o.AuthRequest);
        }

        [TestMethod]
        public void ShouldDeserialize_WithAuthRequestAndPushPackage_SetsPushPackage()
        {
            var json = "{\"auth_request\": \"e4629564-f693-11e7-8e81-328aef89fa8b\", \"push_package\": \"Expected Push Package\"}";
            var o = JsonConvert.DeserializeObject<ServiceV3AuthsPostResponse>(json);

            Assert.AreEqual("Expected Push Package", o.PushPackage);
        }

        [TestMethod]
        public void ShouldDeserialize_WithAuthRequest_PushPackage_DeviceIds()
        {
            var json = "{\"auth_request\": \"e4629564-f693-11e7-8e81-328aef89fa8b\", \"push_package\": \"Expected Push Package\",\"device_ids\": [\"device_a\", \"device_b\"]}";
            var o = JsonConvert.DeserializeObject<ServiceV3AuthsPostResponse>(json);

            Assert.AreEqual(2, o.DeviceIDs.Count);
            Assert.AreEqual(o.DeviceIDs[0], "device_a");
            Assert.AreEqual(o.DeviceIDs[1], "device_b");
        }
    }
}
