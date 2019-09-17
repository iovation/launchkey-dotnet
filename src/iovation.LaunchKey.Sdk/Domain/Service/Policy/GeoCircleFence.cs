using System;
namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    public class GeoCircleFence : IFence
    {
        public String Name { get; }
        public Double Latitude { get; }
        public Double Longitude { get; }
        public Double Radius { get; }

        public GeoCircleFence(
            Double latitude,
            Double longitude,
            Double radius,
            string name=null)
        {
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
            Name = name;
        }
    }
}
