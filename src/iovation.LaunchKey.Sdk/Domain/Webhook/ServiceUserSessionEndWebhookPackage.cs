using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Domain.Webhook
{
	/// <summary>
	/// A webhook package indicating the user ended a session
	/// </summary>
	public class ServiceUserSessionEndWebhookPackage : IWebhookPackage
	{
		/// <summary>
		/// Unique user identifier that will match the user hash in original authentication response
		/// </summary>
		public string ServiceUserHash { get; }

		/// <summary>
		/// The date and time the remote logout was requested. This value is to be used to ensure the user has not started
		/// a new session since the logout was request and inadvertently ending their application session.
		/// </summary>
		public DateTime LogoutRequested { get; }

		public ServiceUserSessionEndWebhookPackage(string serviceUserHash, DateTime logoutRequested)
		{
			ServiceUserHash = serviceUserHash;
			LogoutRequested = logoutRequested;
		}
	}
}