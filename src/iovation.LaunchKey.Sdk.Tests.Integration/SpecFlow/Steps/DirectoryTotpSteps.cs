using System.Linq;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public class DirectoryTotpSteps
    {
        private readonly DirectoryTotpContext _directoryTotpContext;

        public DirectoryTotpSteps(DirectoryTotpContext directoryTotpContext)
        {
            _directoryTotpContext = directoryTotpContext;
        }

        [When(@"I make a User TOTP delete request")]
        public void WhenIMakeAUserTotpDeleteRequest()
        {
            _directoryTotpContext.RemoveTotpCodeForUser();
        }
        
        [When(@"I make a User TOTP create request")]
        public void WhenIMakeAUserTotpCreateRequest()
        {
            _directoryTotpContext.GenerateUserTotp();
        }

        [Then(@"the User TOTP create response contains a valid algorithm")]
        public void ThenTheUserTotpCreateResponseContainsAValidAlgorithm()
        {
            string[] validAlgorithms =  {"SHA1", "SHA256", "SHA512"};
            Assert.IsTrue(validAlgorithms.Contains(_directoryTotpContext.CurrentGenerateUserTotpResponse.Algorithm));
        }

        [Then(@"the User TOTP create response contains a valid amount of digits")]
        public void ThenTheUserTotpCreateResponseContainsAValidAmountOfDigits()
        {
            Assert.IsTrue(_directoryTotpContext.CurrentGenerateUserTotpResponse.Digits >= 6);
        }

        [Then(@"the User TOTP create response contains a valid period")]
        public void ThenTheUserTotpCreateResponseContainsAValidPeriod()
        {
            Assert.IsTrue(_directoryTotpContext.CurrentGenerateUserTotpResponse.Period >= 30);
        }

        [Then(@"the User TOTP create response contains a valid secret")]
        public void ThenTheUserTotpCreateResponseContainsAValidSecret()
        {
            Assert.IsTrue(_directoryTotpContext.CurrentGenerateUserTotpResponse.Secret.Length == 32);
        }
    }
}
