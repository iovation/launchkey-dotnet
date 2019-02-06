using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryV3SessionsListPostRequest
    {
        [JsonProperty("identifier")]
        public string Identifier { get; }

        public DirectoryV3SessionsListPostRequest(string identifier)
        {
            Identifier = identifier;
        }
    }
}