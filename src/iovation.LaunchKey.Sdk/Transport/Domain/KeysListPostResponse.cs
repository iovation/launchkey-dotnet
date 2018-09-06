using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class KeysListPostResponse
	{
		public class Key
		{
			[JsonProperty("id")]
			public string Id { get; set; }

			[JsonProperty("public_key")]
			public string PublicKey { get; set; }

			[JsonProperty("created")]
			public DateTime Created { get; set; }

			[JsonProperty("expires")]
			public DateTime Expires { get; set; }
			
			[JsonProperty("active")]
			public bool Active { get; set; }
		}

		public List<Key> PublicKeys { get; }

		public KeysListPostResponse(List<Key> publicKeys)
		{
			PublicKeys = publicKeys;
		}
	}
}