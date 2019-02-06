using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class OrganizationV3DirectoriesPatchRequestTests
    {
        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            var request = new OrganizationV3DirectoriesPatchRequest(TestConsts.DefaultDirectoryId, true, "a", "i");

            Assert.AreEqual(true, request.Active);
            Assert.AreEqual("a", request.AndroidKey);
            Assert.AreEqual("i", request.IosP12);
            Assert.AreEqual(TestConsts.DefaultDirectoryId, request.DirectoryId);
        }
    }
}
