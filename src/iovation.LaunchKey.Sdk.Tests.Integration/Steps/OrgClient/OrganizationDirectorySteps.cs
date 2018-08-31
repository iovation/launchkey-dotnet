using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Organization;
using iovation.LaunchKey.Sdk.Error;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps.OrgClient
{
	[Binding]
    public class OrganizationDirectorySteps
    {
		private readonly TestConfiguration _config;
		private readonly CommonContext _commonContext;
		private readonly KeyManager _keyManager;
		private readonly IOrganizationClient _orgClient;

		private List<CreatedDirectoryInfo> _ownedDirectories = new List<CreatedDirectoryInfo>();
		private Directory _lastRetrievedDirectory = null;
		private List<Directory> _lastGetAllDirectories = null;
		private List<Directory> _lastGetDirectoriesResponse = null;
		private List<Guid> _addedSdkKeys = new List<Guid>();

		public OrganizationDirectorySteps(TestConfiguration config, CommonContext commonContext, KeyManager keyManager)
		{
			_config = config;
			_commonContext = commonContext;
			_keyManager = keyManager;
			_orgClient = config.GetOrgClient();
		}

		private void CreateDirectory(string name)
		{
			var id = _orgClient.CreateDirectory(name);
			_ownedDirectories.Add(new CreatedDirectoryInfo(id, name));
		}

		private void CreateUniquelyNamedDirectory()
		{
			CreateDirectory(Util.UniqueName("Directory"));
		}

		[When(@"I create a Directory with a unique name")]
		public void WhenICreateADirectoryWithAUniqueName()
		{
			CreateUniquelyNamedDirectory();
		}
		
		[Then(@"the Directory name is the same as was sent")]
		public void ThenTheDirectoryNameIsTheSameAsWasSent()
		{
			Assert.AreEqual(_ownedDirectories[0].Name, _lastRetrievedDirectory.Name);
		}

		[Given(@"I created a Directory")]
		public void GivenICreatedADirectory()
		{
			CreateUniquelyNamedDirectory();
		}

		[Given(@"I attempt to create a Directory with the same name")]
		public void GivenIAttemptToCreateADirectoryWithTheSameName()
		{
			try
			{
				CreateDirectory(_ownedDirectories[0].Name);
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
			var directory = _orgClient.GetDirectory(_ownedDirectories.Last().Id);
			_orgClient.UpdateDirectory(directory.Id, active, directory.AndroidKey, directory.IosCertificateFingerprint);
		}

		[When(@"I retrieve the (?:created|updated|current) Directory")]
		public void WhenIRetrieveTheUpdatedDirectory()
		{
			_lastRetrievedDirectory = _orgClient.GetDirectory(_ownedDirectories.Last().Id);
		}

		[Then(@"the Directory is (not active|active)")]
		public void ThenTheDirectoryIsNotActive(string activeStatus)
		{
			var shouldBeActive = activeStatus == "active";
			Assert.AreEqual(shouldBeActive, _lastRetrievedDirectory.Active);
		}

		[When(@"I updated? the Directory Android Key with ""(.*)""")]
		[Given(@"I updated? the Directory Android Key with ""(.*)""")]
		public void WhenIUpdateTheDirectoryAndroidKeyWith(string androidKey)
		{
			var directory = _orgClient.GetDirectory(_ownedDirectories.Last().Id);
			_orgClient.UpdateDirectory(directory.Id, directory.Active, androidKey, directory.IosCertificateFingerprint);
		}

		[Then(@"the Directory Android Key is ""(.*)""")]
		public void ThenTheDirectoryAndroidKeyIs(string androidKey)
		{
			Assert.AreEqual(androidKey, _lastRetrievedDirectory.AndroidKey);
		}

		[When(@"I updated? the Directory Android Key with null")]
		public void WhenIUpdateTheDirectoryAndroidKeyWithNull()
		{
			var directory = _orgClient.GetDirectory(_ownedDirectories.Last().Id);
			_orgClient.UpdateDirectory(directory.Id, directory.Active, null, directory.IosCertificateFingerprint);
		}

		[Then(@"the Directory has no Android Key")]
		public void ThenTheDirectoryHasNoAndroidKey()
		{
			Assert.IsNull(_lastRetrievedDirectory.AndroidKey);
		}

		[When(@"I updated? the Directory iOS P12 with a valid certificate")]
		[Given(@"I updated? the Directory iOS P12 with a valid certificate")]
		public void WhenIUpdateTheDirectoryIOSPWithAValidCertificate()
		{
			var directory = _orgClient.GetDirectory(_ownedDirectories.Last().Id);
			_orgClient.UpdateDirectory(directory.Id, directory.Active, directory.AndroidKey, _keyManager.GetBase64EncodedAlphaP12());
		}

		[Then(@"Directory the iOS Certificate Fingerprint matches the provided certificate")]
		public void ThenDirectoryTheIOSCertificateFingerprintMatchesTheProvidedCertificate()
		{
			Assert.AreEqual(_keyManager.GetAlphaCertificateFingerprint(), _lastRetrievedDirectory.IosCertificateFingerprint);
		}

		[When(@"I update the Directory iOS P12 with null")]
		public void WhenIUpdateTheDirectoryIOSPWithNull()
		{
			var directory = _orgClient.GetDirectory(_ownedDirectories.Last().Id);
			_orgClient.UpdateDirectory(directory.Id, directory.Active, directory.AndroidKey, null);
		}

		[Then(@"the Directory has no IOS Certificate Fingerprint")]
		public void ThenTheDirectoryHasNoIOSCertificateFingerprint()
		{
			Assert.IsNull(_lastRetrievedDirectory.IosCertificateFingerprint);
		}

		[When(@"I attempt to update the active status of the Directory with the ID ""(.*)""")]
		public void WhenIAttemptToUpdateTheActiveStatusOfTheDirectoryWithTheID(string directoryId)
		{
			try
			{
				_orgClient.UpdateDirectory(Guid.Parse(directoryId), true, null, null);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[Given(@"I generated and added (.*) SDK Keys? to the Directory")]
		public void GivenIGeneratedAndAddedSDKKeysToTheDirectory(int numKeys)
		{
			var lastDirectory = _ownedDirectories[0].Id;
			_addedSdkKeys = new List<Guid>();
			for (var i = 0; i < numKeys; i++)
			{
				_addedSdkKeys.Add(_orgClient.GenerateAndAddDirectorySdkKey(lastDirectory));
			}
		}

		[When(@"I retrieve a list of all Directories")]
		public void WhenIRetrieveAListOfAllDirectories()
		{
			_lastGetAllDirectories = _orgClient.GetAllDirectories();
		}

		[Then(@"the current Directory is in the Directory list")]
		public void ThenTheCurrentDirectoryIsInTheDirectoryList()
		{
			Assert.IsTrue(_lastGetAllDirectories.Any(e => e.Id == _ownedDirectories.Last().Id));
		}

		[Given(@"I added (.*) Services to the Directory")]
		public void GivenIAddedServicesToTheDirectory(int p0)
		{
			// todo implement when i have service management
		}

		[When(@"I retrieve a list of Directories with the created Directory's ID")]
		public void WhenIRetrieveAListOfDirectoriesWithTheCreatedDirectorySID()
		{
			var lastDir = _ownedDirectories.Last().Id;
			_lastGetDirectoriesResponse = _orgClient.GetDirectories(new List<Guid> {lastDir});
		}

		[Then(@"the current Directory list is a list with only the current Directory")]
		public void ThenTheCurrentDirectoryListIsAListWithOnlyTheCurrentDirectory()
		{
			var lastDir = _ownedDirectories.Last().Id;
			Assert.IsTrue(_lastGetDirectoriesResponse.Count == 1);
			Assert.IsTrue(_lastGetDirectoriesResponse[0].Id == lastDir);
		}

		[When(@"I attempt retrieve a list of Directories with the Directory ID ""(.*)""")]
		public void WhenIAttemptRetrieveAListOfDirectoriesWithTheDirectoryID(string directoryId)
		{
			try
			{
				_lastGetDirectoriesResponse = _orgClient.GetDirectories(new List<Guid> {Guid.Parse(directoryId)});
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[Then(@"the ID matches the value returned when the Directory was created")]
		public void ThenTheIDMatchesTheValueReturnedWhenTheDirectoryWasCreated()
		{
			var lastDir = _ownedDirectories.Last().Id;
			Assert.AreEqual(lastDir, _lastRetrievedDirectory.Id);
		}

		[Then(@"the Directory has the added SDK Key")]
		public void ThenTheDirectoryHasTheAddedSDKKey()
		{
			foreach (var key in _addedSdkKeys)
			{
				Assert.IsTrue(_lastRetrievedDirectory.SdkKeys.Contains(key));
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
				_orgClient.GetDirectory(Guid.Parse(directoryId));
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I generate and add an SDK Key to the Directory")]
		public void WhenIGenerateAndAddAnSDKKeyToTheDirectory()
		{
			_addedSdkKeys = new List<Guid>
			{
				_orgClient.GenerateAndAddDirectorySdkKey(_ownedDirectories[0].Id)
			};
		}

		[Then(@"the SDK Key is in the list for the Directory")]
		public void ThenTheSDKKeyIsInTheListForTheDirectory()
		{
			Assert.IsTrue(_lastRetrievedDirectory.SdkKeys.Contains(_addedSdkKeys[0]));
		}

		[When(@"I attempt to generate and add an SDK Key to the Directory with the ID ""(.*)""")]
		public void WhenIAttemptToGenerateAndAddAnSDKKeyToTheDirectoryWithTheID(string directoryId)
		{
			try
			{
				_orgClient.GenerateAndAddDirectorySdkKey(Guid.Parse(directoryId));
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		private List<Guid> _lastRetrievedDirectorySdkKeys = null;

		[When(@"I retrieve the current Directory's SDK Keys")]
		public void WhenIRetrieveTheCurrentDirectorySSDKKeys()
		{
			var lastDir = _ownedDirectories.Last().Id;
			_lastRetrievedDirectorySdkKeys = _orgClient.GetAllDirectorySdkKeys(lastDir);
		}

		[Then(@"all of the SDK Keys for the Directory are in the SDK Keys list")]
		public void ThenAllOfTheSDKKeysForTheDirectoryAreInTheSDKKeysList()
		{
			Assert.AreEqual(1, _lastRetrievedDirectorySdkKeys.Count);
			Assert.AreEqual(_addedSdkKeys[0], _lastRetrievedDirectorySdkKeys[0]);
		}

		[When(@"I attempt to retrieve the current Directory SDK Keys for the Directory with the ID ""(.*)""")]
		public void WhenIAttemptToRetrieveTheCurrentDirectorySDKKeysForTheDirectoryWithTheID(string directoryId)
		{
			try
			{
				_orgClient.GetAllDirectorySdkKeys(Guid.Parse(directoryId));
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I remove the last generated SDK Key from the Directory")]
		public void WhenIRemoveTheLastGeneratedSDKKeyFromTheDirectory()
		{
			var lastDir = _ownedDirectories.Last().Id;
			var lastKeyAdded = _addedSdkKeys.Last();
			_orgClient.RemoveDirectorySdkKey(lastDir, lastKeyAdded);
		}

		[Then(@"the last generated SDK Key is not in the list for the Directory")]
		public void ThenTheLastGeneratedSDKKeyIsNotInTheListForTheDirectory()
		{
			var lastKeyAdded = _addedSdkKeys.Last();
			Assert.IsFalse(_lastRetrievedDirectorySdkKeys.Contains(lastKeyAdded));
		}

		[When(@"I attempt to remove the last generated SDK Key from the Directory with the ID ""(.*)""")]
		public void WhenIAttemptToRemoveTheLastGeneratedSDKKeyFromTheDirectoryWithTheID(string directoryId)
		{
			try
			{
				_orgClient.RemoveDirectorySdkKey(Guid.Parse(directoryId), Guid.NewGuid());
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I attempt to remove the last generated SDK Key ""(.*)"" from the Directory")]
		public void WhenIAttemptToRemoveTheLastGeneratedSDKKeyFromTheDirectory(string sdkKeyId)
		{
			var directoryId = _ownedDirectories[0].Id;
			try
			{
				_orgClient.RemoveDirectorySdkKey(directoryId, Guid.Parse(sdkKeyId));
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I attempt to remove the last generated SDK Key from the Directory")]
		public void WhenIAttemptToRemoveTheLastGeneratedSDKKeyFromTheDirectory()
		{
			var directoryId = _ownedDirectories[0].Id;
			try
			{
				_orgClient.RemoveDirectorySdkKey(directoryId, _addedSdkKeys[0]);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

	}
}
