using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class KeysListPostResponse
    {
        public class Key
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("public_key")]
            public string PublicKey { get; set; }

            [JsonProperty("date_created")]
            public DateTime Created { get; set; }

            [JsonProperty("date_expires")]
            public DateTime? Expires { get; set; }

            [JsonProperty("active")]
            public bool Active { get; set; }

            [JsonProperty("key_type")]
            public int KeyType { get; set; }
        }

        public List<Key> PublicKeys { get; }

        public KeysListPostResponse(List<Key> publicKeys)
        {
            PublicKeys = publicKeys;
        }
    }
}