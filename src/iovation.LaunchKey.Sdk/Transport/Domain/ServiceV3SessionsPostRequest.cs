using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServiceV3SessionsPostRequest
	{
		[JsonProperty("username")]
		public string Username { get; }

		[JsonProperty("auth_request")]
		public Guid? AuthRequest { get; }

		public ServiceV3SessionsPostRequest(string username, Guid? authRequest)
		{
			Username = username;
			AuthRequest = authRequest;
		}
	}
}