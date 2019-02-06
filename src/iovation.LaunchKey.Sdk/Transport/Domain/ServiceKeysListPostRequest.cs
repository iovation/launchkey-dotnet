using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ServiceKeysListPostRequest
    {
        [JsonProperty("service_id")]
        public Guid ServiceId { get; }

        public ServiceKeysListPostRequest(Guid serviceId)
        {
            ServiceId = serviceId;
        }
    }
}