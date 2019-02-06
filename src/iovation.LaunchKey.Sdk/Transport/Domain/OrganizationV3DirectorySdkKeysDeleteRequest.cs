using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class OrganizationV3DirectorySdkKeysDeleteRequest
    {
        [JsonProperty("directory_id")]
        public Guid DirectoryId { get; }

        [JsonProperty("sdk_key")]
        public Guid SdkKey { get; }

        public OrganizationV3DirectorySdkKeysDeleteRequest(Guid directoryId, Guid sdkKey)
        {
            DirectoryId = directoryId;
            SdkKey = sdkKey;
        }
    }
}