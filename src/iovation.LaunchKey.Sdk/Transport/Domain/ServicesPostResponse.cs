using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServicesPostResponse
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
	}
}