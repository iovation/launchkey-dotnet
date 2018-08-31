using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Error;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps.OrgClient
{
	[Binding]
	public class OrganizationServiceSteps
	{
		private readonly TestConfiguration _config;
		private readonly CommonContext _commonContext;

		private List<CreatedServiceInfo> _ownedServices = new List<CreatedServiceInfo>();
		private Service _lastRetrievedService = null;
		private CreatedServiceInfo _lastCreatedService = null;
		private List<Service> _lastGetAllResult = null;
		private List<Service> _lastGetServicesResult = null;

		private IOrganizationClient _orgClient;

		public OrganizationServiceSteps(TestConfiguration config, CommonContext commonContext)
		{
			_config = config;
			_commonContext = commonContext;
			_orgClient = config.GetOrgClient();
		}
		
		private void CreateAService(string serviceName)
		{
			var serviceGuid = _orgClient.CreateService(
				serviceName,
				"Test desc",
				new Uri("http://a.com/icon"),
				new Uri("http://a.com/cb"),
				true
			);
			_lastCreatedService = new CreatedServiceInfo(serviceGuid, serviceName);
			_ownedServices.Add(_lastCreatedService);
		}

		private void CreateAUniquelyNamedService()
		{
			CreateAService(Util.UniqueName("Service"));
		}

		[When(@"I create an Organization Service")]
		public void WhenICreateAnOrganizationService()
		{
			CreateAUniquelyNamedService();
		}

		[When(@"I retrieve the created Organization Service")]
		public void WhenIRetrieveTheCreatedOrganizationService()
		{
			_lastRetrievedService = _orgClient.GetService(_ownedServices[0].Id);
		}

		[Then(@"the Organization Service name is the same as was sent")]
		public void ThenTheOrganizationServiceNameIsTheSameAsWasSent()
		{
			Assert.AreEqual(_lastCreatedService.Name, _lastRetrievedService.Name, "Service name should match.");
		}

		[Given(@"I created an Organization Service")]
		public void GivenICreatedAnOrganizationService()
		{
			CreateAUniquelyNamedService();
		}

		[Given(@"I attempt to create a Organization Service with the same name")]
		public void GivenIAttemptToCreateAOrganizationServiceWithTheSameName()
		{
			try
			{
				CreateAService(_ownedServices[0].Name);
			}
			catch (BaseException be)
			{
				_commonContext.RecordException(be);
			}
		}

		[When(@"I retrieve a list of all Organization Services")]
		public void WhenIRetrieveAListOfAllOrganizationServices()
		{
			_lastGetAllResult = _orgClient.GetAllServices();
		}

		[Then(@"the current Organization Service is in the Services list")]
		public void ThenTheCurrentOrganizationServiceIsInTheServicesList()
		{
			Assert.IsNotNull(_lastGetAllResult, "GetAllServices result should be non-null");
			Assert.IsTrue(_lastGetAllResult.Count >= 1, "GetAllServices should contain at least 1 entry.");
			Assert.IsTrue(_lastGetAllResult.Any(s => s.Name == _lastCreatedService.Name), "GetAllServices results should contain an entry for the service we created.");
		}

		[When(@"I retrieve a list of Organization Services with the created Service's ID")]
		public void WhenIRetrieveAListOfOrganizationServicesWithTheCreatedServiceSID()
		{
			_lastGetServicesResult = _orgClient.GetServices(new List<Guid> {_lastCreatedService.Id});
		}

		[Then(@"the current Organization Service list is a list with only the current Service")]
		public void ThenTheCurrentOrganizationServiceListIsAListWithOnlyTheCurrentService()
		{
			Assert.IsNotNull(_lastGetServicesResult, "GetServices result should be non-null");
			Assert.IsTrue(_lastGetServicesResult.Count == 1, "GetServices result should be exactly 1 element in length.");
			Assert.IsTrue(_lastGetServicesResult[0].Id == _lastCreatedService.Id, "GetServices result's single element should have the same ID as the service we created.");
		}

		[When(@"I attempt retrieve a list of Organization Services with the Service ID ""(.*)""")]
		public void WhenIAttemptRetrieveAListOfOrganizationServicesWithTheServiceID(string serviceId)
		{
			var guid = Guid.Parse(serviceId);

			try
			{
				_lastGetServicesResult = _orgClient.GetServices(new List<Guid> {guid});
			}
			catch (BaseException be)
			{
				_commonContext.RecordException(be);
			}
		}

		[Given(@"I created a Organization Service with the following:")]
		public void GivenICreatedAOrganizationServiceWithTheFollowing(Table table)
		{
			var userServiceInfo = TechTalk.SpecFlow.Assist.TableHelperExtensionMethods.CreateInstance<ServiceDescriptionTable>(table);
			var serviceName = Util.UniqueName("service");
			var serviceId = _orgClient.CreateService(
				serviceName,
				userServiceInfo.Description,
				new Uri(userServiceInfo.Icon),
				new Uri(userServiceInfo.CallbackUrl),
				userServiceInfo.Active
			);
			_lastCreatedService = new CreatedServiceInfo(serviceId, serviceName);
			_ownedServices.Add(_lastCreatedService);
		}

		[Then(@"the Organization Service description is ""(.*)""")]
		public void ThenTheOrganizationServiceDescriptionIs(string serviceDescription)
		{
			Assert.AreEqual(serviceDescription, _lastRetrievedService.Description, "The service description should match what we sent");
		}

		[Then(@"the Organization Service icon is ""(.*)""")]
		public void ThenTheOrganizationServiceIconIs(string serviceIcon)
		{
			Assert.AreEqual(new Uri(serviceIcon), _lastRetrievedService.Icon, "The service icon URL should match what we sent");
		}

		[Then(@"the Organization Service callback_url is ""(.*)""")]
		public void ThenTheOrganizationServiceCallback_UrlIs(string serviceCallbackUrl)
		{
			Assert.AreEqual(new Uri(serviceCallbackUrl), _lastRetrievedService.CallbackUrl, "The service callback URL should match what we sent");
		}

		[Then(@"the Organization Service is active")]
		public void ThenTheOrganizationServiceIsActive()
		{
			Assert.IsTrue(_lastRetrievedService.Active, "The service should be active");
		}

		[When(@"I attempt to retrieve the Organization Service with the ID ""(.*)""")]
		public void WhenIAttemptToRetrieveTheOrganizationServiceWithTheID(string serviceId)
		{
			try
			{
				_orgClient.GetService(Guid.Parse(serviceId));
			}
			catch (BaseException be)
			{
				_commonContext.RecordException(be);
			}
		}

		[When(@"I update the Organization Service with the following:")]
		public void WhenIUpdateTheOrganizationServiceWithTheFollowing(Table table)
		{
			var userServiceInfo = TechTalk.SpecFlow.Assist.TableHelperExtensionMethods.CreateInstance<ServiceDescriptionTable>(table);
			_orgClient.UpdateService(
				_lastCreatedService.Id,
				_lastCreatedService.Name,
				userServiceInfo.Description,
				new Uri(userServiceInfo.Icon),
				new Uri(userServiceInfo.CallbackUrl),
				userServiceInfo.Active
			);
		}

		[Then(@"the Organization Service is not active")]
		public void ThenTheOrganizationServiceIsNotActive()
		{
			Assert.IsFalse(_lastRetrievedService.Active, "Service should not be active (Active should be False)");
		}

		[When(@"I attempt to update the active status of the Organization Service with the ID ""(.*)""")]
		public void WhenIAttemptToUpdateTheActiveStatusOfTheOrganizationServiceWithTheID(string serviceId)
		{
			try
			{
				var serviceIdGuid = Guid.Parse(serviceId);
				_orgClient.UpdateService(
					serviceIdGuid,
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

		[After]
		public void After()
		{
			Debug.WriteLine("After feature");
		}

		[Before]
		public void Before()
		{
			Debug.WriteLine("Before feature");
		}
	}
}
