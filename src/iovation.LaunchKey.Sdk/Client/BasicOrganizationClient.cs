using System;
using System.Collections.Generic;
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
    public class BasicOrganizationClient : IOrganizationClient
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

            if(legacyPolicy.GetType() != typeof(LegacyPolicy))
            {
                return null;
            }
            else
            {
                DomainPolicy.LegacyPolicy convertedLegacyPolicy = (DomainPolicy.LegacyPolicy)legacyPolicy;
                List<AuthPolicy.Location> convertedLocations = null;

                // You can only convert GeoCircleFences
                foreach(var newFence in convertedLegacyPolicy.Fences.OfType<DomainPolicy.GeoCircleFence>())
                {
                    convertedLocations.Add(
                        new AuthPolicy.Location(
                            newFence.Name,
                            newFence.Radius,
                            newFence.Latitude,
                            newFence.Longitude
                        )
                    );
                }

                AuthPolicy convertedPolicy = new AuthPolicy(
                    convertedLegacyPolicy.Amount,
                    convertedLegacyPolicy.KnowledgeRequired,
                    convertedLegacyPolicy.InherenceRequired,
                    convertedLegacyPolicy.PossessionRequired,
                    convertedLegacyPolicy.DenyRootedJailbroken,
                    convertedLocations,
                    convertedLegacyPolicy.TimeRestrictions
                    );
                return ServicePolicy.FromTransport(convertedPolicy);
            }
        }

        [Obsolete("SetServicePolicy is deprecated, please use SetAdvancedServicePolicy instead")]
        public void SetServicePolicy(Guid serviceId, ServicePolicy policy)
        {
            DomainPolicy.IPolicy testPolicy = ParseTransportPolicyToClientPolicy(policy.ToTransport());
            SetAdvancedServicePolicy(serviceId, testPolicy);
        }

        public DomainPolicy.IPolicy GetAdvancedServicePolicy(Guid serviceId)
        {
            var request = new ServicePolicyItemPostRequest(serviceId);
            var response = _transport.DirectoryV3ServicePolicyItemPost(request, _organizationId);
            /* Return a fancy client object instead of the transport object here */

            if(response.Type == "LEGACY")
            {
                return ParseTransportPolicyToClientPolicy(response);
            }
            else if (response.Type == "COND_GEO")
            {
                ConditionalGeoFencePolicy convertedCondGeo = (ConditionalGeoFencePolicy)response;
                return ParseTransportPolicyToClientPolicy(convertedCondGeo);
            }
            else if (response.Type == "FACTORS")
            {
                FactorsPolicy convertedFactorsPolicy = (FactorsPolicy)response;
                return ParseTransportPolicyToClientPolicy(convertedFactorsPolicy);
            }
            else if (response.Type == "METHOD_AMOUNT")
            {
                MethodAmountPolicy convertedMethodAmountPolicy = (MethodAmountPolicy)response;
                return ParseTransportPolicyToClientPolicy(convertedMethodAmountPolicy);
            }
            else
            {
                //TODO: Throw real UnknownPolicyException
                throw new Exception("SHITS BROKE YO");
            }
        }

        private DomainPolicy.IPolicy ParseTransportPolicyToClientPolicy(IPolicy policy)
        {
            DomainPolicy.IPolicy returnPolicy = null;
            if(policy.Type == "LEGACY")
            {
                AuthPolicy convertedLegacyPolicy = (AuthPolicy)policy;
                //Grab a parsed object to save having to reimplement policy logic
                ServicePolicy parsedLegacyPolicy = ServicePolicy.FromTransport(convertedLegacyPolicy);
                List<DomainPolicy.IFence> convertedGeoFences = null;
                List<AuthPolicy.TimeFence> convertedTimeFences = null;
                foreach (var factor in convertedLegacyPolicy.Factors)
                {
                    if (factor.Factor == AuthPolicy.FactorType.Geofence)
                    {
                        foreach (var location in factor.Attributes.Locations)
                            convertedGeoFences.Add(
                                new DomainPolicy.GeoCircleFence(
                                    name: location.Name,
                                    latitude: location.Latitude,
                                    longitude: location.Longitude,
                                    radius: location.Radius
                                )
                            );
                    }
                    else if (factor.Factor == AuthPolicy.FactorType.TimeFence)
                    {
                        convertedTimeFences = factor.Attributes.TimeFences;
                    }
                }
                return new DomainPolicy.LegacyPolicy(
                    fences: convertedGeoFences,
                    denyRootedJailbroken: (bool)parsedLegacyPolicy.JailbreakDetection,
                    amount: parsedLegacyPolicy.RequiredFactors,
                    inherenceRequired: (bool)parsedLegacyPolicy.RequireInherenceFactor,
                    knowledgeRequired: (bool)parsedLegacyPolicy.RequireKnowledgeFactor,
                    possessionRequired: (bool)parsedLegacyPolicy.RequirePossessionFactor,
                    timeRestrictions: convertedTimeFences
                );
            }
            else if(policy.Type == "COND_GEO")
            {
                ConditionalGeoFencePolicy convertedPolicy = (ConditionalGeoFencePolicy)policy;
                DomainPolicy.IPolicy inside = ParseTransportPolicyToClientPolicy(convertedPolicy.Inside);
                DomainPolicy.IPolicy outside = ParseTransportPolicyToClientPolicy(convertedPolicy.Outside);
                List<DomainPolicy.IFence> fences = GetClientFences(convertedPolicy.Fences);
                returnPolicy = new DomainPolicy.ConditionalGeoFencePolicy(
                    inside: inside,
                    outside: outside,
                    fences: fences,
                    denyRootedJailbroken: (bool)convertedPolicy.DenyRootedJailbroken,
                    denyEmulatorSimulator: (bool)convertedPolicy.DenyEmulatorSimulator
                );
            }
            else if(policy.Type == "FACTORS")
            {
                FactorsPolicy convertedPolicy = (FactorsPolicy)policy;
                List<DomainPolicy.IFence> fences = GetClientFences(convertedPolicy.Fences);
                returnPolicy = new DomainPolicy.FactorsPolicy(
                    fences: fences,
                    requireKnowledgeFactor: convertedPolicy.Factors.Contains("KNOWLEDGE"),
                    requirePossessionFactor: convertedPolicy.Factors.Contains("POSSESSION"),
                    requireInherenceFactor: convertedPolicy.Factors.Contains("INHERENCE")
                );
            }
            else if(policy.Type == "METHOD_AMOUNT")
            {
                MethodAmountPolicy convertedPolicy = (MethodAmountPolicy)policy;
                List<DomainPolicy.IFence> fences = GetClientFences(convertedPolicy.Fences);
                returnPolicy = new DomainPolicy.MethodAmountPolicy(
                    fences: fences,
                    amount: convertedPolicy.Amount,
                    denyRootedJailbroken: (bool)convertedPolicy.DenyRootedJailbroken,
                    denyEmulatorSimulator: (bool)convertedPolicy.DenyEmulatorSimulator
                );
            }


            return returnPolicy;
        }

        private IPolicy ParseClientPolicyToTransportPolicy(DomainPolicy.IPolicy policy)
        {
            IPolicy returnPolicy = null;
            if (policy.GetType() == typeof(DomainPolicy.LegacyPolicy))
            {
                // THIS IS COMPLETELY DIFFERENT, IT NEEDS TO TRANSFORM TO OLD OBJECT
                DomainPolicy.LegacyPolicy convertedPolicy = (DomainPolicy.LegacyPolicy)policy;

                List<AuthPolicy.Location> convertedFences = null;
                foreach(DomainPolicy.GeoCircleFence geoCircleFence in convertedPolicy.Fences)
                {
                    convertedFences.Add(new AuthPolicy.Location(
                        radius: geoCircleFence.Radius,
                        longitude: geoCircleFence.Longitude,
                        latitude: geoCircleFence.Latitude,
                        name: geoCircleFence.Name
                        )
                    );
                }

                returnPolicy = new AuthPolicy(
                    any: convertedPolicy.Amount,
                    requireKnowledgeFactor: convertedPolicy.KnowledgeRequired,
                    requireInherenceFactor: convertedPolicy.InherenceRequired,
                    requirePossessionFactor: convertedPolicy.PossessionRequired,
                    deviceIntegrity: convertedPolicy.DenyRootedJailbroken,
                    locations: convertedFences,
                    timeFences: convertedPolicy.TimeRestrictions
                    ); 
            }
            else if (policy.GetType() == typeof(DomainPolicy.ConditionalGeoFencePolicy))
            {
                DomainPolicy.ConditionalGeoFencePolicy convertedPolicy = (DomainPolicy.ConditionalGeoFencePolicy)policy;
                IPolicy inside = ParseClientPolicyToTransportPolicy(convertedPolicy.Inside);
                IPolicy outside = ParseClientPolicyToTransportPolicy(convertedPolicy.Outside);
                List<IFence> fences = GetTransportFences(convertedPolicy.Fences);
                returnPolicy = new ConditionalGeoFencePolicy(
                    inside: inside,
                    outside: outside,
                    fences: fences,
                    denyRootedJailbroken: (bool)convertedPolicy.DenyRootedJailbroken,
                    denyEmulatorSimulator: (bool)convertedPolicy.DenyEmulatorSimulator
                );
            }
            else if (policy.GetType() == typeof(DomainPolicy.FactorsPolicy))
            {
                DomainPolicy.FactorsPolicy convertedPolicy = (DomainPolicy.FactorsPolicy)policy;
                List<IFence> fences = GetTransportFences(convertedPolicy.Fences);

                List<string> factors = null;
                if (convertedPolicy.RequireInherenceFactor) factors.Add("INHERENCE");
                if (convertedPolicy.RequireKnowledgeFactor) factors.Add("KNOWLEDGE");
                if (convertedPolicy.RequirePossessionFactor) factors.Add("POSSESSION");

                returnPolicy = new FactorsPolicy(
                    denyRootedJailbroken: convertedPolicy.DenyRootedJailbroken,
                    denyEmulatorSimulator: convertedPolicy.DenyEmulatorSimulator,
                    fences: fences,
                    factors: factors
                );
            }
            else if (policy.GetType() == typeof(DomainPolicy.MethodAmountPolicy))
            {
                DomainPolicy.MethodAmountPolicy convertedPolicy = (DomainPolicy.MethodAmountPolicy)policy;
                List<IFence> fences = GetTransportFences(convertedPolicy.Fences);
                returnPolicy = new MethodAmountPolicy(
                    fences: fences,
                    amount: convertedPolicy.Amount,
                    denyRootedJailbroken: (bool)convertedPolicy.DenyRootedJailbroken,
                    denyEmulatorSimulator: (bool)convertedPolicy.DenyEmulatorSimulator
                );
            }
            
            return returnPolicy;
        }

        private List<DomainPolicy.IFence> GetClientFences(List<IFence> fences)
        {
            List<DomainPolicy.IFence> convertedFences = null;
            foreach(IFence fence in fences)
            {
                if(fence.GetType() == typeof(GeoCircleFence))
                {
                    GeoCircleFence convertedFence = (GeoCircleFence)fence;
                    convertedFences.Add(
                        new DomainPolicy.GeoCircleFence(
                            name: convertedFence.Name,
                            latitude: convertedFence.Latitude,
                            longitude: convertedFence.Longitude,
                            radius: convertedFence.Radius
                        )
                    );
                }
                else if(fence.GetType() == typeof(TerritoryFence))
                {
                    TerritoryFence convertedFence = (TerritoryFence)fence;
                    convertedFences.Add(
                        new DomainPolicy.TerritoryFence(
                            name: convertedFence.Name,
                            country: convertedFence.Country,
                            administrativeArea: convertedFence.AdministrativeArea,
                            postalCode: convertedFence.PostalCode
                        )
                    );
                }
            }

            return convertedFences;

        }

        private List<IFence> GetTransportFences(List<DomainPolicy.IFence> fences)
        {
            List<IFence> convertedFences = null;
            foreach (DomainPolicy.IFence fence in fences)
            {
                if (fence.GetType() == typeof(DomainPolicy.GeoCircleFence))
                {
                    DomainPolicy.GeoCircleFence convertedFence = (DomainPolicy.GeoCircleFence)fence;
                    convertedFences.Add(
                        new GeoCircleFence(
                            name: convertedFence.Name,
                            latitude: convertedFence.Latitude,
                            longitude: convertedFence.Longitude,
                            radius: convertedFence.Radius
                        )
                    );
                }
                else if (fence.GetType() == typeof(DomainPolicy.TerritoryFence))
                {
                    DomainPolicy.TerritoryFence convertedFence = (DomainPolicy.TerritoryFence)fence;
                    convertedFences.Add(
                        new TerritoryFence(
                            name: convertedFence.Name,
                            country: convertedFence.Country,
                            administrativeArea: convertedFence.AdministrativeArea,
                            postalCode: convertedFence.PostalCode
                        )
                    );
                }
            }

            return convertedFences;

        }

        public void SetAdvancedServicePolicy(Guid serviceId, DomainPolicy.IPolicy policy)
        {
            /* Convert from DomainPolicy to a TransportPolicy */
            IPolicy requestPolicy = null;
            if(policy.GetType() == typeof(DomainPolicy.LegacyPolicy))
            {
                DomainPolicy.LegacyPolicy legacyPolicy =
                    (DomainPolicy.LegacyPolicy)policy;
                List<AuthPolicy.Location> locations = null;
                foreach ( GeoCircleFence geoCircleFence in legacyPolicy.Fences)
                {
                    locations.Add(new AuthPolicy.Location(
                            geoCircleFence.Name,
                            geoCircleFence.Radius,
                            geoCircleFence.Latitude,
                            geoCircleFence.Longitude
                        ));
                }
                requestPolicy = new AuthPolicy(
                    legacyPolicy.Amount, legacyPolicy.KnowledgeRequired,
                    legacyPolicy.InherenceRequired, legacyPolicy.PossessionRequired,
                    legacyPolicy.DenyRootedJailbroken,locations,
                    legacyPolicy.TimeRestrictions);
            }
            else if(policy.GetType() == typeof(DomainPolicy.ConditionalGeoFencePolicy))
            {
                requestPolicy = ParseClientPolicyToTransportPolicy(policy);
            }
            else if (policy.GetType() == typeof(DomainPolicy.MethodAmountPolicy))
            {
                requestPolicy = ParseClientPolicyToTransportPolicy(policy);
            }
            else if (policy.GetType() == typeof(DomainPolicy.FactorsPolicy))
            {
                requestPolicy = ParseClientPolicyToTransportPolicy(policy);
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
