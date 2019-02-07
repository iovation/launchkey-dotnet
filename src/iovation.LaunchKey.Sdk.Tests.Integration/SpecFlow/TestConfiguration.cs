using System;
using System.IO;
using iovation.LaunchKey.Sdk.Client;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow
{
    public class TestConfiguration
    {
        private OrganizationFactory organizationFactory;

        public TestConfiguration()
        {
            var keyPath = Path.Combine("Secrets", "OrgPrivateKey.txt");
            var idPath = Path.Combine("Secrets", "OrgId.txt");

            if (!File.Exists(keyPath) || !File.Exists(idPath))
                throw new Exception($"Test configuration is invalid -- files with secrets should exist: {keyPath}, {idPath}");

            var factoryFactoryBuilder = new FactoryFactoryBuilder();

            var apiUrlPath = Path.Combine("Secrets", "ApiUrl.txt");
            if (File.Exists(apiUrlPath))
            {
                var apiUrl = File.ReadAllText(apiUrlPath);
                factoryFactoryBuilder.SetApiBaseUrl(apiUrl);
            }

            var orgPrivateKey = File.ReadAllText(keyPath);
            var orgId = File.ReadAllText(idPath);
            var factoryFactory = factoryFactoryBuilder.Build();
            organizationFactory = factoryFactory.MakeOrganizationFactory(orgId, orgPrivateKey);
        }

        public IOrganizationClient GetOrgClient()
        {
            return organizationFactory.MakeOrganizationClient();
        }

        public IDirectoryClient GetDirectoryClient(string directoryId)
        {
            return organizationFactory.MakeDirectoryClient(directoryId);
        }

        public IServiceClient GetServiceClient(string serviceId)
        {
            return organizationFactory.MakeServiceClient(serviceId);
        }
    }
}