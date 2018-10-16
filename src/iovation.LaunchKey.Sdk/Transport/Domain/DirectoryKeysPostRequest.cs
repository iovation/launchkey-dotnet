using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class DirectoryKeysPostRequest
	{
		[JsonProperty("directory_id")]
		public Guid DirectoryId { get; }

		[JsonProperty("public_key")]
		public string PublicKey { get; }

		[JsonProperty("date_expires")]
		public DateTime? Expires { get; }

		[JsonProperty("active")]
		public bool Active { get; }

		public DirectoryKeysPostRequest(Guid directoryId, string publicKey, DateTime? expires, bool active)
		{
			DirectoryId = directoryId;
			PublicKey = publicKey;
			Expires = expires;
			Active = active;
		}
	}
}