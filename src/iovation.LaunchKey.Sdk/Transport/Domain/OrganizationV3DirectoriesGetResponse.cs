using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class OrganizationV3DirectoriesGetResponse
    {
        public class Directory
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("active")]
            public bool Active { get; set; }

            [JsonProperty("service_ids")]
            public List<Guid> ServiceIds { get; set; }

            [JsonProperty("sdk_keys")]
            public List<Guid> SdkKeys { get; set; }

            [JsonProperty("android_key")]
            public string AndroidKey { get; set; }

            [JsonProperty("ios_certificate_fingerprint")]
            public string IosCertificateFingerprint { get; set; }
        }

        public List<Directory> Directories { get; }

        public OrganizationV3DirectoriesGetResponse(List<Directory> directories)
        {
            Directories = directories;
        }
    }
}