using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class DirectoryV3DevicesPostRequestTests
    {
        [TestMethod]
        public void Constructor_ShouldSetIdProperly()
        {
            var o = new DirectoryV3DevicesPostRequest("id");
            Assert.AreEqual("id", o.Identifier);
        }

        [TestMethod]
        public void Constructor_ShouldSetTtlProperly()
        {
            var o = new DirectoryV3DevicesPostRequest("id", 1234);
            Assert.AreEqual(1234, o.TTL);
        }

        [TestMethod]
        public void ShouldSerializeCorrectlyWithoutTTL()
        {
            var encoder = new JsonNetJsonEncoder();
            var o = new DirectoryV3DevicesPostRequest("id");
            var json = encoder.EncodeObject(o);
            Assert.AreEqual("{\"identifier\":\"id\"}", json);
        }

        [TestMethod]
        public void ShouldSerializeCorrectlyWithTTL()
        {
            var encoder = new JsonNetJsonEncoder();
            var o = new DirectoryV3DevicesPostRequest("id", 1234);
            var json = encoder.EncodeObject(o);
            Assert.AreEqual("{\"identifier\":\"id\",\"ttl\":1234}", json);
        }

        [TestMethod]
        public void Equals_ShouldBeTrueWhenAllItemsEqual()
        {
            var left = new DirectoryV3DevicesPostRequest("id", 123);
            var right = new DirectoryV3DevicesPostRequest("id", 123);
            Assert.AreEqual(left, right);
        }

        [TestMethod]
        public void Equals_ShouldBeTrueWhenTtlIsNulll()
        {
            var left = new DirectoryV3DevicesPostRequest("id", null);
            var right = new DirectoryV3DevicesPostRequest("id", null);
            Assert.AreEqual(left, right);
        }

        [TestMethod]
        public void Equals_ShouldBeFalseWhenIdentifierIsDifferentl()
        {
            var left = new DirectoryV3DevicesPostRequest("id1");
            var right = new DirectoryV3DevicesPostRequest("id2");
            Assert.AreNotEqual(left, right);
        }

        [TestMethod]
        public void Equals_ShouldBeFalseWhenTtlIsDifferentl()
        {
            var left = new DirectoryV3DevicesPostRequest("id", 1);
            var right = new DirectoryV3DevicesPostRequest("id", 2);
            Assert.AreNotEqual(left, right);
        }

        [TestMethod]
        public void GetHashCode_ShouldBeEqualWhenAllItemsEqual()
        {
            var left = new DirectoryV3DevicesPostRequest("id", 123).GetHashCode();
            var right = new DirectoryV3DevicesPostRequest("id", 123).GetHashCode();
            Assert.AreEqual(left, right);
        }

        [TestMethod]
        public void GetHashCode_ShouldBeEqualWhenTtlIsNulll()
        {
            var left = new DirectoryV3DevicesPostRequest("id", null).GetHashCode();
            var right = new DirectoryV3DevicesPostRequest("id", null).GetHashCode();
            Assert.AreEqual(left, right);
        }

        [TestMethod]
        public void GetHashCode_ShouldNotBeEqualWhenIdentifierIsDifferentl()
        {
            var left = new DirectoryV3DevicesPostRequest("id1").GetHashCode();
            var right = new DirectoryV3DevicesPostRequest("id2").GetHashCode();
            Assert.AreNotEqual(left, right);
        }

        [TestMethod]
        public void GetHashCode_ShouldNotBeEqualWhenTtlIsDifferentl()
        {
            var left = new DirectoryV3DevicesPostRequest("id", 1).GetHashCode();
            var right = new DirectoryV3DevicesPostRequest("id", 2).GetHashCode();
            Assert.AreNotEqual(left, right);
        }

    }
}
