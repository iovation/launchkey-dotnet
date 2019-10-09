using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
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


        [Given(@"the Organization Service Policy is set to have the following Time Fences:")]
        [When(@"the Organization Service Policy is set to have the following Time Fences:")]
        public void GivenTheOrganizationServicePolicyIsSetToHaveTheFollowingTimeFences(Table table)
        {
            _orgClientContext.LoadedServicePolicy.TimeFences = TableUtils.TimeFencesFromTable(table);
        }

        [Then(@"the Organization Service Policy has the following Time Fences:")]
        public void GivenTheOrganizationServicePolicyHasTheFollowingTimeFences(Table table)
        {
            var timeFences = TableUtils.TimeFencesFromTable(table);
            _orgClientContext.LoadedServicePolicy.TimeFences.ShouldCompare(timeFences);
        }

        [Given(@"the Organization Service Policy is set to have the following Geofence locations:")]
        [When(@"the Organization Service Policy is set to have the following Geofence locations:")]
        public void GivenTheOrganizationServicePolicyIsSetToHaveTheFollowingGeofenceLocations(Table table)
        {
            _orgClientContext.LoadedServicePolicy.Locations = TableUtils.LocationsFromTable(table);
        }

        [Then(@"the Organization Service Policy has the following Geofence locations:")]
        public void GivenTheOrganizationServicePolicyHasTheFollowingGeofenceLocations(Table table)
        {
            var locations = TableUtils.LocationsFromTable(table);
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

        //==============================NEW STUFF HERE =========================
        //==============================NEW STUFF HERE =========================
        //==============================NEW STUFF HERE =========================
        //==============================NEW STUFF HERE =========================

        [When(@"I create a new MethodAmountPolicy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenICreateANewMethodAmountPolicy()
        {
            _orgClientContext.CreateMethodAmountPolicy();
        }

        [When(@"I add the following GeoCircleFence items")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenIAddTheFollowingGeoCircleFenceItems(Table table)
        {
            List<IFence> geocirclefences = TableUtils.GeoCircleFenceFromTable(table);
            _orgClientContext.AddIFenceToAdvancedPolicy(geocirclefences);
        }

        [When(@"I add the following TerritoryFence items")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenIAddTheFollowingTerritoryFenceItems(Table table)
        {
            List<IFence> territoryfences = TableUtils.TerritoryFenceFromTable(table);
            _orgClientContext.AddIFenceToAdvancedPolicy(territoryfences);
        }

        [When(@"I set the Policy for the Current Organization Service to the new policy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetThePolicyForTheCurrentOrganizationServiceToTheNewPolicy()
        {
            _orgClientContext.SetAdvancedServicePolicy(
                _orgClientContext.LastCreatedService.Id,
                _orgClientContext.LoadedAdvancedServicePolicy
            );
        }

        [Then(@"the Organization Service Policy has ""(.*)"" fences")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheOrganizationServicePolicyHasFences(int p0)
        {
            Assert.AreEqual(p0, _orgClientContext.LoadedAdvancedServicePolicy.Fences.Count);
        }

        [When(@"I retrieve the Advanced Policy for the Current Organization Service")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenIRetrieveTheAdvancedPolicyForTheCurrentOrganizationService()
        {
            _orgClientContext.LoadAdvancedServicePolicy(_orgClientContext.LastCreatedService.Id);
        }


        [When(@"I set the amount to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetTheAmountTo(int p0)
        {
            _orgClientContext.SetAmountOnMethodAmountPolicy(p0);
        }

        [Then(@"the amount should be set to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheAmountShouldBeSetTo(int p0)
        {

            Assert.AreEqual(p0, (_orgClientContext.LoadedAdvancedServicePolicy as MethodAmountPolicy)?.Amount);
        }

        [When(@"I create a new Factors Policy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenICreateANewFactorsPolicy()
        {
            _orgClientContext.CreateFactorsPolicy();
        }

        [When(@"I set the factors to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void OrgClientWhenISetTheFactorsTo(string factors)
        {
            bool requireKnowledge = factors.ToLower().Contains("knowledge");
            bool requirePossession = factors.ToLower().Contains("possession");
            bool requireInherence = factors.ToLower().Contains("inherence");
            _orgClientContext.SetFactors(requireKnowledge, requirePossession, requireInherence);
        }

        [Then(@"factors should be set to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenFactorsShouldBeSetTo(string factors)
        {
            if (factors.ToLower().Contains("knowledge"))
            {
                Assert.AreEqual(true, (_orgClientContext.LoadedAdvancedServicePolicy as FactorsPolicy)?.RequireKnowledgeFactor);
            }
            else if (factors.ToLower().Contains("possession"))
            {
                Assert.AreEqual(true, (_orgClientContext.LoadedAdvancedServicePolicy as FactorsPolicy)?.RequirePossessionFactor);
            }
            else if (factors.ToLower().Contains("inherence"))
            {
                Assert.AreEqual(true, (_orgClientContext.LoadedAdvancedServicePolicy as FactorsPolicy)?.RequireInherenceFactor);
            }
        }

        [When(@"I set deny_rooted_jailbroken to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetDeny_Rooted_JailbrokenTo(bool value)
        {
            _orgClientContext.SetDenyRootedJailbroken(value);
        }

        [Then(@"deny_rooted_jailbroken should be set to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenDeny_Rooted_JailbrokenShouldBeSetTo(bool value)
        {
            Assert.AreEqual(value, _orgClientContext.LoadedAdvancedServicePolicy.DenyRootedJailbroken);
        }

        [When(@"I set deny_emulator_simulator to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetDeny_Emulator_SimulatorTo(bool value)
        {
            _orgClientContext.SetDenyEmulatorSimulator(value);
        }

        [Then(@"deny_emulator_simulator should be set to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenDeny_Emulator_SimulatorShouldBeSetTo(bool value)
        {
            Assert.AreEqual(value, _orgClientContext.LoadedAdvancedServicePolicy.DenyEmulatorSimulator);
        }

        [Given(@"the Organization Service is set to any Conditional Geofence Policy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void GivenTheOrganizationServiceIsSetToAnyConditionalGeofencePolicy()
        {
            _orgClientContext.CreateConditionaGeofence();
        }

        [When(@"I set the inside Policy to a new Factors Policy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetTheInsidePolicyToANewFactorsPolicy()
        {
            FactorsPolicy insidePolicy = new FactorsPolicy(null, true);
            _orgClientContext.SetInsideConditionalGeofencePolicy(insidePolicy);
        }

        [When(@"I set the inside Policy factors to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetTheInsidePolicyFactorsTo(string factors)
        {
            FactorsPolicy currentPolicy = ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside as FactorsPolicy);
            bool requireKnowledge = factors.ToLower().Contains("knowledge");
            bool requirePossession = factors.ToLower().Contains("possession");
            bool requireInherence = factors.ToLower().Contains("inherence");
            FactorsPolicy newPolicy = new FactorsPolicy(
                fences: currentPolicy.Fences,
                requireKnowledgeFactor: requireKnowledge,
                requirePossessionFactor: requirePossession,
                requireInherenceFactor: requireInherence,
                denyRootedJailbroken: currentPolicy.DenyRootedJailbroken,
                denyEmulatorSimulator: currentPolicy.DenyEmulatorSimulator
            );
            _orgClientContext.SetInsideConditionalGeofencePolicy(newPolicy);
        }

        [Then(@"the inside Policy should be a FactorsPolicy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheInsidePolicyShouldBeAFactorsPolicy()
        {
            Assert.IsInstanceOfType((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside, typeof(FactorsPolicy));
        }

        [When(@"I set the inside Policy to a new MethodAmountPolicy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetTheInsidePolicyToANewMethodAmountPolicy()
        {
            MethodAmountPolicy insidePolicy = new MethodAmountPolicy(null, 0);
            _orgClientContext.SetInsideConditionalGeofencePolicy(insidePolicy);
        }

        [When(@"I set the inside Policy amount to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetTheInsidePolicyAmountTo(int amount)
        {
            MethodAmountPolicy currentPolicy = ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside as MethodAmountPolicy);
            MethodAmountPolicy newPolicy = new MethodAmountPolicy(
                fences: currentPolicy.Fences,
                amount: amount,
                denyRootedJailbroken: currentPolicy.DenyRootedJailbroken,
                denyEmulatorSimulator: currentPolicy.DenyEmulatorSimulator
            );
            _orgClientContext.SetInsideConditionalGeofencePolicy(newPolicy);
        }

        [Then(@"the inside Policy should be a MethodAmountPolicy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheInsidePolicyShouldBeAMethodAmountPolicy()
        {
            Assert.IsInstanceOfType((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside, typeof(MethodAmountPolicy));
        }

        [When(@"I set the outside Policy to a new Factors Policy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetTheOutsidePolicyToANewFactorsPolicy()
        {
            FactorsPolicy insidePolicy = new FactorsPolicy(null, true);
            _orgClientContext.SetOutsideConditionalGeofencePolicy(insidePolicy);
        }

        [When(@"I set the outside Policy factors to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetTheOutsidePolicyFactorsTo(string factors)
        {
            FactorsPolicy currentPolicy = ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside as FactorsPolicy);
            bool requireKnowledge = factors.ToLower().Contains("knowledge");
            bool requirePossession = factors.ToLower().Contains("possession");
            bool requireInherence = factors.ToLower().Contains("inherence");
            FactorsPolicy newPolicy = new FactorsPolicy(
                fences: currentPolicy.Fences,
                requireKnowledgeFactor: requireKnowledge,
                requirePossessionFactor: requirePossession,
                requireInherenceFactor: requireInherence,
                denyRootedJailbroken: currentPolicy.DenyRootedJailbroken,
                denyEmulatorSimulator: currentPolicy.DenyEmulatorSimulator
            );
            _orgClientContext.SetOutsideConditionalGeofencePolicy(newPolicy);
        }

        [Then(@"the outside Policy should be a FactorsPolicy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheOutsidePolicyShouldBeAFactorsPolicy()
        {
            Assert.IsInstanceOfType((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside, typeof(FactorsPolicy));
        }

        [When(@"I set the outside Policy to a new MethodAmountPolicy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetTheOutsidePolicyToANewMethodAmountPolicy()
        {
            MethodAmountPolicy outsidePolicy = new MethodAmountPolicy(null, 0);
            _orgClientContext.SetOutsideConditionalGeofencePolicy(outsidePolicy);
        }

        [When(@"I set the outside Policy amount to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void WhenISetTheOutsidePolicyAmountTo(int amount)
        {
            MethodAmountPolicy currentPolicy = ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside as MethodAmountPolicy);
            MethodAmountPolicy newPolicy = new MethodAmountPolicy(
                fences: currentPolicy.Fences,
                amount: amount,
                denyRootedJailbroken: currentPolicy.DenyRootedJailbroken,
                denyEmulatorSimulator: currentPolicy.DenyEmulatorSimulator
            );
            _orgClientContext.SetOutsideConditionalGeofencePolicy(newPolicy);
        }

        [Then(@"the outside Policy should be a MethodAmountPolicy")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheOutsidePolicyShouldBeAMethodAmountPolicy()
        {
            Assert.IsInstanceOfType((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside, typeof(MethodAmountPolicy));
        }

        private IFence GetFenceFromPolicy(string fenceName)
        {
            foreach (var fence in _orgClientContext.LoadedAdvancedServicePolicy.Fences)
            {
                if (fence.Name == fenceName)
                {
                    return fence;
                }
            }

            return null;
        }

        [Then(@"the Organization Service Policy contains the GeoCircleFence ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheOrganizationServicePolicyContainsTheGeoCircleFence(string geofenceName)
        {
            IFence fence = GetFenceFromPolicy(geofenceName);
            Assert.IsNotNull(fence);
            Assert.IsInstanceOfType(fence, typeof(GeoCircleFence));
        }

        [Then(@"the ""(.*)"" fence has a latitude of ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheFenceHasALatitudeOf(string geofenceName, double latitude)
        {
            IFence fence = GetFenceFromPolicy(geofenceName);
            Assert.IsNotNull(fence);
            Assert.AreEqual(latitude, (fence as GeoCircleFence).Latitude);
        }

        [Then(@"the ""(.*)"" fence has a longitude of ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheFenceHasALongitudeOf(string geofenceName, double longitude)
        {
            IFence fence = GetFenceFromPolicy(geofenceName);
            Assert.IsNotNull(fence);
            Assert.AreEqual(longitude, (fence as GeoCircleFence).Longitude);
        }

        [Then(@"the ""(.*)"" fence has a radius of ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheFenceHasARadiusOf(string geofenceName, int radius)
        {
            IFence fence = GetFenceFromPolicy(geofenceName);
            Assert.IsNotNull(fence);
            Assert.AreEqual(radius, (fence as GeoCircleFence).Radius);
        }

        [Then(@"the Organization Service Policy contains the TerritoryFence ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheOrganizationServicePolicyContainsTheTerritoryFence(string geofenceName)
        {
            IFence fence = GetFenceFromPolicy(geofenceName);
            Assert.IsNotNull(fence);
            Assert.IsInstanceOfType(fence, typeof(TerritoryFence));
        }

        [Then(@"the ""(.*)"" fence has a country of ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheFenceHasACountryOf(string geofenceName, string country)
        {
            IFence fence = GetFenceFromPolicy(geofenceName);
            Assert.IsNotNull(fence);
            Assert.AreEqual(country, (fence as TerritoryFence).Country);
        }

        [Then(@"the ""(.*)"" fence has an administrative_area of ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheFenceHasAnAdministrative_AreaOf(string geofenceName, string adminArea)
        {
            IFence fence = GetFenceFromPolicy(geofenceName);
            Assert.IsNotNull(fence);
            Assert.AreEqual(adminArea, (fence as TerritoryFence).AdministrativeArea);
        }

        [Then(@"the ""(.*)"" fence has a postal_code of ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheFenceHasAPostal_CodeOf(string geofenceName, string postalCode)
        {
            IFence fence = GetFenceFromPolicy(geofenceName);
            Assert.IsNotNull(fence);
            Assert.AreEqual(postalCode, (fence as TerritoryFence).PostalCode);
        }

        [Then(@"the inside Policy factors should be set to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheInsidePolicyFactorsShouldBeSetTo(string factors)
        {
            bool requiredKnowledge = factors.ToLower().Contains("knowledge");
            bool requiredPossession = factors.ToLower().Contains("possession");
            bool requiredInherence = factors.ToLower().Contains("inherence");

            Assert.AreEqual(requiredKnowledge, ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside as FactorsPolicy).RequireKnowledgeFactor);
            Assert.AreEqual(requiredPossession, ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside as FactorsPolicy).RequirePossessionFactor);
            Assert.AreEqual(requiredInherence, ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside as FactorsPolicy).RequireInherenceFactor);
        }

        [Then(@"the inside Policy amount should be set to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheInsidePolicyAmountShouldBeSetTo(int amount)
        {
            Assert.AreEqual(amount, ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside as MethodAmountPolicy).Amount);
        }

        [Then(@"the Organization Service Policy has ""(.*)"" fence")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheOrganizationServicePolicyHasFence(int numOfFences)
        {
            Assert.AreEqual(numOfFences, _orgClientContext.LoadedAdvancedServicePolicy.Fences.Count);
        }

        [Then(@"the outside Policy factors should be set to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheOutsidePolicyFactorsShouldBeSetTo(string factors)
        {
            bool requiredKnowledge = factors.ToLower().Contains("knowledge");
            bool requiredPossession = factors.ToLower().Contains("possession");
            bool requiredInherence = factors.ToLower().Contains("inherence");

            Assert.AreEqual(requiredKnowledge, ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside as FactorsPolicy).RequireKnowledgeFactor);
            Assert.AreEqual(requiredPossession, ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside as FactorsPolicy).RequirePossessionFactor);
            Assert.AreEqual(requiredInherence, ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside as FactorsPolicy).RequireInherenceFactor);
        }

        [Then(@"the outside Policy amount should be set to ""(.*)""")]
        [Scope(Feature = "Organization Client can retrieve Organization Service Policy")]
        [Scope(Feature = "Organization Client can set Organization Service Policy")]
        public void ThenTheOutsidePolicyAmountShouldBeSetTo(int amount)
        {
            Assert.AreEqual(amount, ((_orgClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside as MethodAmountPolicy).Amount);
        }

    }
}
