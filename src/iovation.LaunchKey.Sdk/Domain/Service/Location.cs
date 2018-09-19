namespace iovation.LaunchKey.Sdk.Domain.Service
{
	public class Location
	{
		public double Latitude { get; }
		public double Longitude { get; }
		public double Radius { get; }

		public Location(double radius, double latitude, double longitude)
		{
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
			return Equals((Location)obj);
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