using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.Service;
using iovation.LaunchKey.Sdk.Domain.Webhook;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using AuthPolicy = iovation.LaunchKey.Sdk.Domain.Service.AuthPolicy;

namespace iovation.LaunchKey.Sdk.Client
{
	public class BasicServiceClient : IServiceClient
	{
		private readonly EntityIdentifier _serviceId;
		private readonly ITransport _transport;

		public BasicServiceClient(Guid serviceId, ITransport transport)
		{
			_serviceId = new EntityIdentifier(EntityType.Service, serviceId);
			_transport = transport;
		}

		public string Authorize(string user, string context = null, AuthPolicy policy = null)
		{
			Transport.Domain.AuthPolicy requestPolicy = null;
			if (policy != null)
			{
				requestPolicy = new Transport.Domain.AuthPolicy(
					policy.RequiredFactors,
					policy.RequireKnowledgeFactor,
					policy.RequireInherenceFactor,
					policy.RequirePosessionFactor,
					policy.JailbreakDetection,
					policy.Locations?.Select(
						ploc => new Transport.Domain.AuthPolicy.Location
						{
							Latitude = ploc.Latitude,
							Longitude = ploc.Longitude,
							Radius = ploc.Radius
						}
					).ToList()
				);
			}

			var request = new ServiceV3AuthsPostRequest(user, requestPolicy, context);
			var response = _transport.ServiceV3AuthsPost(request, _serviceId);
			return response.AuthRequest.ToString("D");
		}

		public AuthorizationResponse GetAuthorizationResponse(string authorizationRequestId)
		{
			var response = _transport.ServiceV3AuthsGet(Guid.Parse(authorizationRequestId), _serviceId);
			if (response != null)
			{
				return new AuthorizationResponse(
					response.AuthorizationRequestId.ToString("D"),
					response.Response,
					response.ServiceUserHash,
					response.OrganizationUserHash,
					response.UserPushId,
					response.DeviceId,
					new List<string>(response.DevicePins)
				);
			}

			return null;
		}

		public void SessionStart(string user, string authorizationRequestId)
		{
			var guid = default(Guid?);
			if (!string.IsNullOrWhiteSpace(authorizationRequestId))
				guid = Guid.Parse(authorizationRequestId);

			var request = new ServiceV3SessionsPostRequest(user, guid);
			_transport.ServiceV3SessionsPost(request, _serviceId);
		}

		public void SessionStart(string user)
		{
			SessionStart(user, null);
		}

		public void SessionEnd(string user)
		{
			var request = new ServiceV3SessionsDeleteRequest(user);
			_transport.ServiceV3SessionsDelete(request, _serviceId);
		}

		public IWebhookPackage HandleWebhook(Dictionary<string, List<string>> headers, string body, string method = null, string path = null)
		{
			var serverSentEvent = _transport.HandleServerSentEvent(headers, body, method, path);
			if (serverSentEvent is ServerSentEventAuthorizationResponse)
			{
				var authEvent = (ServerSentEventAuthorizationResponse)serverSentEvent;
				return new AuthorizationResponseWebhookPackage(
					new AuthorizationResponse(
						authEvent.AuthorizationRequestId.ToString("D"),
						authEvent.Response,
						authEvent.ServiceUserHash,
						authEvent.OrganizationUserHash,
						authEvent.UserPushId,
						authEvent.DeviceId,
						authEvent.DevicePins.ToList()
					)
				);
			}

			if (serverSentEvent is ServerSentEventUserServiceSessionEnd)
			{
				var sessionEvent = (ServerSentEventUserServiceSessionEnd)serverSentEvent;
				return new ServiceUserSessionEndWebhookPackage(
					sessionEvent.UserHash,
					sessionEvent.ApiTime
				);
			}

			throw new InvalidRequestException("Unknown response type");
		}
	}
}