using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class OrganizationV3DirectoriesPostRequest
    {
        [JsonProperty("name")]
        public string Name { get; }

        public OrganizationV3DirectoriesPostRequest(string name)
        {
            Name = name;
        }
    }
}