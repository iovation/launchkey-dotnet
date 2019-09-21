using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class GeoCircleFence : IFence
    {
        [JsonProperty("type")]
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
        }

    }
}