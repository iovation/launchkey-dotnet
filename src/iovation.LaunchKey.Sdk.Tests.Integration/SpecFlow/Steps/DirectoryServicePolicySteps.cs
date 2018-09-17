using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Tables;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
	[Binding]
	public class DirectoryServicePolicySteps
	{
		private readonly CommonContext _commonContext;
		private readonly DirectoryClientContext _directoryClientContext;

		public DirectoryServicePolicySteps(CommonContext commonContext, DirectoryClientContext directoryClientContext)
		{
			_commonContext = commonContext;
			_directoryClientContext = directoryClientContext;
		}

		[When(@"I retrieve the Policy for the Current Directory Service")]
		[Given(@"I retrieve the Policy for the Current Directory Service")]
		public void WhenIRetrieveThePolicyForTheCurrentOrganizationService()
		{
			_directoryClientContext.LoadServicePolicy(_directoryClientContext.LastCreatedService.Id);
		}

		[Then(@"the Directory Service Policy has no requirement for inherence")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForInherence()
		{
			Assert.IsNull(_directoryClientContext.LoadedServicePolicy.RequireInherenceFactor);
		}

		[Then(@"the Directory Service Policy has no requirement for knowledge")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForKnowledge()
		{
			Assert.IsNull(_directoryClientContext.LoadedServicePolicy.RequireKnowledgeFactor);
		}

		[Then(@"the Directory Service Policy has no requirement for possession")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForPossession()
		{
			Assert.IsNull(_directoryClientContext.LoadedServicePolicy.RequirePossessionFactor);
		}

		[Then(@"the Directory Service Policy has no requirement for number of factors")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForNumberOfFactors()
		{
			Assert.IsNull(_directoryClientContext.LoadedServicePolicy.RequiredFactors);
		}

		[Given(@"the Directory Service Policy is set to require (.*) factors")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireFactors(int numFactors)
		{
			_directoryClientContext.LoadedServicePolicy.RequiredFactors = numFactors;
		}

		[Given(@"I set the Policy for the Directory Service")]
		[Given(@"I set the Policy for the Current Directory Service")]
		[When(@"I set the Policy for the Current Directory Service")]
		public void GivenISetThePolicyForTheCurrentOrganizationService()
		{
			_directoryClientContext.SetServicePolicy(
				_directoryClientContext.LastCreatedService.Id,
				_directoryClientContext.LoadedServicePolicy
			);
		}

		[Then(@"the Directory Service Policy requires (.*) factors")]
		public void ThenTheOrganizationServicePolicyRequiresFactors(int numFactors)
		{
			Assert.IsTrue(_directoryClientContext.LoadedServicePolicy.RequiredFactors == numFactors);
		}

		[Given(@"the Directory Service Policy is set to require inherence")]
		[When(@"the Directory Service Policy is set to require inherence")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireInherence()
		{
			_directoryClientContext.LoadedServicePolicy.RequireInherenceFactor = true;
		}

		[Given(@"the Directory Service Policy is set to require knowledge")]
		[When(@"the Directory Service Policy is set to require knowledge")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireKnowledge()
		{
			_directoryClientContext.LoadedServicePolicy.RequireKnowledgeFactor = true;
		}

		[Given(@"the Directory Service Policy is set to require possession")]
		[When(@"the Directory Service Policy is set to require possession")]
		public void GivenTheOrganizationServicePolicyIsSetToRequirePossession()
		{
			_directoryClientContext.LoadedServicePolicy.RequirePossessionFactor = true;
		}

		[Then(@"the Directory Service Policy does require inherence")]
		public void ThenTheOrganizationServicePolicyDoesRequireInherence()
		{
			Assert.IsTrue(_directoryClientContext.LoadedServicePolicy.RequireInherenceFactor == true);
		}

		[Then(@"the Directory Service Policy does require knowledge")]
		public void ThenTheOrganizationServicePolicyDoesRequireKnowledge()
		{
			Assert.IsTrue(_directoryClientContext.LoadedServicePolicy.RequireKnowledgeFactor == true);
		}

		[Then(@"the Directory Service Policy does require possession")]
		public void ThenTheOrganizationServicePolicyDoesRequirePossession()
		{
			Assert.IsTrue(_directoryClientContext.LoadedServicePolicy.RequirePossessionFactor == true);
		}

		[Given(@"the Directory Service Policy is set to require jail break protection")]
		[When(@"the Directory Service Policy is set to require jail break protection")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireJailBreakProtection()
		{
			_directoryClientContext.LoadedServicePolicy.JailbreakDetection = true;
		}
		
		[Then(@"the Directory Service Policy does require jail break protection")]
		public void ThenTheOrganizationServicePolicyDoesRequireJailBreakProtection()
		{
			Assert.IsTrue(_directoryClientContext.LoadedServicePolicy.JailbreakDetection == true);
		}
		

		[Given(@"the Directory Service Policy is set to have the following Time Fences:")]
		[When(@"the Directory Service Policy is set to have the following Time Fences:")]
		public void GivenTheOrganizationServicePolicyIsSetToHaveTheFollowingTimeFences(Table table)
		{
			_directoryClientContext.LoadedServicePolicy.TimeFences = TableUtils.TimeFencesFromTable(table);
		}

		[Then(@"the Directory Service Policy has the following Time Fences:")]
		public void GivenTheOrganizationServicePolicyHasTheFollowingTimeFences(Table table)
		{
			var timeFences = TableUtils.TimeFencesFromTable(table);
			_directoryClientContext.LoadedServicePolicy.TimeFences.ShouldCompare(timeFences);
		}

		[Given(@"the Directory Service Policy is set to have the following Geofence locations:")]
		[When(@"the Directory Service Policy is set to have the following Geofence locations:")]
		public void GivenTheOrganizationServicePolicyIsSetToHaveTheFollowingGeofenceLocations(Table table)
		{
			_directoryClientContext.LoadedServicePolicy.Locations = TableUtils.LocationsFromTable(table);
		}

		[Then(@"the Directory Service Policy has the following Geofence locations:")]
		public void GivenTheOrganizationServicePolicyHasTheFollowingGeofenceLocations(Table table)
		{
			var locations = TableUtils.LocationsFromTable(table);
			_directoryClientContext.LoadedServicePolicy.Locations.ShouldCompare(locations);
		}

		[When(@"I attempt to retrieve the Policy for the Directory Service with the ID ""(.*)""")]
		public void WhenIAttemptToRetrieveThePolicyForTheOrganizationServiceWithTheID(string serviceId)
		{
			try
			{
				_directoryClientContext.SetServicePolicy(
					Guid.Parse(serviceId),
					new ServicePolicy()
				);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I remove the Policy for the Directory Service")]
		public void WhenIRemoveThePolicyForTheOrganizationService()
		{
			_directoryClientContext.RemoveServicePolicy(_directoryClientContext.LastCreatedService.Id);
		}

		[Given(@"the Directory Service Policy is set to require (.*) factor")]
		[When(@"the Directory Service Policy is set to require (.*) factors")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireFactor(int p0)
		{
			_directoryClientContext.LoadedServicePolicy.RequiredFactors = p0;
		}

		[When(@"I attempt to remove the Policy for the Directory Service with the ID ""(.*)""")]
		public void WhenIAttemptToRemoveThePolicyForTheOrganizationServiceWithTheID(string p0)
		{
			try
			{
				_directoryClientContext.RemoveServicePolicy(Guid.Parse(p0));
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I attempt to set the Policy for the Directory Service with the ID ""(.*)""")]
		public void WhenIAttemptToSetThePolicyForTheOrganizationServiceWithTheID(string p0)
		{
			try
			{
				_directoryClientContext.SetServicePolicy(Guid.Parse(p0), new ServicePolicy());
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[Then(@"the Directory Service Policy has (.*) locations")]
		public void ThenTheOrganizationServicePolicyHasLocations(int p0)
		{
			Assert.AreEqual(p0, _directoryClientContext.LoadedServicePolicy.Locations.Count);
		}

		[Then(@"the Directory Service Policy has (.*) time fences")]
		public void ThenTheOrganizationServicePolicyHasTimeFences(int p0)
		{
			Assert.AreEqual(p0, _directoryClientContext.LoadedServicePolicy.TimeFences.Count);
		}

		[Then(@"the Directory Service Policy has no requirement for jail break protection")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForJailBreakProtection()
		{
			Assert.IsTrue(_directoryClientContext.LoadedServicePolicy.JailbreakDetection == null);
		}
	}
}
