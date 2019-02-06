using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Transport.WebClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.WebClient
{
    [TestClass]
    public class CachedKeyTests
    {
        [TestMethod]
        public void TestMutability()
        {
            var rsa = new RSACryptoServiceProvider();
            var key = new CachedKey("test", rsa);

            Assert.AreEqual(key.Thumbprint, "test");
            Assert.AreEqual(key.KeyData, rsa);
        }
    }
}
