using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryV3SessionsDeleteRequest
    {
        [JsonProperty("identifier")]
        public string Identifier { get; }

        public DirectoryV3SessionsDeleteRequest(string identifier)
        {
            Identifier = identifier;
        }
    }
}