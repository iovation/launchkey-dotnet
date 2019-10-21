using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iovation.LaunchKey.Sdk.Domain.Service;
using iovation.LaunchKey.Sdk.Domain.Webhook;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using AuthPolicy = iovation.LaunchKey.Sdk.Domain.Service.AuthPolicy;
using DenialReason = iovation.LaunchKey.Sdk.Domain.Service.DenialReason;
using TransportDenialReason = iovation.LaunchKey.Sdk.Transport.Domain.DenialReason;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;

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

        [Obsolete("GetAuthorizationResponse has been deprecated and will be removed in the next major version. Please use GetAdvancedAuthorizationResponse")]
        public AuthorizationResponse GetAuthorizationResponse(string authorizationRequestId)
        {
            AdvancedAuthorizationResponse advancedAuthResponse = GetAdvancedAuthorizationResponse(authorizationRequestId);

            if(advancedAuthResponse != null)
            {
                AuthPolicy authPolicy = GetAuthPolicyFromAuthResponsePolicy(advancedAuthResponse.Policy);

                return new AuthorizationResponse(
                    authorizationRequestId: advancedAuthResponse.AuthorizationRequestId,
                    authorized: advancedAuthResponse.Authorized,
                    serviceUserHash: advancedAuthResponse.ServiceUserHash,
                    organizationUserHash: advancedAuthResponse.OrganizationUserHash,
                    userPushId: advancedAuthResponse.UserPushId,
                    deviceId: advancedAuthResponse.DeviceId,
                    devicePins: advancedAuthResponse.DevicePins,
                    type: advancedAuthResponse.Type,
                    reason: advancedAuthResponse.Reason,
                    denialReason: advancedAuthResponse.DenialReason,
                    fraud: advancedAuthResponse.Fraud,
                    authPolicy: authPolicy,
                    authMethods: advancedAuthResponse.AuthMethods
                );
            }
            else
            {
                return null;
            }
        }

        private AdvancedAuthorizationResponse ParseAuthsGetToAdvancedAuthorizationPackage(ServiceV3AuthsGetResponse response)
        {
            AuthorizationResponseType? type;
            switch (response.Type)
            {
                case null:
                    type = null;
                    break;
                case "AUTHORIZED":
                    type = AuthorizationResponseType.AUTHORIZED;
                    break;
                case "DENIED":
                    type = AuthorizationResponseType.DENIED;
                    break;
                case "FAILED":
                    type = AuthorizationResponseType.FAILED;
                    break;
                default:
                    type = AuthorizationResponseType.OTHER;
                    break;
            }

            AuthorizationResponseReason? reason;
            switch (response.Reason)
            {
                case null:
                    reason = null;
                    break;
                case "APPROVED":
                    reason = AuthorizationResponseReason.APPROVED;
                    break;
                case "DISAPPROVED":
                    reason = AuthorizationResponseReason.DISAPPROVED;
                    break;
                case "FRAUDULENT":
                    reason = AuthorizationResponseReason.FRAUDULENT;
                    break;
                case "POLICY":
                    reason = AuthorizationResponseReason.POLICY;
                    break;
                case "PERMISSION":
                    reason = AuthorizationResponseReason.PERMISSION;
                    break;
                case "AUTHENTICATION":
                    reason = AuthorizationResponseReason.AUTHENTICATION;
                    break;
                case "CONFIGURATION":
                    reason = AuthorizationResponseReason.CONFIGURATION;
                    break;
                case "BUSY_LOCAL":
                    reason = AuthorizationResponseReason.BUSY_LOCAL;
                    break;
                case "SENSOR":
                    reason = AuthorizationResponseReason.SENSOR;
                    break;
                default:
                    reason = AuthorizationResponseReason.OTHER;
                    break;
            }

            AuthorizationResponsePolicy authResponse = null;

            if (response.AuthPolicy != null)
            {
                List<DomainPolicy.IFence> fences = ServiceManagingBaseClient.GetDomainFencesFromTransportFences(response.AuthPolicy.Geofences);

                bool? knowledgeRequired = null;
                bool? possessionRequired = null;
                bool? inherenceRequired = null;

                if (response.AuthPolicy.Types != null && response.AuthPolicy.Types.Count > 0)
                {
                    knowledgeRequired = response.AuthPolicy.Types.Contains("KNOWLEDGE", StringComparer.OrdinalIgnoreCase);
                    inherenceRequired = response.AuthPolicy.Types.Contains("INHERENCE", StringComparer.OrdinalIgnoreCase);
                    possessionRequired = response.AuthPolicy.Types.Contains("POSSESSION", StringComparer.OrdinalIgnoreCase);
                }

                Requirement? requirement = null;

                if (response.AuthPolicy.Requirement != null)
                {
                    Requirement parsedRequirement;
                    if (Enum.TryParse(response.AuthPolicy.Requirement, true, out parsedRequirement))
                    {
                        requirement = parsedRequirement;
                    }
                    else
                    {
                        requirement = Requirement.OTHER;
                    }
                }

                authResponse = new AuthorizationResponsePolicy(
                    requirement: requirement,
                    amount: response.AuthPolicy.Amount,
                    fences: fences,
                    knowledgeRequired: knowledgeRequired,
                    inherenceRequired: inherenceRequired,
                    possessionRequired: possessionRequired
                );
            }


            List<AuthMethod> authMethods = GetAuthMethods(response);


            return new AdvancedAuthorizationResponse(
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
                reason == AuthorizationResponseReason.FRAUDULENT,
                authResponse,
                authMethods
            );
        }

        public AdvancedAuthorizationResponse GetAdvancedAuthorizationResponse(string authorizationRequestId)
        {
            ServiceV3AuthsGetResponse response = _transport.ServiceV3AuthsGet(Guid.Parse(authorizationRequestId), _serviceId);
            if (response != null)
            {
                return ParseAuthsGetToAdvancedAuthorizationPackage(response);
            }
            else
            {
                return null;
            }
        }

        private AuthPolicy GetAuthPolicyFromAuthResponsePolicy(AuthorizationResponsePolicy authResponsePolicy)
        {
            AuthPolicy authPolicy = new AuthPolicy();
            List<Location> authPolicyLocations = new List<Location>();

            if (authResponsePolicy != null)
            {
                if (authResponsePolicy.Fences != null && authResponsePolicy.Fences.Any())
                {
                    foreach (DomainPolicy.IFence fence in authResponsePolicy.Fences)
                    {
                        if(fence.GetType() == typeof(DomainPolicy.GeoCircleFence))
                        {
                            authPolicyLocations.Add(
                                new Location(
                                    (fence as DomainPolicy.GeoCircleFence).Radius,
                                    (fence as DomainPolicy.GeoCircleFence).Latitude,
                                    (fence as DomainPolicy.GeoCircleFence).Longitude,
                                    (fence as DomainPolicy.GeoCircleFence)?.Name
                                )
                            );
                        }
                        else
                        {
                            Trace.TraceWarning($"A Fence besides GeoCircleFence was present while using legacy functionality. This fence has been skipped from being processed.");
                        }
                    }
                }

                if (authResponsePolicy.Requirement == Requirement.AMOUNT)
                {
                    authPolicy = new AuthPolicy(
                        authResponsePolicy.Amount,
                        null,
                        null,
                        null,
                        null,
                        authPolicyLocations
                    );
                }
                else if(authResponsePolicy.Requirement == Requirement.TYPES)
                {
                    bool? requiredKnowledge = authResponsePolicy.KnowledgeRequired;
                    bool? requiredInherence = authResponsePolicy.InherenceRequired;
                    bool? requiredPosession = authResponsePolicy.PossessionRequired;

                    authPolicy = new AuthPolicy(
                        null,
                        requiredKnowledge,
                        requiredInherence,
                        requiredPosession,
                        null,
                        authPolicyLocations
                    );
                }
                else if(authResponsePolicy.Requirement == Requirement.COND_GEO)
                {
                    Trace.TraceWarning($"Conditional Geofence cannot be converted to the legacy policy. To utilize new policies please use HandleAdvancedWebhook");
                    return null;
                }
                else
                {
                    authPolicy = new AuthPolicy(
                        null,
                        null,
                        null,
                        null,
                        null,
                        authPolicyLocations
                    );
                }
            } 
            else
            {
                authPolicy = null;
            }

            return authPolicy;

        }

        private List<AuthMethod> GetAuthMethods(ServiceV3AuthsGetResponse authResponse)
        {
            var authMethods = new List<AuthMethod>();

            if (authResponse.AuthMethods != null)
            {

                foreach (var transportMethod in authResponse.AuthMethods)
                {
                    string methodType = transportMethod.Method.ToUpper();
                    AuthMethodType authMethodType;

                    if (methodType == "PIN_CODE")
                    {
                        authMethodType = AuthMethodType.PIN_CODE;
                    }
                    else if (methodType == "CIRCLE_CODE")
                    {
                        authMethodType = AuthMethodType.CIRCLE_CODE;
                    }
                    else if (methodType == "GEOFENCING")
                    {
                        authMethodType = AuthMethodType.GEOFENCING;
                    }
                    else if (methodType == "LOCATIONS")
                    {
                        authMethodType = AuthMethodType.LOCATIONS;
                    }
                    else if (methodType == "WEARABLES")
                    {
                        authMethodType = AuthMethodType.WEARABLES;
                    }
                    else if (methodType == "FINGERPRINT")
                    {
                        authMethodType = AuthMethodType.FINGERPRINT;
                    }
                    else if (methodType == "FACE")
                    {
                        authMethodType = AuthMethodType.FACE;
                    }
                    else
                    {
                        authMethodType = AuthMethodType.OTHER;
                    }

                    authMethods.Add(
                        new AuthMethod
                        (
                            authMethodType,
                            transportMethod.Set,
                            transportMethod.Active,
                            transportMethod.Allowed,
                            transportMethod.Supported,
                            transportMethod.UserRequired,
                            transportMethod.Passed,
                            transportMethod.Error
                        )
                    );
                        
                }
            }
            else
            {
                authMethods = null;
            }

            return authMethods;
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

        [Obsolete("HandleWebhook(headers, body) is obsolete. Please use HandleWebhook(headers, body, method, path)", false)]
        public IWebhookPackage HandleWebhook(Dictionary<string, List<string>> headers, string body)
        {
            return HandleWebhook(headers, body, null, null);
        }

        public IWebhookPackage HandleWebhook(Dictionary<string, List<string>> headers, string body, string method = null, string path = null)
        {
            var webhookPackage = HandleAdvancedWebhook(headers, body, method, path);
            if(webhookPackage is ServiceUserSessionEndWebhookPackage)
            {
                return webhookPackage;
            }
            else if(webhookPackage is AdvancedAuthorizationResponseWebhookPackage)
            {
                AdvancedAuthorizationResponse advancedAuthorizationResponse = ((AdvancedAuthorizationResponseWebhookPackage)webhookPackage).AdvancedAuthorizationResponse;
                AuthPolicy authPolicy = GetAuthPolicyFromAuthResponsePolicy(advancedAuthorizationResponse.Policy);

                return new AuthorizationResponseWebhookPackage(
                    new AuthorizationResponse(
                        authorizationRequestId: advancedAuthorizationResponse.AuthorizationRequestId,
                        authorized: advancedAuthorizationResponse.Authorized,
                        serviceUserHash: advancedAuthorizationResponse.ServiceUserHash,
                        organizationUserHash: advancedAuthorizationResponse.OrganizationUserHash,
                        userPushId: advancedAuthorizationResponse.UserPushId,
                        deviceId: advancedAuthorizationResponse.DeviceId,
                        devicePins: advancedAuthorizationResponse.DevicePins,
                        type: advancedAuthorizationResponse.Type,
                        reason: advancedAuthorizationResponse.Reason,
                        denialReason: advancedAuthorizationResponse.DenialReason,
                        fraud: advancedAuthorizationResponse.Fraud,
                        authPolicy: authPolicy,
                        authMethods: advancedAuthorizationResponse.AuthMethods
                    )
                );
            }

            throw new InvalidRequestException("Unknown response type");
        }

        public IWebhookPackage HandleAdvancedWebhook(Dictionary<string, List<string>> headers, string body, string method = null, string path = null)
        {
            IServerSentEvent serverSentEvent = _transport.HandleServerSentEvent(headers, body, method, path);
            if (serverSentEvent is ServerSentEventAuthorizationResponse)
            {
                ServerSentEventAuthorizationResponse authEvent = (ServerSentEventAuthorizationResponse)serverSentEvent;

                AdvancedAuthorizationResponse advancedAuthorizationResponse = ParseAuthsGetToAdvancedAuthorizationPackage(authEvent);

                return new AdvancedAuthorizationResponseWebhookPackage(
                    advancedAuthorizationResponse
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