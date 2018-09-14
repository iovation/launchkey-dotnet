using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Directory;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    public class DirectoryClientContext : IDisposable
    {
		private readonly TestConfiguration _testConfiguration;
		private readonly OrgClientContext _orgClientContext;

		public List<Session> LoadedSessions => _loadedSessions;
		public CreatedServiceInfo LastCreatedService => _ownedServices.Last();
		public Service LoadedService => _loadedService;
		public List<Service> LoadedServices => _loadedServices;

		private string _currentUserId;
		private List<Session> _loadedSessions;
		private CreatedServiceInfo _lastCreatedService;
		private List<CreatedServiceInfo> _ownedServices = new List<CreatedServiceInfo>();
		private Service _loadedService;
		private List<Service> _loadedServices;

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

	}
}
