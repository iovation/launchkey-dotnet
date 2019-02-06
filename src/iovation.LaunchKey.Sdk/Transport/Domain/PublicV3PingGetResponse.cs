using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class PublicV3PingGetResponse
    {
        [JsonProperty("api_time")]
        public DateTime ApiTime { get; set; }
    }
}