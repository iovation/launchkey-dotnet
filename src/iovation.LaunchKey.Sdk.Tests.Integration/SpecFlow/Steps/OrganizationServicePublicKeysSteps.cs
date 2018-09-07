using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
	[Binding]
    public class OrganizationServicePublicKeysSteps
    {
		private readonly CommonContext _commonContext;
		private readonly OrgClientContext _orgClientContext;
		private readonly KeyManager _keyManager;
		
		public OrganizationServicePublicKeysSteps(CommonContext commonContext, OrgClientContext orgClientContext, KeyManager keyManager)
		{
			_commonContext = commonContext;
			_orgClientContext = orgClientContext;
			_keyManager = keyManager;
		}

		[Given(@"I added a Public Key to the Organization Service")]
		[When(@"I add a Public Key to the Organization Service")]
		public void WhenIAddAPublicKeyToTheOrganizationService()
		{
			_orgClientContext.AddServicePublicKey(
				_orgClientContext.LastCreatedService.Id,
				_keyManager.GetAlphaPublicKey(),
				true,
				null
			);
		}

		[Given(@"I added another Public Key to the Organization Service")]
		[When(@"I add another Public Key to the Organization Service")]
		public void WhenIAddAnotherPublicKeyToTheOrganizationService()
		{
			_orgClientContext.AddServicePublicKey(
				_orgClientContext.LastCreatedService.Id,
				_keyManager.GetBetaPublicKey(),
				true,
				null
			);
		}

		[When(@"I retrieve the current Organization Service's Public Keys")]
		public void WhenIRetrieveTheCurrentOrganizationServiceSPublicKeys()
		{
			_orgClientContext.LoadServicePublicKeys(_orgClientContext.LastCreatedService.Id);
		}

		[Then(@"the Public Key is in the list of Public Keys for the Organization Service")]
		public void ThenThePublicKeyIsInTheListOfPublicKeysForTheOrganizationService()
		{
			Assert.IsTrue(_orgClientContext.LoadedServicePublicKeys.Any(
				e => e.Id == _orgClientContext.AddedServicePublicKeys[0]
			));
		}

		[Then(@"the other Public Key is in the list of Public Keys for the Organization Service")]
		public void ThenTheOtherPublicKeyIsInTheListOfPublicKeysForTheOrganizationService()
		{
			Assert.IsTrue(_orgClientContext.LoadedServicePublicKeys.Any(
				e => e.Id == _orgClientContext.AddedServicePublicKeys[1]
			));
		}

		[When(@"I attempt to add a Public Key to the Organization Service with the ID ""(.*)""")]
		public void WhenIAttemptToAddAPublicKeyToTheOrganizationServiceWithTheID(string serviceId)
		{
			try
			{
				_orgClientContext.AddServicePublicKey(Guid.Parse(serviceId), _keyManager.GetAlphaPublicKey(), true, null);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I attempt to add the same Public Key to the Organization Service")]
		public void WhenIAttemptToAddTheSamePublicKeyToTheOrganizationService()
		{
			try
			{
				_orgClientContext.AddServicePublicKey(_orgClientContext.LastCreatedService.Id, _keyManager.GetAlphaPublicKey(), true, null);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[Then(@"the Organization Service Public Keys list is empty")]
		public void ThenTheOrganizationServicePublicKeysListIsEmpty()
		{
			Assert.IsTrue(_orgClientContext.LoadedServicePublicKeys.Count == 0);
		}

		[Given(@"I added a Public Key to the Organization Service which is (active|inactive) and expires on ""(.*)""")]
		public void GivenIAddedAPublicKeyToTheOrganizationServiceWhichIsInactiveAndExpiresOn(string activeStatus, string expireString)
		{
			var active = activeStatus == "active";
			var expires = DateTime.Parse(expireString).ToUniversalTime();
			_orgClientContext.AddServicePublicKey(
				_orgClientContext.LastCreatedService.Id,
				_keyManager.GetAlphaPublicKey(),
				active,
				expires
			);
		}

		[Then(@"the Organization Service Public Key is inactive")]
		public void ThenTheOrganizationServicePublicKeyIsInactive()
		{
			Assert.IsFalse(_orgClientContext.LoadedServicePublicKeys[0].Active);
		}

		[Then(@"the Organization Service Public Key Expiration Date is ""(.*)""")]
		public void ThenTheOrganizationServicePublicKeyExpirationDateIs(string expireString)
		{
			var expires = DateTime.Parse(expireString).ToUniversalTime();
			Assert.AreEqual(expires, _orgClientContext.LoadedServicePublicKeys[0].Expires);
		}

		[When(@"I attempt to retrieve the Public Keys for the Organization Service with the ID ""(.*)""")]
		public void WhenIAttemptToRetrieveThePublicKeysForTheOrganizationServiceWithTheID(string serviceId)
		{
			try
			{
				_orgClientContext.LoadServicePublicKeys(Guid.Parse(serviceId));
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I remove the current Organization Service Public Key")]
		public void WhenIRemoveTheCurrentOrganizationServicePublicKey()
		{
			_orgClientContext.RemoveServicePublicKey(
				_orgClientContext.LastCreatedService.Id, 
				_orgClientContext.AddedServicePublicKeys.Last()
			);
		}

		[Then(@"the last current Organization Service's Public Key is not in the list")]
		public void ThenTheLastCurrentOrganizationServiceSPublicKeyIsNotInTheList()
		{
			Assert.IsFalse(
				_orgClientContext
					.LoadedServicePublicKeys
					.Any(k => k.Id == _orgClientContext.AddedServicePublicKeys.Last())
			);
		}

		[When(@"I attempt to remove the current Organization Service Public Key")]
		public void WhenIAttemptToRemoveTheCurrentOrganizationServicePublicKey()
		{
			try
			{
				_orgClientContext.RemoveServicePublicKey(
					_orgClientContext.LastCreatedService.Id,
					_orgClientContext.AddedServicePublicKeys[0]
				);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I attempt to remove a Public Key from the Organization Service with the ID ""(.*)""")]
		public void WhenIAttemptToRemoveAPublicKeyFromTheOrganizationServiceWithTheID(string serviceId)
		{
			try
			{	
				_orgClientContext.RemoveServicePublicKey(
					Guid.Parse(serviceId), 
					_orgClientContext.AddedServicePublicKeys.First()
				);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I attempt to remove a Public Key identified by ""(.*)"" from the Organization Service")]
		public void WhenIAttemptToRemoveAPublicKeyIdentifiedByFromTheOrganizationService(string keyId)
		{
			try
			{
				_orgClientContext.RemoveServicePublicKey(
					_orgClientContext.LastCreatedService.Id,
					keyId
				);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I updated the Organization Service Public Key to inactive")]
		public void WhenIUpdatedTheOrganizationServicePublicKeyToInactive()
		{
			_orgClientContext.DeactivateServicePublicKey(
				_orgClientContext.LastCreatedService.Id,
				_orgClientContext.AddedServicePublicKeys[0]
			);
		}

		[When(@"I updated the Organization Service Public Key expiration date to ""(.*)""")]
		public void WhenIUpdatedTheOrganizationServicePublicKeyExpirationDateTo(string expiresString)
		{
			var expires = DateTime.Parse(expiresString).ToUniversalTime();
			_orgClientContext.UpdateServicePublicKeyExpires(
				_orgClientContext.LastCreatedService.Id,
				_orgClientContext.AddedServicePublicKeys[0],
				expires
			);
		}

		[When(@"I attempt to update a Public Key for the Organization Service with the ID ""(.*)""")]
		public void WhenIAttemptToUpdateAPublicKeyForTheOrganizationServiceWithTheID(string serviceId)
		{
			try
			{
				_orgClientContext.UpdateServicePublicKey(
					Guid.Parse(serviceId),
					_orgClientContext.AddedServicePublicKeys[0],
					true,
					new DateTime(2020, 1, 1)
				);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I attempt to update a Public Key identified by ""(.*)"" for the Organization Service")]
		public void WhenIAttemptToUpdateAPublicKeyIdentifiedByForTheOrganizationService(string keyId)
		{
			try
			{
				_orgClientContext.UpdateServicePublicKey(
					_orgClientContext.LastCreatedService.Id,
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
	}
}
