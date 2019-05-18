using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DeviceLinkCompletion
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("device_public_key")]
        public string DevicePublicKey { get; set; }

        [JsonProperty("device_public_key_id")]
        public string DevicePublicKeyId { get; set; }
    }
}
