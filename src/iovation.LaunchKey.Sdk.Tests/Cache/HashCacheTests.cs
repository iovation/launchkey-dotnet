using iovation.LaunchKey.Sdk.Cache;
using iovation.LaunchKey.Sdk.Error;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Cache
{
    [TestClass]
    public class HashCacheTests
    {
        [TestMethod]
        public void TestPersistence()
        {
            var cache = new HashCache();
            cache.Put("test_key", "test_value");
            Assert.AreEqual(cache.Get("test_key"), "test_value");
        }

        [TestMethod]
        [ExpectedException(typeof(CacheException))]
        public void TestGetInvalidKey()
        {
            var cache = new HashCache();
            cache.Get("Doesn't exist");
        }

        [TestMethod]
        [ExpectedException(typeof(CacheException))]
        public void TestSetInvalidKey()
        {
            var cache = new HashCache();
            cache.Put(null, "value");
        }
    }
}
