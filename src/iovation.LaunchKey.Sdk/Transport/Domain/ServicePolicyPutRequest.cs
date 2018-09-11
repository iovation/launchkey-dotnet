using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServicePolicyPutRequest
	{
		[JsonProperty("service_id")]
		public Guid ServiceId { get; }

		[JsonProperty("policy")]
		public AuthPolicy AuthPolicy { get; }

		public ServicePolicyPutRequest(Guid serviceId, AuthPolicy authPolicy)
		{
			ServiceId = serviceId;
			AuthPolicy = authPolicy;
		}
	}
}