using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using iovation.LaunchKey.Sdk.Domain;

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

        public List<PublicKey> FromTransport()
        {
            var keys = new List<PublicKey>();
            foreach (var transportKey in PublicKeys)
            {
                KeyType keyType = KeyType.OTHER;

                KeyType parsedKeyType;
                if (Enum.TryParse(transportKey.KeyType.ToString(), true, out parsedKeyType))
                {
                    keyType = parsedKeyType;
                }

                keys.Add(new PublicKey(
                    transportKey.Id,
                    transportKey.Active,
                    transportKey.Created,
                    transportKey.Expires,
                    keyType
                ));
            }
            return keys;
        }

        public List<Key> PublicKeys { get; }

        public KeysListPostResponse(List<Key> publicKeys)
        {
            PublicKeys = publicKeys;
        }
    }
}