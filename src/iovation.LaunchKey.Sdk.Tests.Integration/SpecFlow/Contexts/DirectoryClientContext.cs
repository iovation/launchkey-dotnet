using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain;
using iovation.LaunchKey.Sdk.Domain.Directory;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    public class DirectoryClientContext : IDisposable
    {
        private readonly TestConfiguration _testConfiguration;
        private readonly OrgClientContext _orgClientContext;

        // session contextual data
        public List<Session> LoadedSessions => _loadedSessions;
        public CreatedServiceInfo LastCreatedService => _ownedServices.Last();
        public Service LoadedService => _loadedService;
        public List<Service> LoadedServices => _loadedServices;
        public DirectoryUserDeviceLinkData LastLinkResponse => _linkData;
        public string CurrentUserId => _currentUserId;

        // public key contextual data
        public List<PublicKey> LoadedServicePublicKeys => _loadedServicePublicKeys;
        public List<string> AddedServicePublicKeys => _addedServicePublicKeys;

        // policy-related contextual data
        public ServicePolicy LoadedServicePolicy => _loadedServicePolicy;
        public IPolicy LoadedAdvancedServicePolicy => _loadedAdvancedServicePolicy;

        // device-related contextual data
        public List<Device> LoadedDevices => _loadedDevices;

        private List<PublicKey> _loadedServicePublicKeys;
        private List<string> _addedServicePublicKeys = new List<string>();
        private string _currentUserId;
        private List<Session> _loadedSessions;
        private CreatedServiceInfo _lastCreatedService;
        private List<CreatedServiceInfo> _ownedServices = new List<CreatedServiceInfo>();
        private Service _loadedService;
        private List<Service> _loadedServices;
        private ServicePolicy _loadedServicePolicy = new ServicePolicy();
        private IPolicy _loadedAdvancedServicePolicy;
        private DirectoryUserDeviceLinkData _linkData;
        private List<Device> _loadedDevices;

        public DirectoryClientContext(TestConfiguration testConfiguration, OrgClientContext orgClientContext)
        {
            _testConfiguration = testConfiguration;
            _orgClientContext = orgClientContext;
        }

        private IDirectoryClient GetDirectoryClient()
        {
            return _testConfiguration.GetDirectoryClient(_orgClientContext.LastCreatedDirectory.Id.ToString());
        }

        public void LinkDevice(string userId, int? ttl = null)
        {
            if (ttl==null)
            {
                _linkData = GetDirectoryClient().LinkDevice(userId);
            }
            else
            {
                _linkData = GetDirectoryClient().LinkDevice(userId, ttl);
            }
            _currentUserId = userId;
        }

        public void EndAllServiceSessions(string userId)
        {
            GetDirectoryClient().EndAllServiceSessions(userId);
        }

        public void EndAllServiceSessionsForCurrentUser()
        {
            EndAllServiceSessions(_currentUserId);
        }

        public void LoadSessions(string userId)
        {
            _loadedSessions = GetDirectoryClient().GetAllServiceSessions(userId);
        }

        public void LoadSessionsForCurrentUser()
        {
            LoadSessions(_currentUserId);
        }

        public void CreateService(string serviceName)
        {
            CreateService(
                serviceName,
                "Test desc",
                new Uri("http://a.com/icon"),
                new Uri("http://a.com/cb"),
                true
            );
        }

        public void CreateService(string serviceName, string description, Uri icon, Uri callbackUrl, bool active)
        {
            var serviceGuid = GetDirectoryClient().CreateService(serviceName, description, icon, callbackUrl, active);
            _lastCreatedService = new CreatedServiceInfo(serviceGuid, serviceName);
            _ownedServices.Add(_lastCreatedService);
        }

        public void Dispose()
        {
            foreach (var ownedService in _ownedServices)
            {
                try
                {
                    GetDirectoryClient().UpdateService(ownedService.Id, ownedService.Name, "DELETE ME", null, null, false);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error while deactivating service: {e}");
                }
            }
        }

        public void LoadService(Guid serviceId)
        {
            _loadedService = GetDirectoryClient().GetService(serviceId);
        }

        public void LoadAllServices()
        {
            _loadedServices = GetDirectoryClient().GetAllServices();
        }

        public void LoadServices(List<Guid> serviceIds)
        {
            _loadedServices = GetDirectoryClient().GetServices(serviceIds);
        }

        public void UpdateService(Guid serviceId, string name, string description, Uri icon, Uri callback, bool active)
        {
            GetDirectoryClient().UpdateService(serviceId, name, description, icon, callback, active);
        }

        public void AddServicePublicKey(Guid serviceId, string key, bool active, DateTime? expire, KeyType keyType)
        {
            _addedServicePublicKeys.Add(GetDirectoryClient().AddServicePublicKey(serviceId, key, active, expire, keyType));
        }

        public void AddServicePublicKey(Guid serviceId, string key, bool active, DateTime? expire)
        {
            AddServicePublicKey(serviceId, key, active, expire, KeyType.BOTH);
        }

        public void LoadServicePublicKeys(Guid serviceId)
        {
            _loadedServicePublicKeys = GetDirectoryClient().GetServicePublicKeys(serviceId);
        }

        public void RemoveServicePublicKey(Guid serviceId, string keyId)
        {
            GetDirectoryClient().RemoveServicePublicKey(serviceId, keyId);
        }

        public void DeactivateServicePublicKey(Guid serviceId, string keyId)
        {
            var key = GetDirectoryClient().GetServicePublicKeys(serviceId).First(k => k.Id == keyId);
            GetDirectoryClient().UpdateServicePublicKey(serviceId, keyId, false, key.Expires);
        }

        public void UpdateServicePublicKeyExpires(Guid serviceId, string keyId, DateTime? expires)
        {
            var key = GetDirectoryClient().GetServicePublicKeys(serviceId).First(k => k.Id == keyId);
            GetDirectoryClient().UpdateServicePublicKey(serviceId, keyId, key.Active, expires);
        }

        public void UpdateServicePublicKey(Guid serviceId, string keyId, bool active, DateTime? expires)
        {
            GetDirectoryClient().UpdateServicePublicKey(serviceId, keyId, active, expires);
        }
        public void LoadServicePolicy(Guid serviceId)
        {
            _loadedServicePolicy = GetDirectoryClient().GetServicePolicy(serviceId);
        }

        public void SetServicePolicy(Guid serviceId, ServicePolicy servicePolicy)
        {
            GetDirectoryClient().SetServicePolicy(serviceId, servicePolicy);

            // clear the service policy so we don't accidentally inspect data we just sent.
            _loadedServicePolicy = null;
        }

        public void RemoveServicePolicy(Guid serviceId)
        {
            GetDirectoryClient().RemoveServicePolicy(serviceId);
        }

        public void UnlinkCurrentDevice()
        {
            GetDirectoryClient().UnlinkDevice(_currentUserId, _loadedDevices[0].Id);
        }

        public void LoadDevices(string userId)
        {
            _loadedDevices = GetDirectoryClient().GetLinkedDevices(userId);
        }

        public void LoadDevicesForCurrentUser()
        {
            _loadedDevices = GetDirectoryClient().GetLinkedDevices(_currentUserId);
        }

        public void UnlinkDeviceForCurrentUser(string deviceId)
        {
            GetDirectoryClient().UnlinkDevice(_currentUserId, deviceId);
        }

        public void UnlinkDevice(string userId, string deviceId)
        {
            GetDirectoryClient().UnlinkDevice(userId, deviceId);
        }

        public void LoadAdvancedServicePolicy(Guid serviceId)
        {
            _loadedAdvancedServicePolicy = GetDirectoryClient().GetAdvancedServicePolicy(serviceId);
        }

        public void SetAdvancedServicePolicy(Guid serviceId, IPolicy authPolicy)
        {
            GetDirectoryClient().SetAdvancedServicePolicy(serviceId, authPolicy);

            // clear the service policy so we don't accidentally inspect data we just sent.
            _loadedAdvancedServicePolicy = null;
        }

        public void CreateMethodAmountPolicy()
        {
            _loadedAdvancedServicePolicy = new MethodAmountPolicy(fences: null);
        }

        public void AddIFenceToAdvancedPolicy(List<IFence> fences)
        {
            IPolicy currentPolicy = _loadedAdvancedServicePolicy;

            var combinedFences = fences.Concat(currentPolicy.Fences);

            if(currentPolicy is MethodAmountPolicy)
            {
                _loadedAdvancedServicePolicy = new MethodAmountPolicy(
                    fences: combinedFences.ToList(),
                    amount: ((MethodAmountPolicy)currentPolicy).Amount,
                    denyRootedJailbroken: ((MethodAmountPolicy)currentPolicy).DenyRootedJailbroken,
                    denyEmulatorSimulator: ((MethodAmountPolicy)currentPolicy).DenyEmulatorSimulator
                );
            }
            else if(currentPolicy is FactorsPolicy)
            {
                _loadedAdvancedServicePolicy = new FactorsPolicy(
                    fences: combinedFences.ToList(),
                    requireKnowledgeFactor: ((FactorsPolicy)currentPolicy).RequireKnowledgeFactor,
                    requirePossessionFactor: ((FactorsPolicy)currentPolicy).RequirePossessionFactor,
                    requireInherenceFactor: ((FactorsPolicy)currentPolicy).RequireInherenceFactor,
                    denyRootedJailbroken: ((FactorsPolicy)currentPolicy).DenyRootedJailbroken,
                    denyEmulatorSimulator: ((FactorsPolicy)currentPolicy).DenyEmulatorSimulator
                );
            }
            else if (_loadedAdvancedServicePolicy is ConditionalGeoFencePolicy)
            {
                _loadedAdvancedServicePolicy = new ConditionalGeoFencePolicy(
                    inside: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).Inside,
                    outside: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).Outside,
                    fences: combinedFences.ToList(),
                    denyRootedJailbroken: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).DenyRootedJailbroken,
                    denyEmulatorSimulator: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).DenyEmulatorSimulator
                );
            }
        }

        public void SetAmountOnMethodAmountPolicy(int amount)
        {
            IPolicy currentPolicy = _loadedAdvancedServicePolicy;
            _loadedAdvancedServicePolicy = new MethodAmountPolicy(
                fences: currentPolicy.Fences,
                amount: amount,
                denyRootedJailbroken: currentPolicy.DenyRootedJailbroken,
                denyEmulatorSimulator: currentPolicy.DenyEmulatorSimulator
            );
        }

        public void CreateFactorsPolicy()
        {
            _loadedAdvancedServicePolicy = new FactorsPolicy(null);
        }

        public void SetFactors(bool requireKnowledge, bool requirePossession, bool requireInherence)
        {
            _loadedAdvancedServicePolicy = new FactorsPolicy(
                fences: _loadedAdvancedServicePolicy.Fences,
                requireKnowledgeFactor: requireKnowledge,
                requireInherenceFactor: requireInherence,
                requirePossessionFactor: requirePossession,
                denyEmulatorSimulator: _loadedAdvancedServicePolicy.DenyEmulatorSimulator,
                denyRootedJailbroken: _loadedAdvancedServicePolicy.DenyRootedJailbroken
            );
        }

        public void SetDenyRootedJailbroken(bool value)
        {
            if (_loadedAdvancedServicePolicy is MethodAmountPolicy)
            {
                _loadedAdvancedServicePolicy = new MethodAmountPolicy(
                    fences: ((MethodAmountPolicy)_loadedAdvancedServicePolicy).Fences,
                    amount: ((MethodAmountPolicy)_loadedAdvancedServicePolicy).Amount,
                    denyRootedJailbroken: value,
                    denyEmulatorSimulator: ((MethodAmountPolicy)_loadedAdvancedServicePolicy).DenyEmulatorSimulator
                );
            }
            else if (_loadedAdvancedServicePolicy is FactorsPolicy)
            {
                _loadedAdvancedServicePolicy = new FactorsPolicy(
                    fences: ((FactorsPolicy)_loadedAdvancedServicePolicy).Fences,
                    requireKnowledgeFactor: ((FactorsPolicy)_loadedAdvancedServicePolicy).RequireKnowledgeFactor,
                    requirePossessionFactor: ((FactorsPolicy)_loadedAdvancedServicePolicy).RequirePossessionFactor,
                    requireInherenceFactor: ((FactorsPolicy)_loadedAdvancedServicePolicy).RequireInherenceFactor,
                    denyRootedJailbroken: value,
                    denyEmulatorSimulator: ((FactorsPolicy)_loadedAdvancedServicePolicy).DenyEmulatorSimulator
                );
            }
            else if (_loadedAdvancedServicePolicy is ConditionalGeoFencePolicy)
            {
                _loadedAdvancedServicePolicy = new ConditionalGeoFencePolicy(
                    inside: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).Inside,
                    outside: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).Outside,
                    fences: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).Fences,
                    denyRootedJailbroken: value,
                    denyEmulatorSimulator: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).DenyEmulatorSimulator
                );
            }
        }

        public void SetDenyEmulatorSimulator(bool value)
        {
            if (_loadedAdvancedServicePolicy is MethodAmountPolicy)
            {
                _loadedAdvancedServicePolicy = new MethodAmountPolicy(
                    fences: ((MethodAmountPolicy)_loadedAdvancedServicePolicy).Fences,
                    amount: ((MethodAmountPolicy)_loadedAdvancedServicePolicy).Amount,
                    denyRootedJailbroken: ((MethodAmountPolicy)_loadedAdvancedServicePolicy).DenyRootedJailbroken,
                    denyEmulatorSimulator: value
                );
            }
            else if (_loadedAdvancedServicePolicy is FactorsPolicy)
            {
                _loadedAdvancedServicePolicy = new FactorsPolicy(
                    fences: ((FactorsPolicy)_loadedAdvancedServicePolicy).Fences,
                    requireKnowledgeFactor: ((FactorsPolicy)_loadedAdvancedServicePolicy).RequireKnowledgeFactor,
                    requirePossessionFactor: ((FactorsPolicy)_loadedAdvancedServicePolicy).RequirePossessionFactor,
                    requireInherenceFactor: ((FactorsPolicy)_loadedAdvancedServicePolicy).RequireInherenceFactor,
                    denyRootedJailbroken: ((FactorsPolicy)_loadedAdvancedServicePolicy).DenyEmulatorSimulator,
                    denyEmulatorSimulator: value
                );
            }
            else if(_loadedAdvancedServicePolicy is ConditionalGeoFencePolicy)
            {
                _loadedAdvancedServicePolicy = new ConditionalGeoFencePolicy(
                    inside: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).Inside,
                    outside: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).Outside,
                    fences: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).Fences,
                    denyRootedJailbroken: ((ConditionalGeoFencePolicy)_loadedAdvancedServicePolicy).DenyRootedJailbroken,
                    denyEmulatorSimulator: value
                );
            }
        }

        public void CreateConditionaGeofence()
        {
            MethodAmountPolicy defaultPolicy = new MethodAmountPolicy(null, 0, false, false);

            List<IFence> fences = new List<IFence>() {
                new TerritoryFence("US")
            };

            _loadedAdvancedServicePolicy = new ConditionalGeoFencePolicy(
                inside: defaultPolicy,
                outside: defaultPolicy,
                fences: fences,
                denyRootedJailbroken: false,
                denyEmulatorSimulator: false
            );
        }

        public void SetInsideConditionalGeofencePolicy(IPolicy insidePolicy)
        {
            _loadedAdvancedServicePolicy = new ConditionalGeoFencePolicy(
                inside: insidePolicy,
                outside: (_loadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside,
                fences: (_loadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Fences,
                denyRootedJailbroken: (_loadedAdvancedServicePolicy as ConditionalGeoFencePolicy).DenyRootedJailbroken,
                denyEmulatorSimulator: (_loadedAdvancedServicePolicy as ConditionalGeoFencePolicy).DenyEmulatorSimulator
            );
        }

        public void SetOutsideConditionalGeofencePolicy(IPolicy outsidePolicy)
        {
            _loadedAdvancedServicePolicy = new ConditionalGeoFencePolicy(
                inside: (_loadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside,
                outside: outsidePolicy,
                fences: (_loadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Fences,
                denyRootedJailbroken: (_loadedAdvancedServicePolicy as ConditionalGeoFencePolicy).DenyRootedJailbroken,
                denyEmulatorSimulator: (_loadedAdvancedServicePolicy as ConditionalGeoFencePolicy).DenyEmulatorSimulator
            );
        }
    }
}
