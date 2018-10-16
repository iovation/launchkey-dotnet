using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServicePolicyDeleteRequest
	{
		[JsonProperty("service_id")]
		public Guid ServiceId { get; }

		public ServicePolicyDeleteRequest(Guid serviceId)
		{
			ServiceId = serviceId;
		}
	}
}