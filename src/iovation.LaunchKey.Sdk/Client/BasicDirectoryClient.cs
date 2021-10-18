using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iovation.LaunchKey.Sdk.Domain;
using iovation.LaunchKey.Sdk.Domain.Directory;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Domain.Webhook;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;

namespace iovation.LaunchKey.Sdk.Client
{
    public class BasicDirectoryClient : IDirectoryClient
    {
        private readonly EntityIdentifier _directoryId;
        private readonly ITransport _transport;

        public BasicDirectoryClient(Guid directoryId, ITransport transport)
        {
            _directoryId = new EntityIdentifier(EntityType.Directory, directoryId);
            _transport = transport;
        }

        public DirectoryUserDeviceLinkData LinkDevice(string userId, int? ttl = null)
        {
            var request = new DirectoryV3DevicesPostRequest(userId, ttl);
            var response = _transport.DirectoryV3DevicesPost(request, _directoryId);

            Guid? deviceID = Guid.TryParse(response.DeviceId, out Guid deviceGUID) ? (Guid?)deviceGUID : null;
            return new DirectoryUserDeviceLinkData(response.Code, response.QrCode, deviceID);
        }

        public List<Device> GetLinkedDevices(string userId)
        {
            var request = new DirectoryV3DevicesListPostRequest(userId);
            var response = _transport.DirectoryV3DevicesListPost(request, _directoryId);
            var devices = new List<Device>();
            foreach (var responseDevice in response.Devices)
            {
                devices.Add(
                    new Device(
                        responseDevice.Id.ToString("D"),
                        responseDevice.Name,
                        DeviceStatus.FromCode(responseDevice.Status),
                        responseDevice.Type,
                        responseDevice.Created,
                        responseDevice.Updated
                    )
                );
            }

            return devices;
        }

        public void UnlinkDevice(string userId, string deviceId)
        {
            Guid deviceGuid;
            if (!Guid.TryParse(deviceId, out deviceGuid))
                throw new ArgumentException("Invalid device ID format.");
            var request = new DirectoryV3DevicesDeleteRequest(userId, deviceGuid);
            _transport.DirectoryV3DevicesDelete(request, _directoryId);
        }

        public List<Session> GetAllServiceSessions(string userId)
        {
            var request = new DirectoryV3SessionsListPostRequest(userId);
            var response = _transport.DirectoryV3SessionsListPost(request, _directoryId);
            var sessions = new List<Session>();
            foreach (var responseSession in response.Sessions)
            {
                sessions.Add(
                    new Session(
                        responseSession.ServiceId,
                        responseSession.ServiceName,
                        responseSession.ServiceIcon,
                        responseSession.AuthRequest,
                        responseSession.Created
                    )
                );
            }

            return sessions;
        }

        public void EndAllServiceSessions(string userId)
        {
            var request = new DirectoryV3SessionsDeleteRequest(userId);
            _transport.DirectoryV3SessionsDelete(request, _directoryId);
        }

        public Guid CreateService(string name, string description, Uri icon, Uri callbackUrl, bool active)
        {
            var request = new ServicesPostRequest(name, description, icon, callbackUrl, active);
            var response = _transport.DirectoryV3ServicesPost(request, _directoryId);
            return response.Id;
        }

        public void UpdateService(Guid serviceId, string name, string description, Uri icon, Uri callbackUrl, bool active)
        {
            var request = new ServicesPatchRequest(serviceId, name, description, icon, callbackUrl, active);
            _transport.DirectoryV3ServicesPatch(request, _directoryId);
        }

        public Service GetService(Guid serviceId)
        {
            return GetServices(new List<Guid> { serviceId }).First();
        }

        public List<Service> GetServices(List<Guid> serviceIds)
        {
            var request = new ServicesListPostRequest(serviceIds);
            var response = _transport.DirectoryV3ServicesListPost(request, _directoryId);
            var services = new List<Service>();

            foreach (var serviceItem in response.Services)
            {
                services.Add(
                    new Service(
                        serviceItem.Id,
                        serviceItem.Name,
                        serviceItem.Description,
                        serviceItem.Icon,
                        serviceItem.CallbackUrl,
                        serviceItem.Active
                    ));
            }

            return services;
        }

        public List<Service> GetAllServices()
        {
            var response = _transport.DirectoryV3ServicesGet(_directoryId);
            var services = new List<Service>();
            foreach (var serviceItem in response.Services)
            {
                services.Add(
                    new Service(
                        serviceItem.Id,
                        serviceItem.Name,
                        serviceItem.Description,
                        serviceItem.Icon,
                        serviceItem.CallbackUrl,
                        serviceItem.Active
                    )
                );
            }

            return services;
        }

        public List<PublicKey> GetServicePublicKeys(Guid serviceId)
        {
            var request = new ServiceKeysListPostRequest(serviceId);
            var response = _transport.DirectoryV3ServiceKeysListPost(request, _directoryId);
            var keys = new List<PublicKey>();

            foreach (var transportKey in response.PublicKeys)
            {
                KeyType keyType = KeyType.OTHER;

                KeyType parsedKeyType;
                if (Enum.TryParse(transportKey.KeyType.ToString(), true, out parsedKeyType))
                {
                    keyType = parsedKeyType;
                }

                keys.Add(new PublicKey(
                    transportKey.Id,
                    transportKey.Active,
                    transportKey.Created,
                    transportKey.Expires,
                    keyType
                ));
            }

            return keys;
        }

        public string AddServicePublicKey(Guid serviceId, string publicKeyPem, bool active, DateTime? expires, KeyType keyType = KeyType.BOTH)
        {
            var request = new ServiceKeysPostRequest(
                serviceId,
                publicKeyPem,
                expires?.ToUniversalTime(),
                active,
                (int)keyType
            );
            var response = _transport.DirectoryV3ServiceKeysPost(request, _directoryId);
            return response.Id;
        }

        public string AddServicePublicKey(Guid serviceId, string publicKeyPem, bool active, DateTime? expires)
        {
            return AddServicePublicKey(serviceId, publicKeyPem, active, expires, KeyType.BOTH);
        }

        public void UpdateServicePublicKey(Guid serviceId, string keyId, bool active, DateTime? expires)
        {
            var request = new ServiceKeysPatchRequest(
                serviceId,
                keyId,
                expires?.ToUniversalTime(),
                active
            );
            _transport.DirectoryV3ServiceKeysPatch(request, _directoryId);
        }

        public void RemoveServicePublicKey(Guid serviceId, string keyId)
        {
            var request = new ServiceKeysDeleteRequest(serviceId, keyId);
            _transport.DirectoryV3ServiceKeysDelete(request, _directoryId);
        }


        [Obsolete("GetServicePolicy is deprecated, please use GetAdvancedServicePolicy instead")]
        public ServicePolicy GetServicePolicy(Guid serviceId)
        {
            DomainPolicy.IPolicy legacyPolicy = GetAdvancedServicePolicy(serviceId);

            if (legacyPolicy.GetType() != typeof(DomainPolicy.LegacyPolicy))
            {
                Trace.TraceWarning($"Invalid policy type returned to legacy function. To utilize new policies please use GetAdvancedServicePolicy");
                return null;
            }

            // This calls ToTransport because the parsing logic that is contained in the ServicePolicy class shouldn't be duplicated
            return ServicePolicy.FromTransport((AuthPolicy)legacyPolicy.ToTransport());
        }

        [Obsolete("SetServicePolicy is deprecated, please use SetAdvancedServicePolicy instead")]
        public void SetServicePolicy(Guid serviceId, ServicePolicy policy)
        {
            SetAdvancedServicePolicy(serviceId, policy.ToLegacyPolicy());
        }

        public DomainPolicy.IPolicy GetAdvancedServicePolicy(Guid serviceId)
        {
            var request = new ServicePolicyItemPostRequest(serviceId);
            IPolicy response = _transport.DirectoryV3ServicePolicyItemPost(request, _directoryId);
            return response.FromTransport();
        }

        public void SetAdvancedServicePolicy(Guid serviceId, DomainPolicy.IPolicy policy)
        {
            var request = new ServicePolicyPutRequest(serviceId, policy.ToTransport());
            _transport.DirectoryV3ServicePolicyPut(request, _directoryId);
        }

        public void RemoveServicePolicy(Guid serviceId)
        {
            var request = new ServicePolicyDeleteRequest(serviceId);
            _transport.DirectoryV3ServicePolicyDelete(request, _directoryId);
        }

        [Obsolete("HandleWebhook(headers,body) is obsolete. Please use HandleWebhook(headers, body, method, path)", false)]
        public IWebhookPackage HandleWebhook(Dictionary<string, List<string>> headers, string body)
        {
            return HandleWebhook(headers, body, null, null);
        }

        public IWebhookPackage HandleWebhook(
            Dictionary<string, List<string>> headers, string body, 
            string method, string path
        )
        {
            IServerSentEvent serverSentEvent = _transport.HandleServerSentEvent(headers, body, method, path);
            if (serverSentEvent is ServerSentEventDeviceLinked)
            {
                var deviceLinkTransport = ((ServerSentEventDeviceLinked)serverSentEvent).DeviceLinkCompletion;
                var deviceLinkCompletion = new Domain.Directory.DeviceLinkCompletionResponse(
                    deviceLinkTransport.DeviceId, deviceLinkTransport.DevicePublicKey, deviceLinkTransport.DevicePublicKeyId
                );

                return new DirectoryUserDeviceLinkCompletionWebhookPackage(
                    deviceLinkCompletion
                );
            }

            throw new InvalidRequestException("Unknown response type");
        }

        public IWebhookPackage HandleAdvancedWebhook(
            Dictionary<string, List<string>> headers, string body,
            string method, string path)
        {
            return HandleWebhook(headers, body, method, path);
        }

    }
}