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
    public class DirectoryServiceSteps
    {
        private readonly CommonContext _commonContext;
        private readonly DirectoryClientContext _directoryClientContext;

        public DirectoryServiceSteps(CommonContext commonContext, DirectoryClientContext directoryClientContext)
        {
            _commonContext = commonContext;
            _directoryClientContext = directoryClientContext;
        }

        [When(@"I create a Directory Service with the following:")]
        [Given(@"I created a Directory Service with the following:")]
        public void WhenICreateADirectoryServiceWithTheFollowing(Table table)
        {
            var serviceTable = table.CreateInstance<ServiceDescriptionTable>();
            _directoryClientContext.CreateService(
                Util.UniqueServiceName(),
                serviceTable.Description,
                new Uri(serviceTable.Icon),
                new Uri(serviceTable.CallbackUrl),
                serviceTable.Active
            );
        }

        [When(@"I retrieve the created Directory Service")]
        public void WhenIRetrieveTheCreatedDirectoryService()
        {
            _directoryClientContext.LoadService(_directoryClientContext.LastCreatedService.Id);
        }

        [Then(@"the Directory Service name is the same as was sent")]
        public void ThenTheDirectoryServiceNameIsTheSameAsWasSent()
        {
            Assert.IsTrue(_directoryClientContext.LoadedService.Name == _directoryClientContext.LastCreatedService.Name);
        }

        [Then(@"the Directory Service description is ""(.*)""")]
        public void ThenTheDirectoryServiceDescriptionIs(string p0)
        {
            Assert.IsTrue(_directoryClientContext.LoadedService.Description == p0);
        }

        [Then(@"the Directory Service icon is ""(.*)""")]
        public void ThenTheDirectoryServiceIconIs(string p0)
        {
            Assert.IsTrue(_directoryClientContext.LoadedService.Icon == new Uri(p0));
        }

        [Then(@"the Directory Service callback_url is ""(.*)""")]
        public void ThenTheDirectoryServiceCallback_UrlIs(string p0)
        {
            Assert.IsTrue(_directoryClientContext.LoadedService.CallbackUrl == new Uri(p0));
        }

        [Then(@"the Directory Service is active")]
        public void ThenTheDirectoryServiceIsActive()
        {
            Assert.IsTrue(_directoryClientContext.LoadedService.Active);
        }

        [Given(@"I created a Directory Service")]
        public void GivenICreatedADirectoryService()
        {
            _directoryClientContext.CreateService(Util.UniqueServiceName());
        }

        [Given(@"I attempt to create a Directory Service with the same name")]
        public void GivenIAttemptToCreateADirectoryServiceWithTheSameName()
        {
            try
            {
                _directoryClientContext.CreateService(_directoryClientContext.LastCreatedService.Name);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }


        [When(@"I retrieve a list of all Directory Services")]
        public void WhenIRetrieveAListOfAllDirectoryServices()
        {
            _directoryClientContext.LoadAllServices();
        }

        [Then(@"the current Directory Service list is an empty list")]
        public void ThenTheCurrentDirectoryServiceListIsAnEmptyList()
        {
            Assert.IsTrue(_directoryClientContext.LoadedServices.Count == 0);
        }

        [Then(@"the current Directory Service is in the Services list")]
        public void ThenTheCurrentDirectoryServiceIsInTheServicesList()
        {
            Assert.IsTrue(_directoryClientContext.LoadedServices.Any(svc => svc.Id == _directoryClientContext.LastCreatedService.Id));
        }

        [When(@"I retrieve a list of Directory Services with the created Service's ID")]
        public void WhenIRetrieveAListOfDirectoryServicesWithTheCreatedServiceSID()
        {
            _directoryClientContext.LoadServices(new List<Guid> { _directoryClientContext.LastCreatedService.Id });
        }

        [Then(@"the current Directory Service list is a list with only the current Service")]
        public void ThenTheCurrentDirectoryServiceListIsAListWithOnlyTheCurrentService()
        {
            Assert.IsTrue(_directoryClientContext.LoadedServices.Count == 1);
            Assert.IsTrue(_directoryClientContext.LoadedServices[0].Id == _directoryClientContext.LastCreatedService.Id);
        }

        [When(@"I attempt retrieve a list of Directory Services with the Service ID ""(.*)""")]
        public void WhenIAttemptRetrieveAListOfDirectoryServicesWithTheServiceID(string p0)
        {
            try
            {
                _directoryClientContext.LoadServices(new List<Guid> { Guid.Parse(p0) });
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to retrieve the Directory Service with the ID ""(.*)""")]
        public void WhenIAttemptToRetrieveTheDirectoryServiceWithTheID(string p0)
        {
            try
            {
                _directoryClientContext.LoadService(Guid.Parse(p0));
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I update the Directory Service with the following:")]
        public void WhenIUpdateTheDirectoryServiceWithTheFollowing(Table table)
        {
            var serviceDescription = table.CreateInstance<ServiceDescriptionTable>();
            _directoryClientContext.UpdateService(
                _directoryClientContext.LastCreatedService.Id,
                _directoryClientContext.LastCreatedService.Name,
                serviceDescription.Description,
                new Uri(serviceDescription.Icon),
                new Uri(serviceDescription.CallbackUrl),
                serviceDescription.Active
            );
        }

        [Then(@"the Directory Service is not active")]
        public void ThenTheDirectoryServiceIsNotActive()
        {
            Assert.IsTrue(_directoryClientContext.LoadedService.Active == false);
        }

        [When(@"I attempt to update the active status of the Directory Service with the ID ""(.*)""")]
        public void WhenIAttemptToUpdateTheActiveStatusOfTheDirectoryServiceWithTheID(string p0)
        {
            try
            {
                _directoryClientContext.UpdateService(
                    Guid.Parse(p0),
                    "A new name",
                    "A new description",
                    new Uri("http://a.new.url/icon"),
                    new Uri("http://a.new.url/callback"),
                    true
                );

            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }
    }
}
