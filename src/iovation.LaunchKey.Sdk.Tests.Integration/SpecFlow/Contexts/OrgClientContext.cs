﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain;
using iovation.LaunchKey.Sdk.Domain.Organization;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    public class OrgClientContext : IDisposable
    {
        private readonly TestConfiguration _config;

        // service-related contextual data
        public Service LoadedService => _loadedService;
        public CreatedServiceInfo LastCreatedService => _ownedServices.Last();
        public List<Service> LoadedServices => _loadedServices;
        private CreatedServiceInfo _lastCreatedService = null;
        private List<CreatedServiceInfo> _ownedServices = new List<CreatedServiceInfo>();
        private List<Service> _loadedServices = new List<Service>();
        private Service _loadedService = null;

        // directory-related contextual data
        public Directory LoadedDirectory => _loadedDirectory;
        public CreatedDirectoryInfo LastCreatedDirectory => _ownedDirectories.Last();
        public List<Directory> LoadedDirectories => _loadedDirectories;
        private List<CreatedDirectoryInfo> _ownedDirectories = new List<CreatedDirectoryInfo>();
        public List<Guid> AddedSdkKeys => _addedSdkKeys;
        public List<Guid> LoadedSdkKeys => _loadedSdkKeys;

        // public keys-related contextual data
        public List<PublicKey> LoadedServicePublicKeys => _loadedServicePublicKeys;
        public List<PublicKey> LoadedDirectoryPublicKeys => _loadedDirectoryPublicKeys;
        public List<string> AddedServicePublicKeys => _addedServicePublicKeys;
        public List<string> AddedDirectoryPublicKeys => _addedDirectoryPublicKeys;

        // policy-related contextual data
        public ServicePolicy LoadedServicePolicy => _loadedServicePolicy;
        public IPolicy LoadedAdvancedServicePolicy => _loadedAdvancedServicePolicy;

        private readonly IOrganizationClient _orgClient;
        private Directory _loadedDirectory;
        private List<Directory> _loadedDirectories;
        private List<Guid> _loadedSdkKeys = null;
        private List<Guid> _addedSdkKeys = new List<Guid>();
        private List<PublicKey> _loadedServicePublicKeys;
        private List<PublicKey> _loadedDirectoryPublicKeys;
        private List<string> _addedServicePublicKeys = new List<string>();
        private List<string> _addedDirectoryPublicKeys = new List<string>();
        private ServicePolicy _loadedServicePolicy = new ServicePolicy();
        private IPolicy _loadedAdvancedServicePolicy;

        public OrgClientContext(TestConfiguration config)
        {
            _config = config;
            _orgClient = config.GetOrgClient();
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
            var serviceGuid = _orgClient.CreateService(serviceName, description, icon, callbackUrl, active);
            _lastCreatedService = new CreatedServiceInfo(serviceGuid, serviceName);
            _ownedServices.Add(_lastCreatedService);
        }

        public void UpdateService(Guid serviceId, string name, string description, Uri icon, Uri callback, bool active)
        {
            _orgClient.UpdateService(serviceId, name, description, icon, callback, active);
        }

        public void LoadAllServices()
        {
            _loadedServices = _orgClient.GetAllServices();
        }

        public void LoadServices(List<Guid> serviceIds)
        {
            _loadedServices = _orgClient.GetServices(serviceIds);
        }

        public void LoadService(Guid serviceId)
        {
            _loadedService = _orgClient.GetService(serviceId);
        }

        public void LoadLastCreatedService()
        {
            _loadedService = _orgClient.GetService(_ownedServices.Last().Id);
        }

        public void CreateDirectory(string name)
        {
            var id = _orgClient.CreateDirectory(name);
            _ownedDirectories.Add(new CreatedDirectoryInfo(id, name));
        }

        public void UpdateDirectory(Guid directoryId, bool active, string androidKey, string iosP12, bool? denialContextEnabled = null, string webhookUrl = null)
        {
            _orgClient.UpdateDirectory(directoryId, active, androidKey, iosP12, denialContextEnabled, webhookUrl);
        }

        public void LoadAllDirectories()
        {
            _loadedDirectories = _orgClient.GetAllDirectories();
        }

        public void LoadDirectories(List<Guid> directoryIds)
        {
            _loadedDirectories = _orgClient.GetDirectories(directoryIds);
        }

        public void LoadDirectory(Guid directoryId)
        {
            _loadedDirectory = _orgClient.GetDirectory(directoryId);
        }

        public void LoadLastCreatedDirectory()
        {
            _loadedDirectory = _orgClient.GetDirectory(LastCreatedDirectory.Id);
        }

        public void GenerateDirectorySDKKeys(Guid directoryId, int quantity)
        {
            for (var i = 0; i < quantity; i++)
            {
                _addedSdkKeys.Add(_orgClient.GenerateAndAddDirectorySdkKey(directoryId));
            }
        }

        public void LoadSdkKeys(Guid directoryId)
        {
            _loadedSdkKeys = _orgClient.GetAllDirectorySdkKeys(directoryId);
        }

        public void RemoveDirectorySdkKey(Guid directoryId, Guid sdkKey)
        {
            _orgClient.RemoveDirectorySdkKey(directoryId, sdkKey);
        }

        public void Dispose()
        {
            foreach (var ownedService in _ownedServices)
            {
                try
                {
                    _orgClient.UpdateService(ownedService.Id, ownedService.Name, "DELETE ME", null, null, false);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error while deactivating service: {e}");
                }
            }

            foreach (var ownedDirectory in _ownedDirectories)
            {
                try
                {
                    _orgClient.UpdateDirectory(ownedDirectory.Id, false, null, null, true, null);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error while deactivating directory: {e}");
                }
            }
        }

        public void AddServicePublicKey(Guid serviceId, string key, bool active, DateTime? expire)
        {
            _addedServicePublicKeys.Add(_orgClient.AddServicePublicKey(serviceId, key, active, expire));
        }

        public void LoadServicePublicKeys(Guid serviceId)
        {
            _loadedServicePublicKeys = _orgClient.GetServicePublicKeys(serviceId);
        }

        public void RemoveServicePublicKey(Guid serviceId, string keyId)
        {
            _orgClient.RemoveServicePublicKey(serviceId, keyId);
        }

        public void DeactivateServicePublicKey(Guid serviceId, string keyId)
        {
            var key = _orgClient.GetServicePublicKeys(serviceId).First(k => k.Id == keyId);
            _orgClient.UpdateServicePublicKey(serviceId, keyId, false, key.Expires);
        }

        public void UpdateServicePublicKeyExpires(Guid serviceId, string keyId, DateTime? expires)
        {
            var key = _orgClient.GetServicePublicKeys(serviceId).First(k => k.Id == keyId);
            _orgClient.UpdateServicePublicKey(serviceId, keyId, key.Active, expires);
        }

        public void UpdateServicePublicKey(Guid serviceId, string keyId, bool active, DateTime? expires)
        {
            _orgClient.UpdateServicePublicKey(serviceId, keyId, active, expires);
        }

        public void AddDirectoryPublicKey(Guid directoryId, string key, bool active, DateTime? expires, KeyType keyType)
        {
            _addedDirectoryPublicKeys.Add(_orgClient.AddDirectoryPublicKey(directoryId, key, active, expires, keyType));
        }

        public void AddDirectoryPublicKey(Guid directoryId, string key, bool active, DateTime? expires)
        {
            AddDirectoryPublicKey(directoryId, key, active, expires, KeyType.BOTH);
        }

        public void LoadDirectoryPublicKeys(Guid directoryId)
        {
            _loadedDirectoryPublicKeys = _orgClient.GetDirectoryPublicKeys(directoryId);
        }

        public void RemoveDirectoryPublicKey(Guid directoryId, string keyId)
        {
            _orgClient.RemoveDirectoryPublicKey(directoryId, keyId);
        }

        public void DeactivateDirectoryPublicKey(Guid directoryId, string keyId)
        {
            var key = _orgClient.GetDirectoryPublicKeys(directoryId).First(k => k.Id == keyId);
            _orgClient.UpdateDirectoryPublicKey(directoryId, keyId, false, key.Expires);
        }

        public void UpdateDirectoryPublicKeyExpires(Guid directoryId, string keyId, DateTime? expires)
        {
            var key = _orgClient.GetDirectoryPublicKeys(directoryId).First(k => k.Id == keyId);
            _orgClient.UpdateDirectoryPublicKey(directoryId, keyId, key.Active, expires);
        }

        public void UpdateDirectoryPublicKey(Guid directoryId, string keyId, bool active, DateTime? expires)
        {
            _orgClient.UpdateDirectoryPublicKey(directoryId, keyId, active, expires);
        }

        public void LoadServicePolicy(Guid serviceId)
        {
            _loadedServicePolicy = _orgClient.GetServicePolicy(serviceId);
        }

        public void SetServicePolicy(Guid serviceId, ServicePolicy servicePolicy)
        {
            _orgClient.SetServicePolicy(serviceId, servicePolicy);

            // clear the service policy so we don't accidentally inspect data we just sent.
            _loadedServicePolicy = null;//new ServicePolicy();
        }

        public void RemoveServicePolicy(Guid serviceId)
        {
            _orgClient.RemoveServicePolicy(serviceId);
        }

        public void LoadAdvancedServicePolicy(Guid serviceId)
        {
            
            _loadedAdvancedServicePolicy = _orgClient.GetAdvancedServicePolicy(serviceId);
        }

        public void SetAdvancedServicePolicy(Guid serviceId, IPolicy authPolicy)
        {
            _orgClient.SetAdvancedServicePolicy(serviceId, authPolicy);

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

            if (currentPolicy is MethodAmountPolicy)
            {
                _loadedAdvancedServicePolicy = new MethodAmountPolicy(
                    fences: combinedFences.ToList(),
                    amount: ((MethodAmountPolicy)currentPolicy).Amount,
                    denyRootedJailbroken: ((MethodAmountPolicy)currentPolicy).DenyRootedJailbroken,
                    denyEmulatorSimulator: ((MethodAmountPolicy)currentPolicy).DenyEmulatorSimulator
                );
            }
            else if (currentPolicy is FactorsPolicy)
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
            else if (_loadedAdvancedServicePolicy is ConditionalGeoFencePolicy)
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
