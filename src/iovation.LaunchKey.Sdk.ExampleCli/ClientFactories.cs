using System;
using System.IO;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
	class ClientFactories
	{

		private static FactoryFactory MakeFactoryFactory(string apiURL)
		{
			var factoryFactoryBuilder = new FactoryFactoryBuilder();
			if (apiURL != null)
			{
				factoryFactoryBuilder.SetApiBaseUrl(apiURL);
			}
			var factory = factoryFactoryBuilder.Build();
			return factory;
		}

		private static Client.OrganizationFactory MakeOrganizationFactory(string orgId, string privateKeyLocation, string apiURL)
		{
			var privateKey = File.ReadAllText(privateKeyLocation);
			var factory = MakeFactoryFactory(apiURL).MakeOrganizationFactory(orgId, privateKey);
			return factory;
		}

		private static Client.DirectoryFactory MakeDirectoryFactory(string directoryId, string apiURL, string directoryKey)
		{
			return MakeFactoryFactory(apiURL).MakeDirectoryFactory(directoryId, directoryKey);
		}

		public static Client.IDirectoryClient MakeDirectoryClient(string directoryId, string privateKeyLocation, string apiURL)
		{
			var privateKey = File.ReadAllText(privateKeyLocation);
			var directoryClient = MakeDirectoryFactory(directoryId, apiURL, privateKey).MakeDirectoryClient();
			return directoryClient;
		}

		public static Client.IServiceClient MakeOrganizationServiceClient(string orgId, string privateKeyLocation, string serviceId, string apiURL)
		{
			Client.IServiceClient serviceClient = MakeOrganizationFactory(orgId, privateKeyLocation, apiURL).MakeServiceClient(serviceId);
			return serviceClient;
		}

		public static Client.IServiceClient MakeDirectoryServiceClient(string directoryId, string privateKeyLocation, string serviceId, string apiURL)
		{
			var directoryServiceClient = MakeDirectoryFactory(directoryId, privateKeyLocation, apiURL).MakeServiceClient(serviceId);
			return directoryServiceClient;
		}

		public static Client.IDirectoryClient MakeOrganizationDirectoryClient(string orgId, string privateKeyLocation, string directoryId, string apiURL)
		{
			var directoryClient = MakeOrganizationFactory(orgId, privateKeyLocation, apiURL).MakeDirectoryClient(directoryId);
			return directoryClient;
		}

		public static Client.IServiceClient MakeServiceClient(string serviceId, string privateKeyLocation, string apiURL)
		{
			var privateKey = File.ReadAllText(privateKeyLocation);
			var serviceClient = MakeFactoryFactory(apiURL).MakeServiceFactory(serviceId, privateKey).MakeServiceClient();
			return serviceClient;
		}
	}
}

