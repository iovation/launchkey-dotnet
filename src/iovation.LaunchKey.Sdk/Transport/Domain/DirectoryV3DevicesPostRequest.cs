using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryV3DevicesPostRequest
    {
        [JsonProperty("identifier")]
        public string Identifier { get; }

        public DirectoryV3DevicesPostRequest(string identifier)
        {
            Identifier = identifier;
        }
    }
}