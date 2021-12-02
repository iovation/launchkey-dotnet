using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Crypto;
using TechTalk.SpecFlow;
using Directory = iovation.LaunchKey.Sdk.Domain.Organization.Directory;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    public class SinglePurposeKeysOrgClientContext : IDisposable
    {
        private readonly TestConfiguration _config;
        private OrganizationFactory _singlePurposeKeysOrganizationFactory;
        private readonly List<Directory> _ownedDirectories = new List<Directory>();

        public SinglePurposeKeysOrgClientContext(TestConfiguration config)
        {
            _config = config;
        }
        
        [AfterScenario("single_purpose_keys")]
        public void AfterScenarioHook()
        {
            _singlePurposeKeysOrganizationFactory = null;
        }
        
        public enum SinglePurposeKeyFactoryType
        {
            Normal,
            EncryptionKeyToSign,
            NoEncryptionKey
        }
        
        public void CreateSinglePurposeKeyFactory(SinglePurposeKeyFactoryType factoryType = SinglePurposeKeyFactoryType.Normal)
        {
            var idPath = Path.Combine("Secrets", "OrgId.txt");
            if (!File.Exists(idPath))
                throw new Exception($"Test configuration is invalid -- files with secrets should exist: {idPath}");

            var orgId = File.ReadAllText(idPath);
            var factoryFactoryBuilder = new FactoryFactoryBuilder();
            var apiUrlPath = Path.Combine("Secrets", "ApiUrl.txt");
            if (File.Exists(apiUrlPath))
            {
                var apiUrl = File.ReadAllText(apiUrlPath);
                factoryFactoryBuilder.SetApiBaseUrl(apiUrl);
            }
            var factoryFactory = factoryFactoryBuilder.Build();
            
            var encryptionKeyPath = Path.Combine("Secrets", "OrgEncryptionKey.txt");
            var signatureKeyPath = Path.Combine("Secrets", "OrgSignatureKey.txt");
            if (!File.Exists(encryptionKeyPath) || !File.Exists(signatureKeyPath))
                throw new Exception($"Test configuration is invalid -- files with secrets should exist: {encryptionKeyPath}, {signatureKeyPath}");
            var encryptionKeyContents = File.ReadAllText(encryptionKeyPath);
            var signatureKeyContents = File.ReadAllText(signatureKeyPath);
            var singlePurposeKeysList = new List<string> {signatureKeyContents, encryptionKeyContents};
            var crypto = new BouncyCastleCrypto();
            var parsedRsaKey = crypto.LoadRsaPrivateKey(signatureKeyContents);
            var signatureKeyId = "";
            
            switch (factoryType)
            {
                case SinglePurposeKeyFactoryType.NoEncryptionKey:
                    signatureKeyId = crypto.GeneratePublicKeyFingerprintFromPrivateKey(parsedRsaKey);
                    singlePurposeKeysList.Clear();
                    singlePurposeKeysList.Add(signatureKeyContents);
                    break;
                case SinglePurposeKeyFactoryType.EncryptionKeyToSign:
                    var parsedEncryptionRsaKey = crypto.LoadRsaPrivateKey(encryptionKeyContents);
                    signatureKeyId = crypto.GeneratePublicKeyFingerprintFromPrivateKey(parsedEncryptionRsaKey);
                    break;
                default: // SinglePurposeKeyFactoryType.Normal
                    signatureKeyId = crypto.GeneratePublicKeyFingerprintFromPrivateKey(parsedRsaKey);
                    break;
            }
            
            _singlePurposeKeysOrganizationFactory = factoryFactory.MakeOrganizationFactory(orgId, singlePurposeKeysList, signatureKeyId);

        }

        public void PerformAPICall()
        {
            var client = _singlePurposeKeysOrganizationFactory.MakeOrganizationClient();
            var directory = client.CreateDirectory(Util.UniqueDirectoryName());
            // Load directory for deactivation if successful
            _ownedDirectories.Add(client.GetDirectory(directory));
        }
        
        public void Dispose()
        {
            CreateSinglePurposeKeyFactory();
            var orgClient = _singlePurposeKeysOrganizationFactory.MakeOrganizationClient();
            foreach (var ownedDirectory in _ownedDirectories)
            {
                try
                {
                    orgClient.UpdateDirectory(ownedDirectory.Id, false, null, null, true, null);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error while deactivating directory: {e}");
                }
            }
        }
    }
}
