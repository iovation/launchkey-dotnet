using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServiceKeysPostRequest
	{
		[JsonProperty("service_id")]
		public Guid ServiceId { get; }

		[JsonProperty("public_key")]
		public string PublicKey { get; }

		[JsonProperty("date_expires")]
		public DateTime? Expires { get; }

		[JsonProperty("active")]
		public bool Active { get; }

		public ServiceKeysPostRequest(Guid serviceId, string publicKey, DateTime? expires, bool active)
		{
			ServiceId = serviceId;
			PublicKey = publicKey;
			Expires = expires;
			Active = active;
		}
	}
}