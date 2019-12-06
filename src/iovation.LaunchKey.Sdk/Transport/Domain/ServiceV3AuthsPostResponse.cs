using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ServiceV3AuthsPostResponse
    {
        [JsonProperty("auth_request", Required = Required.Always)]
        public Guid AuthRequest { get; set; }

        [JsonProperty("push_package")]
        public string PushPackage { get; set; }

        [JsonProperty("device_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> DeviceIDs { get; set; }
    }
}
