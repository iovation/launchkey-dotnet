using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryKeysListPostRequest
    {
        [JsonProperty("directory_id")]
        public Guid DirectoryId { get; }

        public DirectoryKeysListPostRequest(Guid directoryId)
        {
            DirectoryId = directoryId;
        }
    }
}