using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServicesPatchRequest
	{
		[JsonProperty("service_id")]
		public Guid ServiceId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("icon")]
		public Uri Icon { get; set; }

		[JsonProperty("callback_url")]
		public Uri CallbackUrl { get; set; }

		[JsonProperty("active")]
		public bool Active { get; set; }

		public ServicesPatchRequest(Guid serviceId, string name, string description, Uri icon, Uri callbackUrl, bool active)
		{
			ServiceId = serviceId;
			Name = name;
			Description = description;
			Icon = icon;
			CallbackUrl = callbackUrl;
			Active = active;
		}
	}
}