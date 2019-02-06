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
    public class OrganizationServiceSteps
    {
        private readonly OrgClientContext _orgClientContext;
        private readonly CommonContext _commonContext;

        public OrganizationServiceSteps(
            CommonContext commonContext,
            OrgClientContext orgClientContext)
        {
            _orgClientContext = orgClientContext;
            _commonContext = commonContext;
        }

        [When(@"I create an Organization Service")]
        public void WhenICreateAnOrganizationService()
        {
            _orgClientContext.CreateService(Util.UniqueServiceName());
        }

        [When(@"I retrieve the created Organization Service")]
        public void WhenIRetrieveTheCreatedOrganizationService()
        {
            _orgClientContext.LoadLastCreatedService();
        }

        [Then(@"the Organization Service name is the same as was sent")]
        public void ThenTheOrganizationServiceNameIsTheSameAsWasSent()
        {
            Assert.AreEqual(
                _orgClientContext.LastCreatedService.Name,
                _orgClientContext.LoadedService.Name,
                "Service name should match."
            );
        }

        [Given(@"I created an Organization Service")]
        public void GivenICreatedAnOrganizationService()
        {
            _orgClientContext.CreateService(Util.UniqueServiceName());
        }

        [Given(@"I attempt to create a Organization Service with the same name")]
        public void GivenIAttemptToCreateAOrganizationServiceWithTheSameName()
        {
            try
            {
                _orgClientContext.CreateService(_orgClientContext.LastCreatedService.Name);
            }
            catch (BaseException be)
            {
                _commonContext.RecordException(be);
            }
        }

        [When(@"I retrieve a list of all Organization Services")]
        public void WhenIRetrieveAListOfAllOrganizationServices()
        {
            _orgClientContext.LoadAllServices();
        }

        [Then(@"the current Organization Service is in the Services list")]
        public void ThenTheCurrentOrganizationServiceIsInTheServicesList()
        {
            Assert.IsTrue(
                _orgClientContext.LoadedServices.Count >= 1,
                "GetAllServices should contain at least 1 entry."
            );
            Assert.IsTrue(
                _orgClientContext.LoadedServices.Any(s => s.Name == _orgClientContext.LastCreatedService.Name),
                "GetAllServices results should contain an entry for the service we created."
            );
        }

        [When(@"I retrieve a list of Organization Services with the created Service's ID")]
        public void WhenIRetrieveAListOfOrganizationServicesWithTheCreatedServiceSID()
        {
            _orgClientContext.LoadServices(new List<Guid> { _orgClientContext.LastCreatedService.Id });
        }

        [Then(@"the current Organization Service list is a list with only the current Service")]
        public void ThenTheCurrentOrganizationServiceListIsAListWithOnlyTheCurrentService()
        {
            Assert.IsTrue(
                _orgClientContext.LoadedServices.Count == 1,
                "GetServices result should be exactly 1 element in length."
            );
            Assert.IsTrue(
                _orgClientContext.LoadedServices[0].Id == _orgClientContext.LastCreatedService.Id,
                "GetServices result's single element should have the same ID as the service we created."
            );
        }

        [When(@"I attempt retrieve a list of Organization Services with the Service ID ""(.*)""")]
        public void WhenIAttemptRetrieveAListOfOrganizationServicesWithTheServiceID(string serviceId)
        {
            var guid = Guid.Parse(serviceId);

            try
            {
                _orgClientContext.LoadServices(new List<Guid> { guid });
            }
            catch (BaseException be)
            {
                _commonContext.RecordException(be);
            }
        }

        [Given(@"I created a Organization Service with the following:")]
        public void GivenICreatedAOrganizationServiceWithTheFollowing(Table table)
        {
            var serviceTable = table.CreateInstance<ServiceDescriptionTable>();
            _orgClientContext.CreateService(
                Util.UniqueServiceName(),
                serviceTable.Description,
                new Uri(serviceTable.Icon),
                new Uri(serviceTable.CallbackUrl),
                serviceTable.Active
            );
        }

        [Then(@"the Organization Service description is ""(.*)""")]
        public void ThenTheOrganizationServiceDescriptionIs(string serviceDescription)
        {
            Assert.AreEqual(
                serviceDescription,
                _orgClientContext.LoadedService.Description,
                "The service description should match what we sent"
            );
        }

        [Then(@"the Organization Service icon is ""(.*)""")]
        public void ThenTheOrganizationServiceIconIs(string serviceIcon)
        {
            Assert.AreEqual(
                new Uri(serviceIcon),
                _orgClientContext.LoadedService.Icon,
                "The service icon URL should match what we sent"
            );
        }

        [Then(@"the Organization Service callback_url is ""(.*)""")]
        public void ThenTheOrganizationServiceCallback_UrlIs(string serviceCallbackUrl)
        {
            Assert.AreEqual(
                new Uri(serviceCallbackUrl),
                _orgClientContext.LoadedService.CallbackUrl,
                "The service callback URL should match what we sent"
            );
        }

        [Then(@"the Organization Service is active")]
        public void ThenTheOrganizationServiceIsActive()
        {
            Assert.IsTrue(_orgClientContext.LoadedService.Active, "The service should be active");
        }

        [When(@"I attempt to retrieve the Organization Service with the ID ""(.*)""")]
        public void WhenIAttemptToRetrieveTheOrganizationServiceWithTheID(string serviceId)
        {
            try
            {
                _orgClientContext.LoadService(Guid.Parse(serviceId));
            }
            catch (BaseException be)
            {
                _commonContext.RecordException(be);
            }
        }

        [When(@"I update the Organization Service with the following:")]
        public void WhenIUpdateTheOrganizationServiceWithTheFollowing(Table table)
        {
            var serviceTable = table.CreateInstance<ServiceDescriptionTable>();
            _orgClientContext.UpdateService(
                _orgClientContext.LastCreatedService.Id,
                _orgClientContext.LastCreatedService.Name,
                serviceTable.Description,
                new Uri(serviceTable.Icon),
                new Uri(serviceTable.CallbackUrl),
                serviceTable.Active
            );
        }

        [Then(@"the Organization Service is not active")]
        public void ThenTheOrganizationServiceIsNotActive()
        {
            Assert.IsFalse(_orgClientContext.LoadedService.Active, "Service should not be inactive (Active should be False)");
        }

        [When(@"I attempt to update the active status of the Organization Service with the ID ""(.*)""")]
        public void WhenIAttemptToUpdateTheActiveStatusOfTheOrganizationServiceWithTheID(string serviceId)
        {
            try
            {
                _orgClientContext.UpdateService(
                    Guid.Parse(serviceId),
                    "A new name",
                    "A new description",
                    new Uri("http://a.new.url/icon"),
                    new Uri("http://a.new.url/callback"),
                    true
                );
            }
            catch (BaseException be)
            {
                _commonContext.RecordException(be);
            }
        }
    }
}
