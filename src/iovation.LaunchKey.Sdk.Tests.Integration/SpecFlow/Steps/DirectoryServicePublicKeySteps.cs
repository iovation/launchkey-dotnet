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
    public class DirectoryServicePublicKeySteps
    {
        private readonly CommonContext _commonContext;
        private readonly DirectoryClientContext _directoryClientContext;
        private readonly KeyManager _keyManager;

        public DirectoryServicePublicKeySteps(CommonContext commonContext, DirectoryClientContext directoryClientContext, KeyManager keyManager)
        {
            _commonContext = commonContext;
            _directoryClientContext = directoryClientContext;
            _keyManager = keyManager;
        }

        [When(@"I add a Public Key to the Directory Service")]
        [Given(@"I added a Public Key to the Directory Service")]
        public void WhenIAddAPublicKeyToTheDirectoryService()
        {
            _directoryClientContext.AddServicePublicKey(
                _directoryClientContext.LastCreatedService.Id,
                _keyManager.GetAlphaPublicKey(),
                true,
                null
            );
        }

        [When(@"I add another Public Key to the Directory Service")]
        [Given(@"I added another Public Key to the Directory Service")]
        public void WhenIAddAnotherPublicKeyToTheDirectoryService()
        {
            _directoryClientContext.AddServicePublicKey(
                _directoryClientContext.LastCreatedService.Id,
                _keyManager.GetBetaPublicKey(),
                true,
                null
            );
        }

        [When(@"I retrieve the current Directory Service's Public Keys")]
        public void WhenIRetrieveTheCurrentDirectoryServiceSPublicKeys()
        {
            _directoryClientContext.LoadServicePublicKeys(_directoryClientContext.LastCreatedService.Id);
        }

        [Then(@"the Directory Service Public Key is in the list of Public Keys for the Directory Service")]
        public void ThenTheDirectoryServicePublicKeyIsInTheListOfPublicKeysForTheDirectoryService()
        {
            Assert.IsTrue(_directoryClientContext.LoadedServicePublicKeys.Any(
                e => e.Id == _directoryClientContext.AddedServicePublicKeys[0]
            ));
        }

        [Then(@"the other Public Key is in the list of Public Keys for the Directory Service")]
        public void ThenTheOtherPublicKeyIsInTheListOfPublicKeysForTheDirectoryService()
        {
            Assert.IsTrue(_directoryClientContext.LoadedServicePublicKeys.Any(
                e => e.Id == _directoryClientContext.AddedServicePublicKeys[1]
            ));
        }

        [When(@"I attempt to add a Public Key to the Directory Service with the ID ""(.*)""")]
        public void WhenIAttemptToAddAPublicKeyToTheDirectoryServiceWithTheID(string p0)
        {
            try
            {
                _directoryClientContext.AddServicePublicKey(Guid.Parse(p0), _keyManager.GetAlphaPublicKey(), true, null);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to add the same Public Key to the Directory Service")]
        public void WhenIAttemptToAddTheSamePublicKeyToTheDirectoryService()
        {
            try
            {
                _directoryClientContext.AddServicePublicKey(
                    _directoryClientContext.LastCreatedService.Id,
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

        [Then(@"the Directory Service Public Keys list is empty")]
        public void ThenTheDirectoryServicePublicKeysListIsEmpty()
        {
            Assert.IsTrue(_directoryClientContext.LoadedServicePublicKeys.Count == 0);
        }

        [Given(@"I added a Public Key to the Directory Service which is (active|inactive) and expires on ""(.*)""")]
        public void GivenIAddedAPublicKeyToTheDirectoryServiceWhichIsInactiveAndExpiresOn(string activeStatus, string expireString)
        {
            var active = activeStatus == "active";
            var expires = DateTime.Parse(expireString).ToUniversalTime();
            _directoryClientContext.AddServicePublicKey(
                _directoryClientContext.LastCreatedService.Id,
                _keyManager.GetAlphaPublicKey(),
                active,
                expires
            );
        }

        [Then(@"the Public Key is in the list of Public Keys for the Directory Service")]
        public void ThenThePublicKeyIsInTheListOfPublicKeysForTheDirectoryService()
        {
            Assert.IsTrue(_directoryClientContext.LoadedServicePublicKeys.Any(
                e => e.Id == _directoryClientContext.AddedServicePublicKeys[0]
            ));
        }

        [Then(@"the Public Key is in the list of Public Keys for the Directory Service and has a (BOTH|ENCRYPTION|SIGNATURE) key type")]
        public void ThenThePublicKeyIsInTheListOfPublicKeysForTheDirectoryServiceAndHasAKeyType(string keyType)
        {
            Assert.IsTrue(_directoryClientContext.LoadedServicePublicKeys.Any(
                e => e.Id == _directoryClientContext.AddedServicePublicKeys[0]
            ));
            KeyType parsedKeyType;
            Enum.TryParse(keyType, true, out parsedKeyType);
            Assert.IsTrue(_directoryClientContext.LoadedServicePublicKeys[0].KeyType == parsedKeyType);
        }

        [Then(@"the Public Key is in the list of Public Keys for the Directory Service and has a ""(BOTH|ENCRYPTION|SIGNATURE)"" key type")]
        public void ThenThePublicKeyIsInTheListOfPublicKeysForTheDirectoryServiceAndHasAQuotedKeyType(string keyType)
        {
            ThenThePublicKeyIsInTheListOfPublicKeysForTheDirectoryServiceAndHasAKeyType(keyType);
        }

        [Then(@"the Directory Service Public Key is inactive")]
        public void ThenTheDirectoryServicePublicKeyIsInactive()
        {
            Assert.IsFalse(_directoryClientContext.LoadedServicePublicKeys[0].Active);
        }

        [Then(@"the Directory Service Public Key Expiration Date is ""(.*)""")]
        public void ThenTheDirectoryServicePublicKeyExpirationDateIs(string expireString)
        {
            var expires = DateTime.Parse(expireString).ToUniversalTime();
            Assert.AreEqual(expires, _directoryClientContext.LoadedServicePublicKeys[0].Expires);
        }

        [When(@"I attempt to retrieve the Public Keys for the Directory Service with the Service ID ""(.*)""")]
        public void WhenIAttemptToRetrieveThePublicKeysForTheDirectoryServiceWithTheServiceID(string serviceId)
        {
            try
            {
                _directoryClientContext.LoadServicePublicKeys(Guid.Parse(serviceId));
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I remove the current Directory Service Public Key")]
        public void WhenIRemoveTheCurrentDirectoryServicePublicKey()
        {
            _directoryClientContext.RemoveServicePublicKey(
                _directoryClientContext.LastCreatedService.Id,
                _directoryClientContext.AddedServicePublicKeys.Last()
            );
        }

        [Then(@"the last current Directory Service's Public Key is not in the list")]
        public void ThenTheLastCurrentDirectoryServiceSPublicKeyIsNotInTheList()
        {
            Assert.IsFalse(
                _directoryClientContext
                    .LoadedServicePublicKeys
                    .Any(k => k.Id == _directoryClientContext.AddedServicePublicKeys.Last())
            );
        }

        [When(@"I attempt to remove the current Directory Service Public Key")]
        public void WhenIAttemptToRemoveTheCurrentDirectoryServicePublicKey()
        {
            try
            {
                _directoryClientContext.RemoveServicePublicKey(
                    _directoryClientContext.LastCreatedService.Id,
                    _directoryClientContext.AddedServicePublicKeys[0]
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to remove a Public Key from the Directory Service with the ID ""(.*)""")]
        public void WhenIAttemptToRemoveAPublicKeyFromTheDirectoryServiceWithTheID(string serviceId)
        {
            try
            {
                _directoryClientContext.RemoveServicePublicKey(
                    Guid.Parse(serviceId),
                    _directoryClientContext.AddedServicePublicKeys.First()
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to remove a Public Key identified by ""(.*)"" from the Directory Service")]
        public void WhenIAttemptToRemoveAPublicKeyIdentifiedByFromTheDirectoryService(string keyId)
        {
            try
            {
                _directoryClientContext.RemoveServicePublicKey(
                    _directoryClientContext.LastCreatedService.Id,
                    keyId
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I updated the Directory Service Public Key to inactive")]
        public void WhenIUpdatedTheDirectoryServicePublicKeyToInactive()
        {
            _directoryClientContext.DeactivateServicePublicKey(
                _directoryClientContext.LastCreatedService.Id,
                _directoryClientContext.AddedServicePublicKeys[0]
            );
        }

        [When(@"I updated the Directory Service Public Key expiration date to ""(.*)""")]
        public void WhenIUpdatedTheDirectoryServicePublicKeyExpirationDateTo(string expiresString)
        {
            var expires = DateTime.Parse(expiresString).ToUniversalTime();
            _directoryClientContext.UpdateServicePublicKeyExpires(
                _directoryClientContext.LastCreatedService.Id,
                _directoryClientContext.AddedServicePublicKeys[0],
                expires
            );
        }

        [When(@"I attempt to update a Public Key for the Directory Service with the ID ""(.*)""")]
        public void WhenIAttemptToUpdateAPublicKeyForTheDirectoryServiceWithTheID(string serviceId)
        {
            try
            {
                _directoryClientContext.UpdateServicePublicKey(
                    Guid.Parse(serviceId),
                    _directoryClientContext.AddedServicePublicKeys[0],
                    true,
                    new DateTime(2020, 1, 1)
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to update a Public Key identified by ""(.*)"" for the Directory Service")]
        public void WhenIAttemptToUpdateAPublicKeyIdentifiedByForTheDirectoryService(string keyId)
        {
            try
            {
                _directoryClientContext.UpdateServicePublicKey(
                    _directoryClientContext.LastCreatedService.Id,
                    keyId,
                    true,
                    new DateTime(2020, 1, 1)
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I add a Public Key with a (BOTH|ENCRYPTION|SIGNATURE) type to the Directory Service")]
        public void WhenIAddAPublicKeyWithAKeyTypeTypeToTheDirectoryService(string keyType)
        {
            KeyType parsedKeyType;
            Enum.TryParse(keyType, true, out parsedKeyType);
            _directoryClientContext.AddServicePublicKey(
                _directoryClientContext.LastCreatedService.Id,
                _keyManager.GetAlphaPublicKey(),
                true,
                new DateTime(2020, 1, 1),
                parsedKeyType
            );
        }

        [When(@"I attempt to add a Public Key with a ""(.*)"" type to the Directory Service")]
        public void WhenIAttemptToAddAPublicKeyWithATypeToTheDirectoryService(string keyType)
        {
            try
            {
                KeyType parsedKeyType;
                if (!Enum.TryParse(keyType, true, out parsedKeyType))
                {
                    parsedKeyType = KeyType.OTHER;
                }
                _directoryClientContext.AddServicePublicKey(
                    _directoryClientContext.LastCreatedService.Id,
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