using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Organization;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Tests.Integration.Steps.OrgClient;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps
{
	public class OrgClientContext : IDisposable
	{
		public Service LoadedService => _loadedService;
		public CreatedServiceInfo LastCreatedService => _ownedServices.Last();
		public List<Service> LoadedServices => _loadedServices;

		private readonly IOrganizationClient _orgClient;
		private CreatedServiceInfo _lastCreatedService = null;
		private List<CreatedServiceInfo> _ownedServices = new List<CreatedServiceInfo>();
		private List<Service> _loadedServices = new List<Service>();
		private Service _loadedService = null;

		public OrgClientContext(TestConfiguration config)
		{
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

		public void Dispose()
		{
			foreach (var ownedService in _ownedServices)
			{
				try
				{
					var service = _orgClient.GetService(ownedService.Id);
					_orgClient.UpdateService(service.Id, service.Name, service.Description, service.Icon, service.CallbackUrl, false);
				}
				catch (Exception e)
				{
					Debug.WriteLine($"Error while deactivating service: {e}");
				}
			}
		}
	}
}
