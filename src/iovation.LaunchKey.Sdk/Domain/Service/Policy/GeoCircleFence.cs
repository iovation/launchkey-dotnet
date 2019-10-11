using System;
namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    /// <summary>
    /// Object that represents a Geocircle that can be used in an Authorization Request
    /// </summary>
    public class GeoCircleFence : IFence
    {
        /// <summary>
        /// Name of the Fence
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// The latitude of this Fence
        /// </summary>
        public Double Latitude { get; }

        /// <summary>
        /// The longitude of this Fence
        /// </summary>
        public Double Longitude { get; }

        /// <summary>
        /// The radius of this Fence
        /// </summary>
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
