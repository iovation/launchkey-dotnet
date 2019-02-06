using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ServiceV3AuthsGetResponseDevice
    {
        [JsonProperty("response")]
        public bool Response { get; set; }

        [JsonProperty("auth_request")]
        public Guid AuthorizationRequestId { get; set; }

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("service_pins")]
        public string[] ServicePins { get; set; }
    }
}