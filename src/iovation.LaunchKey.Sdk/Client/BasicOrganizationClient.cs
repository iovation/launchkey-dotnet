using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iovation.LaunchKey.Sdk.Domain;
using iovation.LaunchKey.Sdk.Domain.Organization;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;

namespace iovation.LaunchKey.Sdk.Client
{
    public class BasicOrganizationClient : ServiceManagingBaseClient, IOrganizationClient
    {
        private EntityIdentifier _organizationId;
        private ITransport _transport;

        public BasicOrganizationClient(Guid organizationId, ITransport transport)
        {
            _organizationId = new EntityIdentifier(EntityType.Organization, organizationId);
            _transport = transport;
        }

        public Guid CreateService(string name, string description, Uri icon, Uri callbackUrl, bool active)
        {
            var request = new ServicesPostRequest(name, description, icon, callbackUrl, active);
            var response = _transport.OrganizationV3ServicesPost(request, _organizationId);
            return response.Id;
        }

        public void UpdateService(Guid serviceId, string name, string description, Uri icon, Uri callbackUrl, bool active)
        {
            var request = new ServicesPatchRequest(serviceId, name, description, icon, callbackUrl, active);
            _transport.OrganizationV3ServicesPatch(request, _organizationId);
        }

        public Service GetService(Guid serviceId)
        {
            return GetServices(new List<Guid> { serviceId }).First();
        }

        public List<Service> GetServices(List<Guid> serviceIds)
        {
            var request = new ServicesListPostRequest(serviceIds);
            var response = _transport.OrganizationV3ServicesListPost(request, _organizationId);
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
            var response = _transport.OrganizationV3ServicesGet(_organizationId);
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

        public Guid CreateDirectory(string name)
        {
            var request = new OrganizationV3DirectoriesPostRequest(name);
            var response = _transport.OrganizationV3DirectoriesPost(request, _organizationId);
            return response.Id;
        }

        public void UpdateDirectory(Guid directoryId, bool active, string androidKey, string iosP12, bool? denialContextInquiryEnabled = null, string webhookUrl = null)
        {
            var request = new OrganizationV3DirectoriesPatchRequest(directoryId, active, androidKey, iosP12, denialContextInquiryEnabled, webhookUrl);
            _transport.OrganizationV3DirectoriesPatch(request, _organizationId);
        }

        public Directory GetDirectory(Guid directoryId)
        {
            return GetDirectories(new List<Guid> { directoryId })[0];
        }

        public List<Directory> GetDirectories(List<Guid> directoryIds)
        {
            var request = new OrganizationV3DirectoriesListPostRequest(directoryIds);
            var response = _transport.OrganizationV3DirectoriesListPost(request, _organizationId);
            var directories = new List<Directory>();

            foreach (OrganizationV3DirectoriesListPostResponse.Directory directoryItem in response.Directories)
            {
                directories.Add(new Directory(
                    directoryItem.Id,
                    directoryItem.Name,
                    directoryItem.Active,
                    directoryItem.ServiceIds,
                    directoryItem.SdkKeys,
                    directoryItem.AndroidKey,
                    directoryItem.IosCertificateFingerprint,
                    directoryItem.DenialContextInquiryEnabled,
                    directoryItem.WebhookUrl
                ));
            }

            return directories;
        }

        public List<Directory> GetAllDirectories()
        {
            var response = _transport.OrganizationV3DirectoriesGet(_organizationId);
            var directories = new List<Directory>();

            foreach (OrganizationV3DirectoriesGetResponse.Directory directoryItem in response.Directories)
            {
                directories.Add(new Directory(
                    directoryItem.Id,
                    directoryItem.Name,
                    directoryItem.Active,
                    directoryItem.ServiceIds,
                    directoryItem.SdkKeys,
                    directoryItem.AndroidKey,
                    directoryItem.IosCertificateFingerprint,
                    directoryItem.DenialContextInquiryEnabled,
                    directoryItem.WebhookUrl
                ));
            }

            return directories;
        }

        public Guid GenerateAndAddDirectorySdkKey(Guid directoryId)
        {
            var response = _transport.OrganizationV3DirectorySdkKeysPost(new OrganizationV3DirectorySdkKeysPostRequest(directoryId), _organizationId);
            return response.SdkKey;
        }

        public void RemoveDirectorySdkKey(Guid directoryId, Guid sdkKey)
        {
            _transport.OrganizationV3DirectorySdkKeysDelete(new OrganizationV3DirectorySdkKeysDeleteRequest(directoryId, sdkKey), _organizationId);
        }

        public List<Guid> GetAllDirectorySdkKeys(Guid directoryId)
        {
            var request = new OrganizationV3DirectorySdkKeysListPostRequest(directoryId);
            var response = _transport.OrganizationV3DirectorySdkKeysListPost(request, _organizationId);
            return response.SdkKeys;
        }

        public List<PublicKey> GetServicePublicKeys(Guid serviceId)
        {
            var request = new ServiceKeysListPostRequest(serviceId);
            var response = _transport.OrganizationV3ServiceKeysListPost(request, _organizationId);
            var keys = new List<PublicKey>();

            foreach (var transportKey in response.PublicKeys)
            {
                keys.Add(new PublicKey(
                    transportKey.Id,
                    transportKey.Active,
                    transportKey.Created,
                    transportKey.Expires
                ));
            }

            return keys;
        }

        public string AddServicePublicKey(Guid serviceId, string publicKeyPem, bool active, DateTime? expires)
        {
            var request = new ServiceKeysPostRequest(
                serviceId,
                publicKeyPem,
                expires?.ToUniversalTime(),
                active
            );
            var response = _transport.OrganizationV3ServiceKeysPost(request, _organizationId);
            return response.Id;
        }

        public void UpdateServicePublicKey(Guid serviceId, string keyId, bool active, DateTime? expires)
        {
            var request = new ServiceKeysPatchRequest(
                serviceId,
                keyId,
                expires?.ToUniversalTime(),
                active
            );
            _transport.OrganizationV3ServiceKeysPatch(request, _organizationId);
        }

        public void RemoveServicePublicKey(Guid serviceId, string keyId)
        {
            var request = new ServiceKeysDeleteRequest(serviceId, keyId);
            _transport.OrganizationV3ServiceKeysDelete(request, _organizationId);
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
            else
            {
                return GetDomainServicePolicyFromDomainLegacyPolicy((DomainPolicy.LegacyPolicy)legacyPolicy);
            }
        }

        [Obsolete("SetServicePolicy is deprecated, please use SetAdvancedServicePolicy instead")]
        public void SetServicePolicy(Guid serviceId, ServicePolicy policy)
        {
            DomainPolicy.IPolicy convertedPolicy = GetDomainPolicyFromTransportPolicy(policy.ToTransport());
            SetAdvancedServicePolicy(serviceId, convertedPolicy);
        }

        public DomainPolicy.IPolicy GetAdvancedServicePolicy(Guid serviceId)
        {
            var request = new ServicePolicyItemPostRequest(serviceId);
            IPolicy response = _transport.OrganizationV3ServicePolicyItemPost(request, _organizationId);
            return GetDomainPolicyFromTransportPolicy(response);
        }

        public void SetAdvancedServicePolicy(Guid serviceId, DomainPolicy.IPolicy policy)
        {
            IPolicy requestPolicy = null;
            if(policy.GetType() == typeof(DomainPolicy.LegacyPolicy))
            {
                DomainPolicy.LegacyPolicy legacyPolicy =
                    (DomainPolicy.LegacyPolicy)policy;
                List<AuthPolicy.Location> locations = GetTransportLocationsFromDomainGeoCircleFences(legacyPolicy.Fences);
                requestPolicy = new AuthPolicy(
                    legacyPolicy.Amount, legacyPolicy.KnowledgeRequired,
                    legacyPolicy.InherenceRequired, legacyPolicy.PossessionRequired,
                    legacyPolicy.DenyRootedJailbroken,locations,
                    legacyPolicy.TimeRestrictions);
            }
            else if(policy.GetType() == typeof(DomainPolicy.ConditionalGeoFencePolicy))
            {
                requestPolicy = GetTransportPolicyFromDomainPolicy(policy);
            }
            else if (policy.GetType() == typeof(DomainPolicy.MethodAmountPolicy))
            {
                requestPolicy = GetTransportPolicyFromDomainPolicy(policy);
            }
            else if (policy.GetType() == typeof(DomainPolicy.FactorsPolicy))
            {
                requestPolicy = GetTransportPolicyFromDomainPolicy(policy);
            }
            else
            {
                throw new InvalidParameters("Policy was not a known policy type");
            }
            var request = new ServicePolicyPutRequest(serviceId, requestPolicy);
            _transport.OrganizationV3ServicePolicyPut(request, _organizationId);
        }

        public void RemoveServicePolicy(Guid serviceId)
        {
            var request = new ServicePolicyDeleteRequest(serviceId);
            _transport.OrganizationV3ServicePolicyDelete(request, _organizationId);
        }

        public List<PublicKey> GetDirectoryPublicKeys(Guid directoryId)
        {
            var request = new DirectoryKeysListPostRequest(directoryId);
            var response = _transport.OrganizationV3DirectoryKeysListPost(request, _organizationId);
            var keys = new List<PublicKey>();

            foreach (var transportKey in response.PublicKeys)
            {
                keys.Add(new PublicKey(
                    transportKey.Id,
                    transportKey.Active,
                    transportKey.Created,
                    transportKey.Expires
                ));
            }

            return keys;
        }

        public string AddDirectoryPublicKey(Guid directoryId, string publicKeyPem, bool active, DateTime? expires)
        {
            var request = new DirectoryKeysPostRequest(
                directoryId,
                publicKeyPem,
                expires?.ToUniversalTime(),
                active
            );
            var response = _transport.OrganizationV3DirectoryKeysPost(request, _organizationId);
            return response.Id;
        }

        public void UpdateDirectoryPublicKey(Guid directoryId, string keyId, bool active, DateTime? expires)
        {
            var request = new DirectoryKeysPatchRequest(
                directoryId,
                keyId,
                expires?.ToUniversalTime(),
                active
            );
            _transport.OrganizationV3DirectoryKeysPatch(request, _organizationId);
        }

        public void RemoveDirectoryPublicKey(Guid directoryId, string keyId)
        {
            var request = new DirectoryKeysDeleteRequest(directoryId, keyId);
            _transport.OrganizationV3DirectoryKeysDelete(request, _organizationId);
        }
    }
}
