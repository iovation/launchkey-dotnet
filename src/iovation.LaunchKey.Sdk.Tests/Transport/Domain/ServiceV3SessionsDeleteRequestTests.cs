using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class ServiceV3SessionsDeleteRequestTests
    {
        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            var o = new ServiceV3SessionsDeleteRequest("un");
            Assert.AreEqual(o.Username, "un");
        }

        [TestMethod]
        public void ShouldSerializeCorrectly()
        {
            var encoder = new JsonNetJsonEncoder();
            var o = new ServiceV3SessionsDeleteRequest("my-unique-user-identifier");
            var json = encoder.EncodeObject(o);
            Assert.AreEqual("{\"username\":\"my-unique-user-identifier\"}", json);
        }
    }
}
