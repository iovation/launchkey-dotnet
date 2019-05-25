using System;
namespace iovation.LaunchKey.Sdk.Domain.Directory
{
    public class DeviceLinkCompletion
    {
        public string DeviceId { get; }
        public string DevicePublicKey { get; }
        public string DevicePublicKeyId { get; }

        public DeviceLinkCompletion(
            string deviceId, string devicePublicKey, string devicePublicKeyId)
        {
            DeviceId = deviceId;
            DevicePublicKey = devicePublicKey;
            DevicePublicKeyId = devicePublicKeyId;
        }
    }
}
