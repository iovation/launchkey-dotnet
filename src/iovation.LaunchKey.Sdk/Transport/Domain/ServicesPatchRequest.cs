using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ServicesPatchRequest
    {
        [JsonProperty("service_id")]
        public Guid ServiceId { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("description")]
        public string Description { get; }

        [JsonProperty("icon")]
        public Uri Icon { get; }

        [JsonProperty("callback_url")]
        public Uri CallbackUrl { get; }

        [JsonProperty("active")]
        public bool Active { get; }

        public ServicesPatchRequest(Guid serviceId, string name, string description, Uri icon, Uri callbackUrl, bool active)
        {
            ServiceId = serviceId;
            Name = name;
            Description = description;
            Icon = icon;
            CallbackUrl = callbackUrl;
            Active = active;
        }
    }
}