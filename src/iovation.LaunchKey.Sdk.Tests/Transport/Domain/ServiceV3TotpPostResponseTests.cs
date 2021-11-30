using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class ServiceV3TotpPostResponseTests
    {
        [TestMethod]
        public void ShouldDeserialize()
        {
            var json = "{\"valid\": true}";
            var obj = JsonConvert.DeserializeObject<ServiceV3TotpPostResponse>(json);

            Assert.IsTrue(obj.Valid);
        }
    }
}