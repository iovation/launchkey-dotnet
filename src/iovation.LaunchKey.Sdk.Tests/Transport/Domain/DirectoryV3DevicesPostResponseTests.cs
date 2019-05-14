using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class DirectoryV3DevicesPostResponseTests
    {
        [TestMethod]
        public void ShouldDeserialize()
        {
            var json = "{\"qrcode\": \"https://api.launchkey.com/public/v3/qr/xmchhhc\", \"code\": \"xmchhhc\", \"device_id\":\"61b9722d-2c87-4eb4-80fc-985c900472ba\"}";
            var obj = JsonConvert.DeserializeObject<DirectoryV3DevicesPostResponse>(json);

            Assert.IsTrue(obj.Code == "xmchhhc");
            Assert.IsTrue(obj.QrCode == "https://api.launchkey.com/public/v3/qr/xmchhhc");
            Assert.IsTrue(obj.DeviceId == "61b9722d-2c87-4eb4-80fc-985c900472ba");
        }
    }
}
