using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServiceV3AuthsPostRequest
	{
		[JsonProperty("username")]
		public string Username { get; }

		[JsonProperty("policy")]
		public AuthPolicy AuthPolicy { get; }

		[JsonProperty("context")]
		public string Context { get; }

		public ServiceV3AuthsPostRequest(string username, AuthPolicy authPolicy, string context)
		{
			Username = username;
			AuthPolicy = authPolicy;
			Context = context;
		}
	}
}