using Newtonsoft.Json;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class GeoCircleFence : IFence
    {
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("radius")]
        public double Radius { get; set; }

        public GeoCircleFence(string name, double latitude, double longitude,
            double radius)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude; 
            Radius = radius;
            Type = "GEO_CIRCLE";
        }

        public DomainPolicy.IFence FromTransport()
        {
            return new DomainPolicy.GeoCircleFence(
                latitude: Latitude,
                longitude: Longitude,
                radius: Radius,
                name: Name
            );
        }
    }
}