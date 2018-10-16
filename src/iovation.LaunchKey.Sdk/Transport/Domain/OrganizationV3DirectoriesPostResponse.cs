using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class OrganizationV3DirectoriesPostResponse
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
	}
}