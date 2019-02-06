using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryKeysDeleteRequest
    {
        [JsonProperty("directory_id")]
        public Guid DirectoryId { get; }

        [JsonProperty("key_id")]
        public string KeyId { get; }

        public DirectoryKeysDeleteRequest(Guid directoryId, string keyId)
        {
            DirectoryId = directoryId;
            KeyId = keyId;
        }
    }
}