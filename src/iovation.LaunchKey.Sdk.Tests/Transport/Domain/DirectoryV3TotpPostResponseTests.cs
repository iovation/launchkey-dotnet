using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class DirectoryV3TotpPostResponseTests
    {
        [TestMethod]
        public void ShouldDeserialize()
        {
            var json = "{\"digits\": 6, \"secret\": \"ABCDEFG\", \"period\": 30, \"algorithm\": \"SHA1\"}";
            var obj = JsonConvert.DeserializeObject<DirectoryV3TotpPostResponse>(json);

            Assert.IsTrue(obj.Secret == "ABCDEFG");
            Assert.IsTrue(obj.Algorithm == "SHA1");
            Assert.IsTrue(obj.Period == 30);
            Assert.IsTrue(obj.Digits == 6);
        }
    }
}