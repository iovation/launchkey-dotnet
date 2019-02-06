using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ServiceKeysPatchRequest
    {
        [JsonProperty("service_id")]
        public Guid ServiceId { get; }

        [JsonProperty("key_id")]
        public string KeyId { get; }

        [JsonProperty("date_expires")]
        public DateTime? Expires { get; }

        [JsonProperty("active")]
        public bool Active { get; }

        public ServiceKeysPatchRequest(Guid serviceId, string keyId, DateTime? expires, bool active)
        {
            ServiceId = serviceId;
            KeyId = keyId;
            Expires = expires;
            Active = active;
        }
    }
}