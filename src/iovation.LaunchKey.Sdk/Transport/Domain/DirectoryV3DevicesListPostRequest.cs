using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryV3DevicesListPostRequest
    {
        [JsonProperty("identifier")]
        public string Identifier { get; }

        public DirectoryV3DevicesListPostRequest(string identifier)
        {
            Identifier = identifier;
        }
    }
}