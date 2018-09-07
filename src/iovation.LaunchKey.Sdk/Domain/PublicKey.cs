using System;

namespace iovation.LaunchKey.Sdk.Domain
{
	/// <summary>
	/// Represents a public key on a service or directory
	/// </summary>
	public class PublicKey
	{
		/// <summary>
		/// The unique ID for this public key. Use this when updating/removing the key via API calls.
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// Whether or not the key is currently active
		/// </summary>
		public bool Active { get; }

		/// <summary>
		/// The date and time this key was created
		/// </summary>
		public DateTime Created { get; }

		/// <summary>
		/// The date and time this key will expire
		/// </summary>
		public DateTime Expires { get; }

		public PublicKey(string id, bool active, DateTime created, DateTime expires)
		{
			Id = id;
			Active = active;
			Created = created;
			Expires = expires;
		}
	}
}