using System;
namespace iovation.LaunchKey.Sdk.Domain.Directory
{
    public class DeviceLinkCompletionResponse
    {
        public string DeviceId { get; }
        public string DevicePublicKey { get; }
        public string DevicePublicKeyId { get; }

        public DeviceLinkCompletionResponse(
            string deviceId, string devicePublicKey, string devicePublicKeyId)
        {
            DeviceId = deviceId;
            DevicePublicKey = devicePublicKey;
            DevicePublicKeyId = devicePublicKeyId;
        }
    }
}
