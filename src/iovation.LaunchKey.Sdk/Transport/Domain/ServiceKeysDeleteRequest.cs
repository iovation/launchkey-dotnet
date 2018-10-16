using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServiceKeysDeleteRequest
	{
		[JsonProperty("service_id")]
		public Guid ServiceId { get; }

		[JsonProperty("key_id")]
		public string KeyId { get; }

		public ServiceKeysDeleteRequest(Guid serviceId, string keyId)
		{
			ServiceId = serviceId;
			KeyId = keyId;
		}
	}
}