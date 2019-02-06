using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class OrganizationV3DirectorySdkKeysListPostRequest
    {
        [JsonProperty("directory_id")]
        public Guid DirectoryId { get; }

        public OrganizationV3DirectorySdkKeysListPostRequest(Guid directoryId)
        {
            DirectoryId = directoryId;
        }
    }
}