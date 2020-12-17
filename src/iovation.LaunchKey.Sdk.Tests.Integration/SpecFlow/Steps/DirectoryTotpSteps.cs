using System;
using System.Collections.Generic;
using System.Linq;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public class DirectoryTotpSteps
    {
        private readonly CommonContext _commonContext;
        private readonly DirectoryClientContext _directoryClientContext;

        public DirectoryTotpSteps(CommonContext commonContext, DirectoryClientContext directoryClientContext)
        {
            _commonContext = commonContext;
            _directoryClientContext = directoryClientContext;
        }

        [When(@"I make a User TOTP create request")]
        public void WhenIMakeAUserTOTPCreateRequest()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the User TOTP create response contains a valid algorithm")]
        public void ThenTheUserTOTPCreateResponseContainsAValidAlgorithm()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the User TOTP create response contains a valid amount of digits")]
        public void ThenTheUserTOTPCreateResponseContainsAValidAmountOfDigits()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the User TOTP create response contains a valid period")]
        public void ThenTheUserTOTPCreateResponseContainsAValidPeriod()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the User TOTP create response contains a valid secret")]
        public void ThenTheUserTOTPCreateResponseContainsAValidSecret()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
