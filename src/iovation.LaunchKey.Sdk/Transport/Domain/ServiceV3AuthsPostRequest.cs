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

		[JsonProperty("title")]
		public string Title { get; }

		[JsonProperty("ttl")]
		public int? TTL { get; }

		public ServiceV3AuthsPostRequest(string username, AuthPolicy authPolicy, string context, string title, int? ttl)
		{
			Username = username;
			AuthPolicy = authPolicy;
			Context = context;
			Title = title;
			TTL = ttl;
		}
	}
}