using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iovation.LaunchKey.Sdk.Crypto;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
    class ClientFactories
    {
        private static FactoryFactory MakeFactoryFactory(string apiURL)
        {
            var factoryFactoryBuilder = new FactoryFactoryBuilder();
            if (apiURL != null)
            {
                factoryFactoryBuilder.SetApiBaseUrl(apiURL).SetOffsetTtl(60);
            }
            var factory = factoryFactoryBuilder.Build();
            return factory;
        }

        private static string GetKeyFingerprint(string privateKey)
        {
            var crypto = new BouncyCastleCrypto();
            var parsedKey = crypto.LoadRsaPrivateKey(privateKey); 
            return crypto.GeneratePublicKeyFingerprintFromPrivateKey(parsedKey);
        }
        
        private static Client.OrganizationFactory MakeOrganizationFactory(string orgId, string privateKeyLocation, string apiURL, IEnumerable<string> encryptionPrivateKeys = null)
        {
            var privateKey = File.ReadAllText(privateKeyLocation);
            var keys = new List<string>{privateKey};
            if (encryptionPrivateKeys != null)
            {
                keys.AddRange(encryptionPrivateKeys.Select(encryptionKey => File.ReadAllText(encryptionKey)));
                return MakeFactoryFactory(apiURL).MakeOrganizationFactory(orgId, keys, GetKeyFingerprint(privateKey));
            }

            var factory = MakeFactoryFactory(apiURL).MakeOrganizationFactory(orgId, privateKey);
            return factory;
        }

        private static Client.DirectoryFactory MakeDirectoryFactory(string directoryId, string privateKeyLocation, string apiURL, IEnumerable<string> encryptionPrivateKeys = null)
        {
            var privateKey = File.ReadAllText(privateKeyLocation);
            var keys = new List<string>{privateKey};
            if (encryptionPrivateKeys != null)
            {
                keys.AddRange(encryptionPrivateKeys.Select(encryptionKey => File.ReadAllText(encryptionKey)));
                return MakeFactoryFactory(apiURL).MakeDirectoryFactory(directoryId, keys, GetKeyFingerprint(privateKey));
            }

            return MakeFactoryFactory(apiURL).MakeDirectoryFactory(directoryId, privateKey);
        }

        private static Client.ServiceFactory MakeServiceFactory(string serviceId, string privateKeyLocation, string apiURL, IEnumerable<string> encryptionPrivateKeys = null)
        {
            var privateKey = File.ReadAllText(privateKeyLocation);
            var keys = new List<string>{privateKey};
            if (encryptionPrivateKeys != null)
            {
                keys.AddRange(encryptionPrivateKeys.Select(encryptionKey => File.ReadAllText(encryptionKey)));
                return MakeFactoryFactory(apiURL).MakeServiceFactory(serviceId, keys, GetKeyFingerprint(privateKey));
            }
            
            return MakeFactoryFactory(apiURL).MakeServiceFactory(serviceId, privateKey);
        }

        public static Client.IOrganizationClient MakeOrganizationClient(string orgId, string privateKeyLocation, string apiURL, IEnumerable<string> encryptionPrivateKeys = null)
        {
            var organizationClient = MakeOrganizationFactory(orgId, privateKeyLocation, apiURL, encryptionPrivateKeys).MakeOrganizationClient();
            return organizationClient;

        }

        public static Client.IDirectoryClient MakeDirectoryClient(string directoryId, string privateKeyLocation, string apiURL, IEnumerable<string> encryptionPrivateKeys = null)
        {
            var directoryClient = MakeDirectoryFactory(directoryId, privateKeyLocation, apiURL, encryptionPrivateKeys).MakeDirectoryClient();
            return directoryClient;
        }

        public static Client.IServiceClient MakeOrganizationServiceClient(string orgId, string privateKeyLocation, string serviceId, string apiURL, IEnumerable<string> encryptionPrivateKeys = null)
        {
            Client.IServiceClient serviceClient = MakeOrganizationFactory(orgId, privateKeyLocation, apiURL, encryptionPrivateKeys).MakeServiceClient(serviceId);
            return serviceClient;
        }

        public static Client.IServiceClient MakeDirectoryServiceClient(string directoryId, string privateKeyLocation, string serviceId, string apiURL, IEnumerable<string> encryptionPrivateKeys = null)
        {
            var directoryServiceClient = MakeDirectoryFactory(directoryId, privateKeyLocation, apiURL, encryptionPrivateKeys).MakeServiceClient(serviceId);
            return directoryServiceClient;
        }

        public static Client.IDirectoryClient MakeOrganizationDirectoryClient(string orgId, string privateKeyLocation, string directoryId, string apiURL, IEnumerable<string> encryptionPrivateKeys = null)
        {
            var directoryClient = MakeOrganizationFactory(orgId, privateKeyLocation, apiURL, encryptionPrivateKeys).MakeDirectoryClient(directoryId);
            return directoryClient;
        }

        public static Client.IServiceClient MakeServiceClient(string serviceId, string privateKeyLocation, string apiURL, IEnumerable<string> encryptionPrivateKeys = null)
        {
            return MakeServiceFactory(serviceId, privateKeyLocation, apiURL, encryptionPrivateKeys).MakeServiceClient();
        }
    }
}

