using iovation.LaunchKey.Sdk.Domain.Directory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Domain.Directory
{
    [TestClass]
    public class DeviceStatusTests
    {
        [TestMethod]
        public void FromCode_ShouldReturnLinkPending()
        {
            var result = DeviceStatus.FromCode(0);

            Assert.IsTrue(result.IsActive == false);
            Assert.IsTrue(result.StatusCode == 0);
            Assert.IsTrue(result.Text == "LinkPending");
        }

        [TestMethod]
        public void FromCode_ShouldReturnLinked()
        {
            var result = DeviceStatus.FromCode(1);

            Assert.IsTrue(result.IsActive == true);
            Assert.IsTrue(result.StatusCode == 1);
            Assert.IsTrue(result.Text == "Linked");
        }

        [TestMethod]
        public void FromCode_ShouldReturnUnlinkPending()
        {
            var result = DeviceStatus.FromCode(2);

            Assert.IsTrue(result.IsActive == false);
            Assert.IsTrue(result.StatusCode == 2);
            Assert.IsTrue(result.Text == "UnlinkPending");
        }
    }
}
