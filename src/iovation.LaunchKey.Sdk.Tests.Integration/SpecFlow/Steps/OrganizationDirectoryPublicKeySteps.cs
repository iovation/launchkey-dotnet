using System;
using System.Linq;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using iovation.LaunchKey.Sdk.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public class OrganizationDirectoryPublicKeySteps
    {
        private readonly CommonContext _commonContext;
        private readonly OrgClientContext _orgClientContext;
        private readonly KeyManager _keyManager;

        public OrganizationDirectoryPublicKeySteps(CommonContext commonContext, OrgClientContext orgClientContext, KeyManager keyManager)
        {
            _commonContext = commonContext;
            _orgClientContext = orgClientContext;
            _keyManager = keyManager;
        }

        [When(@"I add a Public Key to the Directory")]
        [Given(@"I added a Public Key to the Directory")]
        public void WhenIAddAPublicKeyToTheDirectory()
        {
            _orgClientContext.AddDirectoryPublicKey(
                _orgClientContext.LastCreatedDirectory.Id,
                _keyManager.GetAlphaPublicKey(),
                true,
                null
            );
        }

        [Given(@"I added another Public Key to the Directory")]
        [When(@"I add another Public Key to the Directory")]
        public void WhenIAddAnotherPublicKeyToTheDirectory()
        {
            _orgClientContext.AddDirectoryPublicKey(
                _orgClientContext.LastCreatedDirectory.Id,
                _keyManager.GetBetaPublicKey(),
                true,
                null
            );
        }

        [When(@"I retrieve the current Directory's Public Keys")]
        public void WhenIRetrieveTheCurrentDirectorySPublicKeys()
        {
            _orgClientContext.LoadDirectoryPublicKeys(
                _orgClientContext.LastCreatedDirectory.Id
            );
        }


        [Then(@"the Public Key is in the list of Public Keys for the Directory")]
        public void ThenThePublicKeyIsInTheListOfPublicKeysForTheDirectory()
        {
            Assert.IsTrue(
                _orgClientContext.LoadedDirectoryPublicKeys.Count(
                    x => x.Id == _orgClientContext.AddedDirectoryPublicKeys[0]
                ) == 1
            );
        }

        [Then(@"the other Public Key is in the list of Public Keys for the Directory")]
        public void ThenTheOtherPublicKeyIsInTheListOfPublicKeysForTheDirectory()
        {
            Assert.IsTrue(
                _orgClientContext.LoadedDirectoryPublicKeys.Count(
                    x => x.Id == _orgClientContext.AddedDirectoryPublicKeys[1]
                ) == 1
            );
        }

        [Then(@"the Public Key is in the list of Public Keys for the Directory and has a (BOTH|ENCRYPTION|SIGNATURE) key type")]
        public void ThenThePublicKeyIsInTheListOfPublicKeysForTheDirectoryAndHasAKeyType(string keyType)
        {
            Assert.IsTrue(_orgClientContext.LoadedDirectoryPublicKeys.Any(
                e => e.Id == _orgClientContext.AddedDirectoryPublicKeys[0]
            ));
            KeyType parsedKeyType;
            Enum.TryParse(keyType, true, out parsedKeyType);
            Assert.IsTrue(_orgClientContext.LoadedDirectoryPublicKeys[0].KeyType == parsedKeyType);
        }

        [Then(@"the Public Key is in the list of Public Keys for the Directory and has a ""(BOTH|ENCRYPTION|SIGNATURE)"" key type")]
        public void ThenThePublicKeyIsInTheListOfPublicKeysForTheDirectoryAndHasAQuotedKeyType(string keyType)
        {
            ThenThePublicKeyIsInTheListOfPublicKeysForTheDirectoryAndHasAKeyType(keyType);
        }

        [When(@"I attempt to add a Public Key to the Directory with the ID ""(.*)""")]
        public void WhenIAttemptToAddAPublicKeyToTheDirectoryWithTheID(string directoryId)
        {
            try
            {
                _orgClientContext.AddDirectoryPublicKey(
                    Guid.Parse(directoryId),
                    _keyManager.GetAlphaPublicKey(),
                    true,
                    null
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to add the same Public Key to the Directory")]
        public void WhenIAttemptToAddTheSamePublicKeyToTheDirectory()
        {
            try
            {
                _orgClientContext.AddDirectoryPublicKey(
                    _orgClientContext.LastCreatedDirectory.Id,
                    _keyManager.GetAlphaPublicKey(),
                    true,
                    null
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Then(@"the Directory Public Keys list is empty")]
        public void ThenTheDirectoryPublicKeysListIsEmpty()
        {
            Assert.IsTrue(
                _orgClientContext.LoadedDirectoryPublicKeys.Count == 0
            );
        }

        [Given(@"I added a Public Key to the Directory which is (active|inactive) and expires on ""(.*)""")]
        public void GivenIAddedAPublicKeyToTheDirectoryWhichIsInactiveAndExpiresOn(string activeStatus, DateTime expires)
        {
            var active = activeStatus == "active";
            _orgClientContext.AddDirectoryPublicKey(
                _orgClientContext.LastCreatedDirectory.Id,
                _keyManager.GetAlphaPublicKey(),
                active,
                expires
            );
        }

        [Then(@"the Directory Public Key is inactive")]
        public void ThenTheDirectoryPublicKeyIsInactive()
        {
            Assert.IsFalse(_orgClientContext.LoadedDirectoryPublicKeys[0].Active);
        }

        [Then(@"the Directory Public Key Expiration Date is ""(.*)""")]
        public void ThenTheDirectoryPublicKeyExpirationDateIs(DateTime expires)
        {
            Assert.AreEqual(
                expires.ToUniversalTime(),
                _orgClientContext.LoadedDirectoryPublicKeys[0].Expires
            );
        }

        [When(@"I attempt to retrieve the Public Keys for the Directory with the ID ""(.*)""")]
        public void WhenIAttemptToRetrieveThePublicKeysForTheDirectoryWithTheID(string directoryId)
        {
            try
            {
                _orgClientContext.LoadDirectoryPublicKeys(Guid.Parse(directoryId));
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I remove the current Directory Public Key")]
        public void WhenIRemoveTheCurrentDirectoryPublicKey()
        {
            _orgClientContext.RemoveDirectoryPublicKey(
                _orgClientContext.LastCreatedDirectory.Id,
                _orgClientContext.AddedDirectoryPublicKeys.Last()
            );
        }

        [Then(@"the last current Directory's Public Key is not in the list")]
        public void ThenTheLastCurrentDirectorySPublicKeyIsNotInTheList()
        {
            Assert.IsTrue(
                _orgClientContext.LoadedDirectoryPublicKeys.Count(
                    e => e.Id == _orgClientContext.AddedDirectoryPublicKeys.Last()
                ) == 0
            );
        }

        [When(@"I attempt to remove the current Directory Public Key")]
        public void WhenIAttemptToRemoveTheCurrentDirectoryPublicKey()
        {
            try
            {
                _orgClientContext.RemoveDirectoryPublicKey(
                    _orgClientContext.LastCreatedDirectory.Id,
                    _orgClientContext.AddedDirectoryPublicKeys[0]
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to remove a Public Key from the Directory with the ID ""(.*)""")]
        public void WhenIAttemptToRemoveAPublicKeyFromTheDirectoryWithTheID(string directoryId)
        {
            try
            {
                _orgClientContext.RemoveDirectoryPublicKey(
                    Guid.Parse(directoryId),
                    _orgClientContext.AddedDirectoryPublicKeys.Last()
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to remove a Public Key identified by ""(.*)"" from the Directory")]
        public void WhenIAttemptToRemoveAPublicKeyIdentifiedByFromTheDirectory(string keyId)
        {
            try
            {
                _orgClientContext.RemoveDirectoryPublicKey(
                    _orgClientContext.LastCreatedDirectory.Id,
                    keyId
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I update the Directory Public Key to inactive")]
        public void WhenIUpdateTheDirectoryPublicKeyToInactive()
        {
            _orgClientContext.DeactivateDirectoryPublicKey(
                _orgClientContext.LastCreatedDirectory.Id,
                _orgClientContext.AddedDirectoryPublicKeys[0]
            );
        }

        [When(@"I updated the Directory Public Key expiration date to ""(.*)""")]
        public void WhenIUpdatedTheDirectoryPublicKeyExpirationDateTo(DateTime expires)
        {
            _orgClientContext.UpdateDirectoryPublicKeyExpires(
                _orgClientContext.LastCreatedDirectory.Id,
                _orgClientContext.AddedDirectoryPublicKeys[0],
                expires
            );
        }

        [When(@"I attempt to update a Public Key for the Directory with the ID ""(.*)""")]
        public void WhenIAttemptToUpdateAPublicKeyForTheDirectoryWithTheID(string directoryId)
        {
            try
            {
                _orgClientContext.UpdateDirectoryPublicKey(
                    Guid.Parse(directoryId),
                    _orgClientContext.AddedDirectoryPublicKeys[0],
                    true,
                    null
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to update a Public Key identified by ""(.*)"" for the Directory")]
        public void WhenIAttemptToUpdateAPublicKeyIdentifiedByForTheDirectory(string keyId)
        {
            try
            {
                _orgClientContext.UpdateDirectoryPublicKey(
                    _orgClientContext.LastCreatedDirectory.Id,
                    keyId,
                    true,
                    null
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I add a Public Key with a (BOTH|ENCRYPTION|SIGNATURE) type to the Directory")]
        public void WhenIAddAPublicKeyWithAKeyTypeTypeToTheDirectory(string keyType)
        {
            try
            {
                KeyType parsedKeyType;
                Enum.TryParse(keyType, true, out parsedKeyType);
                _orgClientContext.AddDirectoryPublicKey(
                    _orgClientContext.LastCreatedDirectory.Id,
                    _keyManager.GetAlphaPublicKey(),
                    true,
                    new DateTime(2020, 1, 1),
                    parsedKeyType
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to add a Public Key with a ""(.*)"" type to the Directory")]
        public void WhenIAttemptToAddAPublicKeyWithATypeToTheDirectory(string keyType)
        {
            try
            {
                KeyType parsedKeyType;
                if (!Enum.TryParse(keyType, true, out parsedKeyType))
                {
                    parsedKeyType = KeyType.OTHER;
                }
                _orgClientContext.AddDirectoryPublicKey(
                    _orgClientContext.LastCreatedDirectory.Id,
                    _keyManager.GetAlphaPublicKey(),
                    true,
                    new DateTime(2020, 1, 1),
                    parsedKeyType
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }
    }
}
