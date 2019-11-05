using Newtonsoft.Json;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class TerritoryFence : IFence
    {
        public string Type { get; set; }

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
            Type = "TERRITORY";
        }

        public DomainPolicy.IFence FromTransport()
        {
            return new DomainPolicy.TerritoryFence(
                country: Country,
                administrativeArea: AdministrativeArea,
                postalCode: PostalCode,
                name: Name
            );
        }

    }
}