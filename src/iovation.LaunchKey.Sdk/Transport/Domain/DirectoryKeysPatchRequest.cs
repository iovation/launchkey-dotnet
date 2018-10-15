using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class DirectoryKeysPatchRequest
	{
		[JsonProperty("directory_id")]
		public Guid DirectoryId { get; }

		[JsonProperty("key_id")]
		public string KeyId { get; }

		[JsonProperty("date_expires")]
		public DateTime? Expires { get; }

		[JsonProperty("active")]
		public bool Active { get; }

		public DirectoryKeysPatchRequest(Guid directoryId, string keyId, DateTime? expires, bool active)
		{
			DirectoryId = directoryId;
			KeyId = keyId;
			Expires = expires;
			Active = active;
		}
	}
}