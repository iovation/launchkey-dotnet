using System.Collections.Generic;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryV3DevicesPostRequest
    {
        [JsonProperty("identifier")]
        public string Identifier { get; }

        [JsonProperty("ttl")]
        public int? TTL { get; }

        public DirectoryV3DevicesPostRequest(string identifier, int? ttl = null)
        {
            Identifier = identifier;
            TTL = ttl;
        }

        public bool Equals(DirectoryV3DevicesPostRequest other)
        {
            return other != null &&
                   Identifier == other.Identifier &&
                   TTL == other.TTL;
        }

        public override bool Equals(object obj)
        {
            var item = obj as DirectoryV3DevicesPostRequest;
            return Equals(item);
        }

        public override int GetHashCode()
        {
            var hashCode = 397;
            hashCode = hashCode * 397 + EqualityComparer<string>.Default.GetHashCode(Identifier);
            hashCode = hashCode * 397 + EqualityComparer<int?>.Default.GetHashCode(TTL);
            return hashCode;
        }

        public override string ToString()
        {
            return $"DirectoryV3DevicesPostRequest{{Identifier:\"{Identifier}\",TTL:{TTL}}}";
        }
    }
}