using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServicesPostRequest
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("icon")]
		public Uri Icon { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("callback_url")]
		public Uri CallbackUrl { get; set; }

		[JsonProperty("active")]
		public bool Active { get; set; }

		public ServicesPostRequest(string name, Uri icon, string description, Uri callbackUrl, bool active)
		{
			Name = name;
			Icon = icon;
			Description = description;
			CallbackUrl = callbackUrl;
			Active = active;
		}
	}
}
