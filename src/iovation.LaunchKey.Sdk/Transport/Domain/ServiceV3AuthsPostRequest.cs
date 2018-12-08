using Newtonsoft.Json;
using System.Collections.Generic;

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

		[JsonProperty("push_title")]
		public string PushTitle { get; }

		[JsonProperty("push_body")]
		public string PushBody { get; }

		[JsonProperty("denial_reasons")]
		public IList<DenialReason> DenialReasons { get; }

		public ServiceV3AuthsPostRequest(string username, AuthPolicy authPolicy, string context, string title, int? ttl, string pushTitle, string pushBody, IList<DenialReason> denialReasons)
		{
			Username = username;
			AuthPolicy = authPolicy;
			Context = context;
			Title = title;
			TTL = ttl;
			PushTitle = pushTitle;
			PushBody = pushBody;
			DenialReasons = denialReasons;
		}
	}
}