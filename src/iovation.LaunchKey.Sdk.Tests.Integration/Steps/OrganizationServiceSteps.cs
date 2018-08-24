using System;
using System.Collections.Generic;
using System.IO;
using iovation.LaunchKey.Sdk.Error;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps
{
	public class OrganizationServiceContext : IDisposable
	{
		private List<Guid> _ownedServices = new List<Guid>();
		public void CreateService(string serviceName)
		{
			var serviceKeyContents = File.ReadAllText(privateKey);
			var factory = new FactoryFactoryBuilder().Build();
			var organizationFactory = factory.MakeOrganizationFactory(orgId, serviceKeyContents);
			var orgClient = organizationFactory.MakeOrganizationClient();
			var svcId = orgClient.CreateService(name, "Example", new Uri("http://example.com"), new Uri("http://example.com/callback"), true);
			_ownedServices.Add(svcId);
		}
			
		public void Dispose()
		{
			// todo cleanup services we made on the server
		}
	}

	[Binding]
	public class OrganizationServiceSteps
	{
		private readonly OrganizationServiceContext _context;

		public OrganizationServiceSteps(OrganizationServiceContext context)
		{
			_context = context;
		}

		[When(@"I retrieve a list of all Organization Services")]
		public void WhenIRetrieveAListOfAllOrganizationServices()
		{
			//ScenarioContext.Current.Pending();
		}

		[Then(@"the current Organization Service is in the Services list")]
		public void ThenTheCurrentOrganizationServiceIsInTheServicesList()
		{
			//ScenarioContext.Current.Pending();
		}

		[Given(@"I created an Organization Service")]
		public void GivenICreatedAnOrganizationService()
		{
			var serviceName = "John hobo";
			_context.CreateService(serviceName);
		}

		[Given(@"I attempt to create a Organization Service with the same name")]
		public void GivenIAttemptToCreateAOrganizationServiceWithTheSameName()
		{
			//ScenarioContext.Current.Pending();
		}

		[When(@"I create an Organization Service")]
		public void WhenICreateAnOrganizationService()
		{
			//ScenarioContext.Current.Pending();
		}

		[When(@"I retrieve the created Organization Service")]
		public void WhenIRetrieveTheCreatedOrganizationService()
		{
			//ScenarioContext.Current.Pending();
		}

		[Then(@"the Organization Service name is the same as was sent")]
		public void ThenTheOrganizationServiceNameIsTheSameAsWasSent()
		{
			//ScenarioContext.Current.Pending();
		}

		[Then(@"a com\.iovation\.launchkey\.sdk\.error\.ServiceNameTaken exception is thrown")]
		public void ThenACom_Iovation_Launchkey_Sdk_Error_ServiceNameTakenExceptionIsThrown()
		{
			//ScenarioContext.Current.Pending();
		}
	}
}
