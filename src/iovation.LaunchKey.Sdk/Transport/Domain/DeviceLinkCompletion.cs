using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DeviceLinkCompletion //: IServerSentEvent
    {
        [JsonProperty("type")]
        public string Type { get; }

        [JsonProperty("device_id")]
        public string DeviceId { get; }

        [JsonProperty("device_public_key")]
        public string DevicePublicKey { get; }

        [JsonProperty("device_public_key_id")]
        public string DevicePublicKeyId { get; }
    }
}
