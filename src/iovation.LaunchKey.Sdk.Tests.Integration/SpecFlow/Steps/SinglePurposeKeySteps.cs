using System;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public sealed class SinglePurposeKeySteps
    {
        private readonly SinglePurposeKeysOrgClientContext _scenarioContext;
        private readonly CommonContext _commonContext;

        public SinglePurposeKeySteps(SinglePurposeKeysOrgClientContext scenarioContext, CommonContext commonContext)
        {
            _scenarioContext = scenarioContext;
            _commonContext = commonContext;
        }

        [Given(@"I am using single purpose keys")]
        public void GivenIAmUsingSinglePurposeKeys()
        {
            _scenarioContext.CreateSinglePurposeKeyFactory();
        }

        [When(@"I perform an API call using single purpose keys")]
        public void WhenIPerformAnApiCallUsingSinglePurposeKeys()
        {
            _scenarioContext.PerformAPICall();
        }

        [Given(@"I am using single purpose keys but I am using my encryption key to sign")]
        public void GivenIAmUsingSinglePurposeKeysButIAmUsingMyEncryptionKeyToSign()
        {
            _scenarioContext.CreateSinglePurposeKeyFactory(SinglePurposeKeysOrgClientContext.SinglePurposeKeyFactoryType.EncryptionKeyToSign);
        }

        [When(@"I attempt an API call using single purpose keys")]
        public void WhenIAttemptAnApiCallUsingSinglePurposeKeys()
        {
            try
            {
                _scenarioContext.PerformAPICall();
            }
            catch (Exception e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int errorCode)
        {
            Assert.AreEqual(true, _commonContext.GetLastException().Message.Contains("401"));
        }

        [Given(@"I am using single purpose keys but I only set my signature key")]
        public void GivenIAmUsingSinglePurposeKeysButIOnlySetMySignatureKey()
        {
            _scenarioContext.CreateSinglePurposeKeyFactory(SinglePurposeKeysOrgClientContext.SinglePurposeKeyFactoryType.NoEncryptionKey);
        }

        [Then(@"no valid key will be available to decrypt response")]
        public void ThenNoValidKeyWillBeAvailableToDecryptResponse()
        {
            Assert.Throws(typeof(NoKeyFoundException), () =>
            {
                _scenarioContext.PerformAPICall();
            });
        }
    }
}