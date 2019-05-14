using System;
namespace iovation.LaunchKey.Sdk.Domain.Directory
{
    public class DeviceLinkCompletion
    {
        public string Type { get; }
        public string DeviceId { get; }
        public string DevicePublicKey { get; }
        public string DevicePublicKeyId { get; }

        public DeviceLinkCompletion(string type, string deviceId, 
            string devicePublicKey, string devicePublicKeyId)
        {
            Type = type;
            DeviceId = deviceId;
            DevicePublicKey = devicePublicKey;
            DevicePublicKeyId = devicePublicKeyId;
        }
    }
}
