using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class PublicV3PublicKeyGetResponseTests
    {
        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            var o = new PublicV3PublicKeyGetResponse("key", "finger");
            Assert.AreEqual("key", o.PublicKey);
            Assert.AreEqual("finger", o.PublicKeyFingerPrint);
        }
    }
}
