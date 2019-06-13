using System;
using System.IO;
using iovation.LaunchKey.Sdk.Client;
using OpenQA.Selenium.Appium;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow
{
    public class TestConfiguration
    {
        private OrganizationFactory organizationFactory;
        public AppiumConfigs appiumConfigs;

        public TestConfiguration()
        {
            var keyPath = Path.Combine("Secrets", "OrgPrivateKey.txt");
            var idPath = Path.Combine("Secrets", "OrgId.txt");
            var appiumConfiguration = Path.Combine("Secrets", "AppiumConfig.txt");

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

            if (File.Exists(appiumConfiguration))
            {
                appiumConfigs = JsonConvert.DeserializeObject<AppiumConfigs>(File.ReadAllText(appiumConfiguration));
            }

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

    public class AppiumConfigs
    {
        [JsonProperty("appium_url")]
        public string AppiumURL { get; set; }

        [JsonProperty("platform_name")]
        public string PlatformName { get; set; }

        [JsonProperty("platform_version")]
        public string PlatformVersion { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("app_apk_path")]
        public string AppFilePath { get; set; }

        public override string ToString()
        {
            return $" Appium URL: {AppiumURL} \n PlatformName: {PlatformName}\n PlatformVersion: {PlatformVersion} \n DeviceName: {DeviceName} \n AppFilePath: {AppFilePath}\n";
        }
    }
}