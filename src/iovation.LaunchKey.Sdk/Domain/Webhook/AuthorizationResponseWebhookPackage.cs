using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.Service;

namespace iovation.LaunchKey.Sdk.Domain.Webhook
{
	/// <summary>
	/// A Webhook package containing an authorization response
	/// </summary>
	public class AuthorizationResponseWebhookPackage : IWebhookPackage
	{
		/// <summary>
		/// The authorization response itself
		/// </summary>
		public AuthorizationResponse AuthorizationResponse { get; }

		public AuthorizationResponseWebhookPackage(AuthorizationResponse authorizationResponse)
		{
			AuthorizationResponse = authorizationResponse;
		}
	}
}