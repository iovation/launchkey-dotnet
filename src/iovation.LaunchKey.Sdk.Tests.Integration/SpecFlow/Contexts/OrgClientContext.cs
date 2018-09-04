using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Organization;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
	public class OrgClientContext : IDisposable
	{
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
		

		private readonly IOrganizationClient _orgClient;
		private Directory _loadedDirectory;
		private List<Directory> _loadedDirectories;
		private List<Guid> _loadedSdkKeys = null;

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

		public void CreateDirectory(string name)
		{
			var id = _orgClient.CreateDirectory(name);
			_ownedDirectories.Add(new CreatedDirectoryInfo(id, name));
		}

		public void UpdateDirectory(Guid directoryId, bool active, string androidKey, string iosP12)
		{
			_orgClient.UpdateDirectory(directoryId, active, androidKey, iosP12);
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

		private List<Guid> _addedSdkKeys = new List<Guid>();

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
					_orgClient.UpdateDirectory(ownedDirectory.Id, false, null, null);
				}
				catch (Exception e)
				{
					Debug.WriteLine($"Error while deactivating directory: {e}");
				}
			}
		}

		
	}
}
