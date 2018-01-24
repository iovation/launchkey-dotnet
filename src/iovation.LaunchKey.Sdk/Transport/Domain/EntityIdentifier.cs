using System;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class EntityIdentifier
	{
		public Guid Id { get; }
		public EntityType Type { get; }

		public EntityIdentifier(EntityType type, Guid id)
		{
			Type = type;
			Id = id;
		}

		private string GetTypeAsString()
		{
			switch (Type)
			{
				case EntityType.Directory: return "dir";
				case EntityType.Organization: return "org";
				case EntityType.Service: return "svc";
				default: throw new InvalidOperationException("Attempted to convert an unrecognized identifier type");
			}
		}

		public override string ToString()
		{
			return $"{GetTypeAsString()}:{Id:D}";
		}

		public static EntityIdentifier FromString(string idStr)
		{
			var parts = idStr.Split(':');
			if (parts.Length != 2)
				throw new ArgumentException("invalid entity identifier structure");

			EntityType type;
			Guid id;

			if (parts[0] == "org") type = EntityType.Organization;
			else if (parts[0] == "svc") type = EntityType.Service;
			else if (parts[0] == "dir") type = EntityType.Directory;
			else throw new ArgumentException($"Invalid EntityType '{parts[0]}'");

			if (!Guid.TryParse(parts[1], out id))
			{
				throw new ArgumentException($"invalid GUID '{parts[1]}'");
			}

			return new EntityIdentifier(type, id);
		}

		protected bool Equals(EntityIdentifier other)
		{
			return Id.Equals(other.Id) && Type == other.Type;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((EntityIdentifier) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Id.GetHashCode() * 397) ^ (int) Type;
			}
		}
	}
}