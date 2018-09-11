namespace iovation.LaunchKey.Sdk.Domain.ServiceManager
{
	public class Location
	{
		/// <summary>
		/// The name of this location
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The latitude of the location
		/// </summary>
		public double Latitude { get; }

		/// <summary>
		/// The longitude of the location
		/// </summary>
		public double Longitude { get; }

		/// <summary>
		/// The radius around the latitude and longitude, representing the physical space of this location
		/// </summary>
		public double Radius { get; }

		/// <summary>
		/// Create a new location for a geofence 
		/// </summary>
		/// <param name="name">The name of the location used in the geofence</param>
		/// <param name="radius">The radius around the latitude and longitude, representing the physical space of this location</param>
		/// <param name="latitude">The latitude of the location</param>
		/// <param name="longitude">The longitude of the location</param>
		public Location(string name, double radius, double latitude, double longitude)
		{
			Name = name;
			Radius = radius;
			Longitude = longitude;
			Latitude = latitude;
		}

		protected bool Equals(Location other)
		{
			return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude) && Radius.Equals(other.Radius);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Location) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Latitude.GetHashCode();
				hashCode = (hashCode * 397) ^ Longitude.GetHashCode();
				hashCode = (hashCode * 397) ^ Radius.GetHashCode();
				return hashCode;
			}
		}
	}
}