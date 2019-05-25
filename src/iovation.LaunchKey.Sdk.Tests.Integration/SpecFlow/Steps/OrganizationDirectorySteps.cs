using System;
using System.Collections.Generic;
using System.Linq;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public class OrganizationDirectorySteps
    {
        private readonly TestConfiguration _config;
        private readonly CommonContext _commonContext;
        private readonly KeyManager _keyManager;
        private readonly OrgClientContext _orgClientContext;

        public OrganizationDirectorySteps(TestConfiguration config, CommonContext commonContext, KeyManager keyManager, OrgClientContext orgClientContext)
        {
            _config = config;
            _commonContext = commonContext;
            _keyManager = keyManager;
            _orgClientContext = orgClientContext;
        }

        [When(@"I create a Directory with a unique name")]
        public void WhenICreateADirectoryWithAUniqueName()
        {
            _orgClientContext.CreateDirectory(Util.UniqueDirectoryName());
        }

        [Then(@"the Directory name is the same as was sent")]
        public void ThenTheDirectoryNameIsTheSameAsWasSent()
        {
            Assert.AreEqual(_orgClientContext.LastCreatedDirectory.Name, _orgClientContext.LoadedDirectory.Name);
        }

        [Given(@"I created a Directory")]
        public void GivenICreatedADirectory()
        {
            WhenICreateADirectoryWithAUniqueName();
        }

        [Given(@"I attempt to create a Directory with the same name")]
        public void GivenIAttemptToCreateADirectoryWithTheSameName()
        {
            try
            {
                _orgClientContext.CreateDirectory(_orgClientContext.LastCreatedDirectory.Name);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I updated? the Directory as (inactive|active)")]
        [Given(@"I updated? the Directory as (inactive|active)")]
        public void WhenIUpdateTheDirectoryAsInactive(string activeStatus)
        {
            var active = activeStatus == "active";
            _orgClientContext.LoadLastCreatedDirectory();
            var directory = _orgClientContext.LoadedDirectory;
            _orgClientContext.UpdateDirectory(
                directory.Id,
                active,
                directory.AndroidKey,
                _keyManager.GetP12ForFingerprint(directory.IosCertificateFingerprint)
            );
        }

        [When(@"I retrieve the (?:created|updated|current) Directory")]
        public void WhenIRetrieveTheUpdatedDirectory()
        {
            _orgClientContext.LoadLastCreatedDirectory();
        }

        [Then(@"the Directory is (not active|active)")]
        public void ThenTheDirectoryIsNotActive(string activeStatus)
        {
            var shouldBeActive = activeStatus == "active";
            Assert.AreEqual(shouldBeActive, _orgClientContext.LoadedDirectory.Active);
        }

        [When(@"I updated? the Directory Android Key with ""(.*)""")]
        [Given(@"I updated? the Directory Android Key with ""(.*)""")]
        public void WhenIUpdateTheDirectoryAndroidKeyWith(string androidKey)
        {
            _orgClientContext.LoadLastCreatedDirectory();
            _orgClientContext.UpdateDirectory(
                _orgClientContext.LoadedDirectory.Id,
                _orgClientContext.LoadedDirectory.Active,
                androidKey,
                _keyManager.GetP12ForFingerprint(_orgClientContext.LoadedDirectory.IosCertificateFingerprint)
            );
        }

        [Then(@"the Directory Android Key is ""(.*)""")]
        public void ThenTheDirectoryAndroidKeyIs(string androidKey)
        {
            Assert.AreEqual(androidKey, _orgClientContext.LoadedDirectory.AndroidKey);
        }

        [When(@"I updated? the Directory Android Key with null")]
        public void WhenIUpdateTheDirectoryAndroidKeyWithNull()
        {
            _orgClientContext.LoadLastCreatedDirectory();
            _orgClientContext.UpdateDirectory(
                _orgClientContext.LoadedDirectory.Id,
                _orgClientContext.LoadedDirectory.Active,
                null,
                _keyManager.GetP12ForFingerprint(_orgClientContext.LoadedDirectory.IosCertificateFingerprint)
            );
        }

        [Then(@"the Directory has no Android Key")]
        public void ThenTheDirectoryHasNoAndroidKey()
        {
            Assert.IsNull(_orgClientContext.LoadedDirectory.AndroidKey);
        }

        [When(@"I updated? the Directory iOS P12 with a valid certificate")]
        [Given(@"I updated? the Directory iOS P12 with a valid certificate")]
        public void WhenIUpdateTheDirectoryIOSPWithAValidCertificate()
        {
            _orgClientContext.LoadLastCreatedDirectory();
            _orgClientContext.UpdateDirectory(
                _orgClientContext.LoadedDirectory.Id,
                _orgClientContext.LoadedDirectory.Active,
                _orgClientContext.LoadedDirectory.AndroidKey,
                _keyManager.GetBase64EncodedAlphaP12()
            );
        }

        [Then(@"Directory the iOS Certificate Fingerprint matches the provided certificate")]
        public void ThenDirectoryTheIOSCertificateFingerprintMatchesTheProvidedCertificate()
        {
            Assert.AreEqual(_keyManager.GetAlphaCertificateFingerprint(), _orgClientContext.LoadedDirectory.IosCertificateFingerprint);
        }

        [When(@"I update the Directory iOS P12 with null")]
        public void WhenIUpdateTheDirectoryIOSPWithNull()
        {
            _orgClientContext.LoadLastCreatedDirectory();
            _orgClientContext.UpdateDirectory(
                _orgClientContext.LoadedDirectory.Id,
                _orgClientContext.LoadedDirectory.Active,
                _orgClientContext.LoadedDirectory.AndroidKey,
                null
            );
        }

        [Then(@"the Directory has no IOS Certificate Fingerprint")]
        public void ThenTheDirectoryHasNoIOSCertificateFingerprint()
        {
            Assert.IsNull(_orgClientContext.LoadedDirectory.IosCertificateFingerprint);
        }

        [When(@"I attempt to update the active status of the Directory with the ID ""(.*)""")]
        public void WhenIAttemptToUpdateTheActiveStatusOfTheDirectoryWithTheID(string directoryId)
        {
            try
            {
                _orgClientContext.UpdateDirectory(Guid.Parse(directoryId), true, null, null, null);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Given(@"I generated and added (.*) SDK Keys? to the Directory")]
        public void GivenIGeneratedAndAddedSDKKeysToTheDirectory(int numKeys)
        {
            _orgClientContext.GenerateDirectorySDKKeys(
                _orgClientContext.LastCreatedDirectory.Id,
                numKeys
            );
        }

        [When(@"I retrieve a list of all Directories")]
        public void WhenIRetrieveAListOfAllDirectories()
        {
            _orgClientContext.LoadAllDirectories();
        }

        [Then(@"the current Directory is in the Directory list")]
        public void ThenTheCurrentDirectoryIsInTheDirectoryList()
        {
            Assert.IsTrue(_orgClientContext.LoadedDirectories.Any(e => e.Id == _orgClientContext.LastCreatedDirectory.Id));
        }

        [Given(@"I added (.*) Services to the Directory")]
        public void GivenIAddedServicesToTheDirectory(int p0)
        {
            // todo implement when i have service management
        }

        [When(@"I retrieve a list of Directories with the created Directory's ID")]
        public void WhenIRetrieveAListOfDirectoriesWithTheCreatedDirectorySID()
        {
            _orgClientContext.LoadDirectories(new List<Guid> { _orgClientContext.LastCreatedDirectory.Id });
        }

        [Then(@"the current Directory list is a list with only the current Directory")]
        public void ThenTheCurrentDirectoryListIsAListWithOnlyTheCurrentDirectory()
        {
            Assert.IsTrue(_orgClientContext.LoadedDirectories.Count == 1);
            Assert.IsTrue(_orgClientContext.LoadedDirectories[0].Id == _orgClientContext.LastCreatedDirectory.Id);
        }

        [When(@"I attempt retrieve a list of Directories with the Directory ID ""(.*)""")]
        public void WhenIAttemptRetrieveAListOfDirectoriesWithTheDirectoryID(string directoryId)
        {
            try
            {
                _orgClientContext.LoadDirectories(new List<Guid> { Guid.Parse(directoryId) });
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Then(@"the ID matches the value returned when the Directory was created")]
        public void ThenTheIDMatchesTheValueReturnedWhenTheDirectoryWasCreated()
        {
            Assert.AreEqual(_orgClientContext.LastCreatedDirectory.Id, _orgClientContext.LoadedDirectory.Id);
        }

        [Then(@"the Directory has the added SDK Key")]
        public void ThenTheDirectoryHasTheAddedSDKKey()
        {
            foreach (var key in _orgClientContext.AddedSdkKeys)
            {
                Assert.IsTrue(_orgClientContext.LoadedDirectory.SdkKeys.Contains(key));
            }
        }

        [Then(@"the Directory has the added Service IDs")]
        public void ThenTheDirectoryHasTheAddedServiceIDs()
        {
            // todo implement when i have dir services
        }

        [When(@"I attempt retrieve the Directory identified by ""(.*)""")]
        public void WhenIAttemptRetrieveTheDirectoryIdentifiedBy(string directoryId)
        {
            try
            {
                _orgClientContext.LoadDirectory(Guid.Parse(directoryId));
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Given(@"I have added an SDK Key to the Directory")]
        [When(@"I generate and add an SDK Key to the Directory")]
        public void WhenIGenerateAndAddAnSDKKeyToTheDirectory()
        {
            _orgClientContext.GenerateDirectorySDKKeys(
                _orgClientContext.LastCreatedDirectory.Id,
                1
            );
        }

        [Then(@"the SDK Key is in the list for the Directory")]
        public void ThenTheSDKKeyIsInTheListForTheDirectory()
        {
            Assert.IsTrue(_orgClientContext.LoadedDirectory.SdkKeys.Contains(_orgClientContext.AddedSdkKeys[0]));
        }

        [When(@"I attempt to generate and add an SDK Key to the Directory with the ID ""(.*)""")]
        public void WhenIAttemptToGenerateAndAddAnSDKKeyToTheDirectoryWithTheID(string directoryId)
        {
            try
            {
                _orgClientContext.GenerateDirectorySDKKeys(Guid.Parse(directoryId), 1);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I retrieve the current Directory's SDK Keys")]
        public void WhenIRetrieveTheCurrentDirectorySSDKKeys()
        {
            _orgClientContext.LoadSdkKeys(
                _orgClientContext.LastCreatedDirectory.Id
            );
        }

        [Then(@"all of the SDK Keys for the Directory are in the SDK Keys list")]
        public void ThenAllOfTheSDKKeysForTheDirectoryAreInTheSDKKeysList()
        {
            Assert.AreEqual(1, _orgClientContext.LoadedSdkKeys.Count);
            Assert.AreEqual(_orgClientContext.AddedSdkKeys[0], _orgClientContext.LoadedSdkKeys[0]);
        }

        [When(@"I attempt to retrieve the current Directory SDK Keys for the Directory with the ID ""(.*)""")]
        public void WhenIAttemptToRetrieveTheCurrentDirectorySDKKeysForTheDirectoryWithTheID(string directoryId)
        {
            try
            {
                _orgClientContext.LoadSdkKeys(Guid.Parse(directoryId));
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I remove the last generated SDK Key from the Directory")]
        public void WhenIRemoveTheLastGeneratedSDKKeyFromTheDirectory()
        {
            _orgClientContext.RemoveDirectorySdkKey(
                _orgClientContext.LastCreatedDirectory.Id,
                _orgClientContext.AddedSdkKeys.Last()
            );
        }

        [Then(@"the last generated SDK Key is not in the list for the Directory")]
        public void ThenTheLastGeneratedSDKKeyIsNotInTheListForTheDirectory()
        {
            Assert.IsFalse(_orgClientContext.LoadedSdkKeys.Contains(_orgClientContext.AddedSdkKeys.Last()));
        }

        [When(@"I attempt to remove the last generated SDK Key from the Directory with the ID ""(.*)""")]
        public void WhenIAttemptToRemoveTheLastGeneratedSDKKeyFromTheDirectoryWithTheID(string directoryId)
        {
            try
            {
                _orgClientContext.RemoveDirectorySdkKey(Guid.Parse(directoryId), Guid.NewGuid());
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to remove the last generated SDK Key ""(.*)"" from the Directory")]
        public void WhenIAttemptToRemoveTheLastGeneratedSDKKeyFromTheDirectory(string sdkKeyId)
        {
            var directoryId = _orgClientContext.LastCreatedDirectory.Id;
            try
            {
                _orgClientContext.RemoveDirectorySdkKey(directoryId, Guid.Parse(sdkKeyId));
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to remove the last generated SDK Key from the Directory")]
        public void WhenIAttemptToRemoveTheLastGeneratedSDKKeyFromTheDirectory()
        {
            var directoryId = _orgClientContext.LastCreatedDirectory.Id;
            try
            {
                _orgClientContext.RemoveDirectorySdkKey(directoryId, _orgClientContext.AddedSdkKeys.Last());
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Given(@"I updated? the Directory webhook url to ""(.*)""")]
        [When(@"I update the Directory webhook url to ""(.*)""")]
        public void GivenIUpdatedTheDirectoryWebhookUrl(string webhookUrl)
        {
            _orgClientContext.LoadLastCreatedDirectory();
            var directory = _orgClientContext.LoadedDirectory;
            _orgClientContext.UpdateDirectory(
                directory.Id,
                directory.Active,
                directory.AndroidKey,
                _keyManager.GetP12ForFingerprint(directory.IosCertificateFingerprint),
                webhookUrl
            );
        }

        [Then(@"the Directory webhook url is ""(.*)""")]
        public void ThenTheDirectoryWebhookUrlIs(string webhookUrl)
        {
            _orgClientContext.LoadLastCreatedDirectory();
            string directoryWebhookUrl = _orgClientContext.LoadedDirectory.WebhookUrl;
            Assert.AreEqual(webhookUrl, directoryWebhookUrl);
        }

        [When(@"I update the Directory webhook url to null")]
        public void WhenIUpdateTheDirectoryWebhookUrlToNull()
        {
            GivenIUpdatedTheDirectoryWebhookUrl("");
        }

        [Then(@"the Directory webhook url is empty")]
        public void ThenTheDirectoryWebhookUrlIsEmpty()
        {
            ThenTheDirectoryWebhookUrlIs(null);
        }
    }
}
