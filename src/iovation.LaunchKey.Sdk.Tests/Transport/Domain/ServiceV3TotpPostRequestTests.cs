using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class ServiceV3TotpPostRequestTests
    {
        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            var deviceGuid = Guid.NewGuid();
            var o = new ServiceV3TotpPostRequest("id", "123456");
            Assert.AreEqual(o.Identifier, "id");
            Assert.AreEqual(o.Otp, "123456");
        }

        [TestMethod]
        public void ShouldSerializeCorrectly()
        {
            var encoder = new JsonNetJsonEncoder();
            var o = new ServiceV3TotpPostRequest("id", "123456");
            var json = encoder.EncodeObject(o);
            Assert.AreEqual("{\"identifier\":\"id\",\"otp\":\"123456\"}", json);
        }
    }
}