using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps.OrgClient
{
	[Binding]
	class DirectoryServiceSteps
	{
		private readonly TestConfiguration _config;
		private readonly CommonContext _commonContext;

		private List<CreatedServiceInfo> _ownedServices = new List<CreatedServiceInfo>();
		private Service _lastRetrievedService = null;
		private CreatedServiceInfo _lastCreatedService = null;
		private List<Service> _lastGetAllResult = null;
		private List<Service> _lastGetServicesResult = null;

		private IOrganizationClient _orgClient;

		public DirectoryServiceSteps(TestConfiguration config, CommonContext commonContext)
		{
			_config = config;
			_commonContext = commonContext;
			_orgClient = MakeOrgClient();
		}

		private IOrganizationClient MakeOrgClient()
		{
			var factory = new FactoryFactoryBuilder().Build();
			var organizationFactory = factory.MakeOrganizationFactory(_config.OrgId, _config.OrgPrivateKey);
			return organizationFactory.MakeOrganizationClient();
		}

		[Given(@"I created a Directory")]
		public void GivenICreatedADirectory()
		{
			ScenarioContext.Current.Pending();
		}

		[When(@"I create a Directory Service with the following:")]
		public void WhenICreateADirectoryServiceWithTheFollowing(Table table)
		{
			ScenarioContext.Current.Pending();
		}

		[When(@"I retrieve the created Directory Service")]
		public void WhenIRetrieveTheCreatedDirectoryService()
		{
			ScenarioContext.Current.Pending();
		}

		[Then(@"the Directory Service name is the same as was sent")]
		public void ThenTheDirectoryServiceNameIsTheSameAsWasSent()
		{
			ScenarioContext.Current.Pending();
		}

		[Then(@"the Directory Service description is ""(.*)""")]
		public void ThenTheDirectoryServiceDescriptionIs(string p0)
		{
			ScenarioContext.Current.Pending();
		}

		[Then(@"the Directory Service icon is ""(.*)""")]
		public void ThenTheDirectoryServiceIconIs(string p0)
		{
			ScenarioContext.Current.Pending();
		}

		[Then(@"the Directory Service callback_url is ""(.*)""")]
		public void ThenTheDirectoryServiceCallback_UrlIs(string p0)
		{
			ScenarioContext.Current.Pending();
		}

		[Then(@"the Directory Service is active")]
		public void ThenTheDirectoryServiceIsActive()
		{
			ScenarioContext.Current.Pending();
		}

		[Given(@"I created a Directory Service")]
		public void GivenICreatedADirectoryService()
		{
			ScenarioContext.Current.Pending();
		}

		[Given(@"I attempt to create a Directory Service with the same name")]
		public void GivenIAttemptToCreateADirectoryServiceWithTheSameName()
		{
			ScenarioContext.Current.Pending();
		}

	}
}
