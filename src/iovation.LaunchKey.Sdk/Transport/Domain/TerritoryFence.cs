using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class TerritoryFence : IFence
    {
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("administrative_area")]
        public string AdministrativeArea { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        public TerritoryFence(string country, string name = null,
            string administrativeArea = null, string postalCode = null)
        {
            Name = name;
            Country = country;
            AdministrativeArea = administrativeArea;
            PostalCode = postalCode; 
        }

    }
}