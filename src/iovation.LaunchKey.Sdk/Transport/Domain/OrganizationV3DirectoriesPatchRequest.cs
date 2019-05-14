using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class OrganizationV3DirectoriesPatchRequest
    {
        [JsonProperty("directory_id")]
        public Guid DirectoryId { get; }

        [JsonProperty("active")]
        public bool Active { get; }

        [JsonProperty("android_key")]
        public string AndroidKey { get; }

        [JsonProperty("ios_p12")]
        public string IosP12 { get; }

        [JsonProperty("webhook_url")]
        public string WebhookUrl { get; }

        public OrganizationV3DirectoriesPatchRequest(Guid directoryId, bool active, string androidKey, string iosP12, string webhookUrl)
        {
            DirectoryId = directoryId;
            Active = active;
            AndroidKey = androidKey;
            IosP12 = iosP12;
            WebhookUrl = webhookUrl;
        }
    }
}