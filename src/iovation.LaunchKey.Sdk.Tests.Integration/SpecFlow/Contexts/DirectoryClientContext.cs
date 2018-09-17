using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain;
using iovation.LaunchKey.Sdk.Domain.Directory;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;

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

		// public key contextual data
		public List<PublicKey> LoadedServicePublicKeys => _loadedServicePublicKeys;
		public List<string> AddedServicePublicKeys => _addedServicePublicKeys;

		// policy-related contextual data
		public ServicePolicy LoadedServicePolicy => _loadedServicePolicy;
		
		private List<PublicKey> _loadedServicePublicKeys;
		private List<string> _addedServicePublicKeys = new List<string>();
		private string _currentUserId;
		private List<Session> _loadedSessions;
		private CreatedServiceInfo _lastCreatedService;
		private List<CreatedServiceInfo> _ownedServices = new List<CreatedServiceInfo>();
		private Service _loadedService;
		private List<Service> _loadedServices;
		private ServicePolicy _loadedServicePolicy = new ServicePolicy();


		public DirectoryClientContext(TestConfiguration testConfiguration, OrgClientContext orgClientContext)
		{
			_testConfiguration = testConfiguration;
			_orgClientContext = orgClientContext;
		}

		private IDirectoryClient GetDirectoryClient()
		{
			return _testConfiguration.GetDirectoryClient(_orgClientContext.LastCreatedDirectory.Id.ToString());
		}

		public void LinkDevice(string userId)
		{
			GetDirectoryClient().LinkDevice(userId);
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

		public void AddServicePublicKey(Guid serviceId, string key, bool active, DateTime? expire)
		{
			_addedServicePublicKeys.Add(GetDirectoryClient().AddServicePublicKey(serviceId, key, active, expire));
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
	}
}
