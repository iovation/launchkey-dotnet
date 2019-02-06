using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public class CommonSteps
    {
        private readonly CommonContext _commonContext;

        public CommonSteps(CommonContext commonContext)
        {
            _commonContext = commonContext;
        }

        [Then(@"an? (.+) error occurs")]
        public void ThrowExceptionStep(string errorName)
        {
            var exception = _commonContext.GetLastException();
            Assert.IsNotNull(exception, "An exception was not thrown when one was expected.");
            Assert.AreEqual($"iovation.LaunchKey.Sdk.Error.{errorName}", exception.GetType().ToString(), "Exception was thrown but was the wrong type.");
        }

        [Then(@"there are no errors")]
        public void NoErrors()
        {
            Assert.IsNull(_commonContext.GetLastException());
        }
    }
}
