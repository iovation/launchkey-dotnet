using System;
using System.Collections.Generic;
using System.Linq;
using iovation.LaunchKey.Sdk.Domain.Service;
using iovation.LaunchKey.Sdk.Domain.Webhook;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using AuthPolicy = iovation.LaunchKey.Sdk.Domain.Service.AuthPolicy;
using DenialReason = iovation.LaunchKey.Sdk.Domain.Service.DenialReason;
using TransportDenialReason = iovation.LaunchKey.Sdk.Transport.Domain.DenialReason;

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
            return CreateAuthorizationRequest(user, context, policy).Id;
        }

        public AuthorizationRequest CreateAuthorizationRequest(string user, string context = null, AuthPolicy policy = null, string title = null, int? ttl = null, string pushTitle = null, string pushBody = null, IList<DenialReason> denialReasons = null)
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
            List<TransportDenialReason> transportDenialReasons;
            if (denialReasons == null)
            {
                transportDenialReasons = null;
            }
            else
            {
                transportDenialReasons = new List<TransportDenialReason>();
                foreach (DenialReason dr in denialReasons)
                {
                    transportDenialReasons.Add(new TransportDenialReason(dr.Id, dr.Reason, dr.Fraud));
                }
            }
            var request = new ServiceV3AuthsPostRequest(user, requestPolicy, context, title, ttl, pushTitle, pushBody, transportDenialReasons);
            var response = _transport.ServiceV3AuthsPost(request, _serviceId);
            var authRequest = new AuthorizationRequest(response.AuthRequest.ToString("D"), response.PushPackage);
            return authRequest;
        }

        public void CancelAuthorizationRequest(string authorizationRequestId)
        {
            _transport.ServiceV3AuthsDelete(Guid.Parse(authorizationRequestId), _serviceId);
        }

        public AuthorizationResponse GetAuthorizationResponse(string authorizationRequestId)
        {
            var response = _transport.ServiceV3AuthsGet(Guid.Parse(authorizationRequestId), _serviceId);
            if (response != null)
            {
                AuthorizationResponseType? type;
                if (response.Type == null)
                {
                    type = null;
                }
                else if (response.Type == "AUTHORIZED")
                {
                    type = AuthorizationResponseType.AUTHORIZED;
                }
                else if (response.Type == "DENIED")
                {
                    type = AuthorizationResponseType.DENIED;
                }
                else if (response.Type == "FAILED")
                {
                    type = AuthorizationResponseType.FAILED;
                }
                else
                {
                    type = AuthorizationResponseType.OTHER;
                }

                AuthorizationResponseReason? reason;
                if (response.Reason == null)
                {
                    reason = null;
                }
                else if (response.Reason == "APPROVED")
                {
                    reason = AuthorizationResponseReason.APPROVED;
                }
                else if (response.Reason == "DISAPPROVED")
                {
                    reason = AuthorizationResponseReason.DISAPPROVED;
                }
                else if (response.Reason == "FRAUDULENT")
                {
                    reason = AuthorizationResponseReason.FRAUDULENT;
                }
                else if (response.Reason == "POLICY")
                {
                    reason = AuthorizationResponseReason.POLICY;
                }
                else if (response.Reason == "PERMISSION")
                {
                    reason = AuthorizationResponseReason.PERMISSION;
                }
                else if (response.Reason == "AUTHENTICATION")
                {
                    reason = AuthorizationResponseReason.AUTHENTICATION;
                }
                else if (response.Reason == "CONFIGURATION")
                {
                    reason = AuthorizationResponseReason.CONFIGURATION;
                }
                else if (response.Reason == "BUSY_LOCAL")
                {
                    reason = AuthorizationResponseReason.BUSY_LOCAL;
                }
                else
                {
                    reason = AuthorizationResponseReason.OTHER;
                }

                return new AuthorizationResponse(
                    response.AuthorizationRequestId.ToString("D"),
                    response.Response,
                    response.ServiceUserHash,
                    response.OrganizationUserHash,
                    response.UserPushId,
                    response.DeviceId,
                    new List<string>(response.DevicePins),
                    type,
                    reason,
                    response.DenialReason,
                    reason == AuthorizationResponseReason.FRAUDULENT
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
                AuthorizationResponseType? type;
                if (authEvent.Type == null)
                {
                    type = null;
                }
                else if (authEvent.Type == "AUTHORIZED")
                {
                    type = AuthorizationResponseType.AUTHORIZED;
                }
                else if (authEvent.Type == "DENIED")
                {
                    type = AuthorizationResponseType.DENIED;
                }
                else if (authEvent.Type == "FAILED")
                {
                    type = AuthorizationResponseType.FAILED;
                }
                else
                {
                    type = AuthorizationResponseType.OTHER;
                }

                AuthorizationResponseReason? reason;
                if (authEvent.Reason == null)
                {
                    reason = null;
                }
                else if (authEvent.Reason == "APPROVED")
                {
                    reason = AuthorizationResponseReason.APPROVED;
                }
                else if (authEvent.Reason == "DISAPPROVED")
                {
                    reason = AuthorizationResponseReason.DISAPPROVED;
                }
                else if (authEvent.Reason == "FRAUDULENT")
                {
                    reason = AuthorizationResponseReason.FRAUDULENT;
                }
                else if (authEvent.Reason == "POLICY")
                {
                    reason = AuthorizationResponseReason.POLICY;
                }
                else if (authEvent.Reason == "PERMISSION")
                {
                    reason = AuthorizationResponseReason.PERMISSION;
                }
                else if (authEvent.Reason == "AUTHENTICATION")
                {
                    reason = AuthorizationResponseReason.AUTHENTICATION;
                }
                else if (authEvent.Reason == "CONFIGURATION")
                {
                    reason = AuthorizationResponseReason.CONFIGURATION;
                }
                else if (authEvent.Reason == "BUSY_LOCAL")
                {
                    reason = AuthorizationResponseReason.BUSY_LOCAL;
                }
                else
                {
                    reason = AuthorizationResponseReason.OTHER;
                }
                return new AuthorizationResponseWebhookPackage(
                    new AuthorizationResponse(
                        authEvent.AuthorizationRequestId.ToString("D"),
                        authEvent.Response,
                        authEvent.ServiceUserHash,
                        authEvent.OrganizationUserHash,
                        authEvent.UserPushId,
                        authEvent.DeviceId,
                        authEvent.DevicePins.ToList(),
                        type,
                        reason,
                        authEvent.DenialReason,
                        reason == AuthorizationResponseReason.FRAUDULENT
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