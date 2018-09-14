using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{

	class TimeFenceTableRow
	{
		public string Name { get; set; }
		public string Days { get; set; }
		public int StartHour { get; set; }
		public int StartMinute { get; set; }
		public int EndHour { get; set; }
		public int EndMinute { get; set; }
		public string TimeZone { get; set; }
	}

	class GeofenceTableRow
	{
		public string Name { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Radius { get; set; }
	}

	[Binding]
	public class OrganizationServicePolicySteps
	{
		private readonly CommonContext _commonContext;
		private readonly OrgClientContext _orgClientContext;

		public OrganizationServicePolicySteps(CommonContext commonContext, OrgClientContext orgClientContext)
		{
			_commonContext = commonContext;
			_orgClientContext = orgClientContext;
		}

		[When(@"I retrieve the Policy for the Current Organization Service")]
		[Given(@"I retrieve the Policy for the Current Organization Service")]
		public void WhenIRetrieveThePolicyForTheCurrentOrganizationService()
		{
			_orgClientContext.LoadServicePolicy(_orgClientContext.LastCreatedService.Id);
		}

		[Then(@"the Organization Service Policy has no requirement for inherence")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForInherence()
		{
			Assert.IsNull(_orgClientContext.LoadedServicePolicy.RequireInherenceFactor);
		}

		[Then(@"the Organization Service Policy has no requirement for knowledge")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForKnowledge()
		{
			Assert.IsNull(_orgClientContext.LoadedServicePolicy.RequireKnowledgeFactor);
		}

		[Then(@"the Organization Service Policy has no requirement for possession")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForPossession()
		{
			Assert.IsNull(_orgClientContext.LoadedServicePolicy.RequirePossessionFactor);
		}

		[Then(@"the Organization Service Policy has no requirement for number of factors")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForNumberOfFactors()
		{
			Assert.IsNull(_orgClientContext.LoadedServicePolicy.RequiredFactors);
		}

		[Given(@"the Organization Service Policy is set to require (.*) factors")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireFactors(int numFactors)
		{
			_orgClientContext.LoadedServicePolicy.RequiredFactors = numFactors;
		}

		[Given(@"I set the Policy for the Organization Service")]
		[Given(@"I set the Policy for the Current Organization Service")]
		[When(@"I set the Policy for the Current Organization Service")]
		public void GivenISetThePolicyForTheCurrentOrganizationService()
		{
			_orgClientContext.SetServicePolicy(
				_orgClientContext.LastCreatedService.Id,
				_orgClientContext.LoadedServicePolicy
			);
		}

		[Then(@"the Organization Service Policy requires (.*) factors")]
		public void ThenTheOrganizationServicePolicyRequiresFactors(int numFactors)
		{
			Assert.IsTrue(_orgClientContext.LoadedServicePolicy.RequiredFactors == numFactors);
		}

		[Given(@"the Organization Service Policy is set to require inherence")]
		[When(@"the Organization Service Policy is set to require inherence")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireInherence()
		{
			_orgClientContext.LoadedServicePolicy.RequireInherenceFactor = true;
		}

		[Given(@"the Organization Service Policy is set to require knowledge")]
		[When(@"the Organization Service Policy is set to require knowledge")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireKnowledge()
		{
			_orgClientContext.LoadedServicePolicy.RequireKnowledgeFactor = true;
		}

		[Given(@"the Organization Service Policy is set to require possession")]
		[When(@"the Organization Service Policy is set to require possession")]
		public void GivenTheOrganizationServicePolicyIsSetToRequirePossession()
		{
			_orgClientContext.LoadedServicePolicy.RequirePossessionFactor = true;
		}

		[Then(@"the Organization Service Policy does require inherence")]
		public void ThenTheOrganizationServicePolicyDoesRequireInherence()
		{
			Assert.IsTrue(_orgClientContext.LoadedServicePolicy.RequireInherenceFactor == true);
		}

		[Then(@"the Organization Service Policy does require knowledge")]
		public void ThenTheOrganizationServicePolicyDoesRequireKnowledge()
		{
			Assert.IsTrue(_orgClientContext.LoadedServicePolicy.RequireKnowledgeFactor == true);
		}

		[Then(@"the Organization Service Policy does require possession")]
		public void ThenTheOrganizationServicePolicyDoesRequirePossession()
		{
			Assert.IsTrue(_orgClientContext.LoadedServicePolicy.RequirePossessionFactor == true);
		}

		[Given(@"the Organization Service Policy is set to require jail break protection")]
		[When(@"the Organization Service Policy is set to require jail break protection")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireJailBreakProtection()
		{
			_orgClientContext.LoadedServicePolicy.JailbreakDetection = true;
		}
		
		[Then(@"the Organization Service Policy does require jail break protection")]
		public void ThenTheOrganizationServicePolicyDoesRequireJailBreakProtection()
		{
			Assert.IsTrue(_orgClientContext.LoadedServicePolicy.JailbreakDetection == true);
		}

		private List<TimeFence> TimeFencesFromTable(Table table)
		{
			var timeFences = new List<TimeFence>();

			foreach (var row in table.CreateSet<TimeFenceTableRow>())
			{
				timeFences.Add(new TimeFence(
					row.Name,
					row.Days.Split(',').Select(d => (DayOfWeek) Enum.Parse(typeof(DayOfWeek), d)).ToList(),
					row.StartHour,
					row.StartMinute,
					row.EndHour,
					row.EndMinute,
					row.TimeZone
				));
			}

			return timeFences;
		}

		private List<Location> LocationsFromTable(Table table)
		{
			var locations = new List<Location>();

			foreach (var row in table.CreateSet<GeofenceTableRow>())
			{
				locations.Add(new Location(row.Name, row.Radius, row.Latitude, row.Longitude));
			}

			return locations;
		}

		[Given(@"the Organization Service Policy is set to have the following Time Fences:")]
		[When(@"the Organization Service Policy is set to have the following Time Fences:")]
		public void GivenTheOrganizationServicePolicyIsSetToHaveTheFollowingTimeFences(Table table)
		{
			_orgClientContext.LoadedServicePolicy.TimeFences = TimeFencesFromTable(table);
		}

		[Then(@"the Organization Service Policy has the following Time Fences:")]
		public void GivenTheOrganizationServicePolicyHasTheFollowingTimeFences(Table table)
		{
			var timeFences = TimeFencesFromTable(table);
			_orgClientContext.LoadedServicePolicy.TimeFences.ShouldCompare(timeFences);
		}

		[Given(@"the Organization Service Policy is set to have the following Geofence locations:")]
		[When(@"the Organization Service Policy is set to have the following Geofence locations:")]
		public void GivenTheOrganizationServicePolicyIsSetToHaveTheFollowingGeofenceLocations(Table table)
		{
			_orgClientContext.LoadedServicePolicy.Locations = LocationsFromTable(table);
		}

		[Then(@"the Organization Service Policy has the following Geofence locations:")]
		public void GivenTheOrganizationServicePolicyHasTheFollowingGeofenceLocations(Table table)
		{
			var locations = LocationsFromTable(table);
			_orgClientContext.LoadedServicePolicy.Locations.ShouldCompare(locations);
		}

		[When(@"I attempt to retrieve the Policy for the Organization Service with the ID ""(.*)""")]
		public void WhenIAttemptToRetrieveThePolicyForTheOrganizationServiceWithTheID(string serviceId)
		{
			try
			{
				_orgClientContext.SetServicePolicy(
					Guid.Parse(serviceId),
					new ServicePolicy()
				);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I remove the Policy for the Organization Service")]
		public void WhenIRemoveThePolicyForTheOrganizationService()
		{
			_orgClientContext.RemoveServicePolicy(_orgClientContext.LastCreatedService.Id);
		}

		[Given(@"the Organization Service Policy is set to require (.*) factor")]
		[When(@"the Organization Service Policy is set to require (.*) factors")]
		public void GivenTheOrganizationServicePolicyIsSetToRequireFactor(int p0)
		{
			_orgClientContext.LoadedServicePolicy.RequiredFactors = p0;
		}

		[When(@"I attempt to remove the Policy for the Organization Service with the ID ""(.*)""")]
		public void WhenIAttemptToRemoveThePolicyForTheOrganizationServiceWithTheID(string p0)
		{
			try
			{
				_orgClientContext.RemoveServicePolicy(Guid.Parse(p0));
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I attempt to set the Policy for the Organization Service with the ID ""(.*)""")]
		public void WhenIAttemptToSetThePolicyForTheOrganizationServiceWithTheID(string p0)
		{
			try
			{
				_orgClientContext.SetServicePolicy(Guid.Parse(p0), new ServicePolicy());
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[Then(@"the Organization Service Policy has (.*) locations")]
		public void ThenTheOrganizationServicePolicyHasLocations(int p0)
		{
			Assert.AreEqual(p0, _orgClientContext.LoadedServicePolicy.Locations.Count);
		}

		[Then(@"the Organization Service Policy has (.*) time fences")]
		public void ThenTheOrganizationServicePolicyHasTimeFences(int p0)
		{
			Assert.AreEqual(p0, _orgClientContext.LoadedServicePolicy.TimeFences.Count);
		}

		[Then(@"the Organization Service Policy has no requirement for jail break protection")]
		public void ThenTheOrganizationServicePolicyHasNoRequirementForJailBreakProtection()
		{
			Assert.IsTrue(_orgClientContext.LoadedServicePolicy.JailbreakDetection == null);
		}
	}
}
