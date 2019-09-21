using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class TransportFence
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("administrative_area", NullValueHandling = NullValueHandling.Ignore)]
        public string AdministrativeArea { get; set; }

        [JsonProperty("postal_code", NullValueHandling = NullValueHandling.Ignore)]
        public string PostalCode { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double? Latitude { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double? Longitude { get; set; }

        [JsonProperty("radius", NullValueHandling = NullValueHandling.Ignore)]
        public double? Radius { get; set; }

        public TransportFence(string name = null, string country = null, string administrativeArea = null, string postalCode = null, string type = null, double? latitude = null, double? longitude = null, double? radius = null)
        {
            Name = name;
            Country = country;
            AdministrativeArea = administrativeArea;
            PostalCode = postalCode;
            Type = type;
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
        }

    }
}
