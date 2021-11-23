using System.Text;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OtpNet;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public class DirectoryServiceTotpSteps
    {
        private readonly CommonContext _commonContext;
        private readonly DirectoryTotpContext _directoryTotpContext;
        private readonly DirectoryServiceTotpContext _directoryServiceTotpContext;
        private string _userId;

        public DirectoryServiceTotpSteps(CommonContext commonContext, DirectoryTotpContext directoryTotpContext, DirectoryServiceTotpContext directoryServiceTotpContext)
        {
            _commonContext = commonContext;
            _directoryTotpContext = directoryTotpContext;
            _directoryServiceTotpContext = directoryServiceTotpContext;
        }

        [Given(@"I have created a User TOTP")]
        public void GivenIMakeAUserTotpCreateRequest()
        {
            _userId = Util.UniqueName("TOTP");
            _directoryTotpContext.GenerateUserTotp(_userId);
        }
        
        [When(@"I verify a TOTP code with a valid code")]
        public void WhenIVerifyATotpCodeWithAValidCode()
        {
            string code = _directoryTotpContext.GetCodeForCurrentUserTotpResponse();
            _directoryServiceTotpContext.VerifyUserTotpCode(_userId, code);
        }
        
        [When(@"I verify a TOTP code with an invalid code")]
        public void WhenIVerifyATotpCodeWithAnInvalidCode()
        {
            _directoryServiceTotpContext.VerifyUserTotpCode(_userId, "123456");
        }
        
        [When(@"I verify a TOTP code with an invalid User")]
        public void WhenIVerifyATotpCodeWithAnInvalidUser()
        {
            try
            {
                _directoryServiceTotpContext.VerifyUserTotpCode("NotAValidUser!", "123456");
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Then(@"the TOTP verification response is True")]
        public void ThenTheTotpVerificationResponseIsTrue()
        {
            Assert.IsTrue(_directoryServiceTotpContext.CurrentVerifyUserResponse);
        }
        
        [Then(@"the TOTP verification response is False")]
        public void ThenTheTotpVerificationResponseIsFalse()
        {
            Assert.IsFalse(_directoryServiceTotpContext.CurrentVerifyUserResponse);
        }
    }
}
