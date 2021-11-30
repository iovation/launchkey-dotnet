using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class DirectoryV3TotpPostRequestTests
    {
        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            var deviceGuid = Guid.NewGuid();
            var o = new DirectoryV3TotpPostRequest("id");
            Assert.AreEqual(o.Identifier, "id");
        }

        [TestMethod]
        public void ShouldSerializeCorrectly()
        {
            var encoder = new JsonNetJsonEncoder();
            var o = new DirectoryV3TotpPostRequest("id");
            var json = encoder.EncodeObject(o);
            Assert.AreEqual("{\"identifier\":\"id\"}", json);
        }
    }
}