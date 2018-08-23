using System;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps
{
	public class OrganizationServiceContext
	{

	}

    [Binding]
    public class OrganizationServiceSteps
    {
        [Given(@"I created an Organization Service")]
        public void GivenICreatedAnOrganizationService()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I attempt to create a Organization Service with the same name")]
        public void GivenIAttemptToCreateAOrganizationServiceWithTheSameName()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I create an Organization Service")]
        public void WhenICreateAnOrganizationService()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I retrieve the created Organization Service")]
        public void WhenIRetrieveTheCreatedOrganizationService()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the Organization Service name is the same as was sent")]
        public void ThenTheOrganizationServiceNameIsTheSameAsWasSent()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"a com\.iovation\.launchkey\.sdk\.error\.ServiceNameTaken exception is thrown")]
        public void ThenACom_Iovation_Launchkey_Sdk_Error_ServiceNameTakenExceptionIsThrown()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
