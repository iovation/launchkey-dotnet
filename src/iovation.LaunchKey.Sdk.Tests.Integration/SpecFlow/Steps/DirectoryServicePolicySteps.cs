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

        [When(@"I create a new MethodAmountPolicy")]
        public void WhenICreateANewMethodAmountPolicy()
        {
            _directoryClientContext.CreateMethodAmountPolicy();
        }

        [When(@"I add the following GeoCircleFence items")]
        public void WhenIAddTheFollowingGeoCircleFenceItems(Table table)
        {
            List<IFence> geocirclefences = TableUtils.GeoCircleFenceFromTable(table);
            _directoryClientContext.AddIFenceToAdvancedPolicy(geocirclefences);
        }

        [When(@"I add the following TerritoryFence items")]
        public void WhenIAddTheFollowingTerritoryFenceItems(Table table)
        {
            List<IFence> territoryfences = TableUtils.TerritoryFenceFromTable(table);
            _directoryClientContext.AddIFenceToAdvancedPolicy(territoryfences);
        }

        [When(@"I set the Policy for the Current Directory Service to the new policy")]
        public void WhenISetThePolicyForTheCurrentDirectoryServiceToTheNewPolicy()
        {
            _directoryClientContext.SetAdvancedServicePolicy(
                _directoryClientContext.LastCreatedService.Id,
                _directoryClientContext.LoadedAdvancedServicePolicy
            );
        }

        [Then(@"the Directory Service Policy has ""(.*)"" fences")]
        public void ThenTheDirectoryServicePolicyHasFences(int p0)
        {
            Assert.AreEqual(p0, _directoryClientContext.LoadedAdvancedServicePolicy.Fences.Count);
        }

        [When(@"I retrieve the Advanced Policy for the Current Directory Service")]
        public void WhenIRetrieveTheAdvancedPolicyForTheCurrentDirectoryService()
        {
            _directoryClientContext.LoadAdvancedServicePolicy(_directoryClientContext.LastCreatedService.Id);
        }


        [When(@"I set the amount to ""(.*)""")]
        public void WhenISetTheAmountTo(int p0)
        {
            _directoryClientContext.SetAmountOnMethodAmountPolicy(p0);
        }

        [Then(@"the amount should be set to ""(.*)""")]
        public void ThenTheAmountShouldBeSetTo(int p0)
        {

            Assert.AreEqual(p0, (_directoryClientContext.LoadedAdvancedServicePolicy as MethodAmountPolicy)?.Amount);
        }

        [When(@"I create a new Factors Policy")]
        public void WhenICreateANewFactorsPolicy()
        {
            _directoryClientContext.CreateFactorsPolicy();
        }

        [When(@"I set the factors to ""(.*)""")]
        public void WhenISetTheFactorsTo(string factors)
        {
            bool requireKnowledge = factors.ToLower().Contains("knowledge");
            bool requirePossession = factors.ToLower().Contains("possession");
            bool requireInherence = factors.ToLower().Contains("inherence");
            _directoryClientContext.SetFactors(requireKnowledge, requirePossession, requireInherence);
        }

        [Then(@"factors should be set to ""(.*)""")]
        public void ThenFactorsShouldBeSetTo(string factors)
        {
            if(factors.ToLower().Contains("knowledge"))
            {
                Assert.AreEqual(true, (_directoryClientContext.LoadedAdvancedServicePolicy as FactorsPolicy)?.RequireKnowledgeFactor);
            }
            else if (factors.ToLower().Contains("possession"))
            {
                Assert.AreEqual(true, (_directoryClientContext.LoadedAdvancedServicePolicy as FactorsPolicy)?.RequirePossessionFactor);
            }
            else if (factors.ToLower().Contains("inherence"))
            {
                Assert.AreEqual(true, (_directoryClientContext.LoadedAdvancedServicePolicy as FactorsPolicy)?.RequireInherenceFactor);
            }
        }

        [When(@"I set deny_rooted_jailbroken to ""(.*)""")]
        public void WhenISetDeny_Rooted_JailbrokenTo(bool value)
        {
            _directoryClientContext.SetDenyRootedJailbroken(value);
        }

        [Then(@"deny_rooted_jailbroken should be set to ""(.*)""")]
        public void ThenDeny_Rooted_JailbrokenShouldBeSetTo(bool value)
        {
            Assert.AreEqual(value, _directoryClientContext.LoadedAdvancedServicePolicy.DenyRootedJailbroken);
        }

        [When(@"I set deny_emulator_simulator to ""(.*)""")]
        public void WhenISetDeny_Emulator_SimulatorTo(bool value)
        {
            _directoryClientContext.SetDenyEmulatorSimulator(value);
        }

        [Then(@"deny_emulator_simulator should be set to ""(.*)""")]
        public void ThenDeny_Emulator_SimulatorShouldBeSetTo(bool value)
        {
            Assert.AreEqual(value, _directoryClientContext.LoadedAdvancedServicePolicy.DenyEmulatorSimulator);
        }

        [Given(@"the Directory Service is set to any Conditional Geofence Policy")]
        public void GivenTheDirectoryServiceIsSetToAnyConditionalGeofencePolicy()
        {
            _directoryClientContext.CreateConditionaGeofence();
        }

        [When(@"I set the inside Policy to a new Factors Policy")]
        public void WhenISetTheInsidePolicyToANewFactorsPolicy()
        {
            FactorsPolicy insidePolicy = new FactorsPolicy(null,true);
            _directoryClientContext.SetInsideConditionalGeofencePolicy(insidePolicy);
        }

        [When(@"I set the inside Policy factors to ""(.*)""")]
        public void WhenISetTheInsidePolicyFactorsTo(string factors)
        {
            FactorsPolicy currentPolicy = ((_directoryClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside as FactorsPolicy);
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
            _directoryClientContext.SetInsideConditionalGeofencePolicy(newPolicy);
        }

        [Then(@"the inside Policy should be a FactorsPolicy")]
        public void ThenTheInsidePolicyShouldBeAFactorsPolicy()
        {
            Assert.IsInstanceOfType((_directoryClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside, typeof(FactorsPolicy));
        }

        [When(@"I set the inside Policy to a new MethodAmountPolicy")]
        public void WhenISetTheInsidePolicyToANewMethodAmountPolicy()
        {
            MethodAmountPolicy insidePolicy = new MethodAmountPolicy(null, 0);
            _directoryClientContext.SetInsideConditionalGeofencePolicy(insidePolicy);
        }

        [When(@"I set the inside Policy amount to ""(.*)""")]
        public void WhenISetTheInsidePolicyAmountTo(int amount)
        {
            MethodAmountPolicy currentPolicy = ((_directoryClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside as MethodAmountPolicy);
            MethodAmountPolicy newPolicy = new MethodAmountPolicy(
                fences: currentPolicy.Fences,
                amount: amount,
                denyRootedJailbroken: currentPolicy.DenyRootedJailbroken,
                denyEmulatorSimulator: currentPolicy.DenyEmulatorSimulator
            );
            _directoryClientContext.SetInsideConditionalGeofencePolicy(newPolicy);
        }

        [Then(@"the inside Policy should be a MethodAmountPolicy")]
        public void ThenTheInsidePolicyShouldBeAMethodAmountPolicy()
        {
            Assert.IsInstanceOfType((_directoryClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Inside, typeof(MethodAmountPolicy));
        }

        [When(@"I set the outside Policy to a new Factors Policy")]
        public void WhenISetTheOutsidePolicyToANewFactorsPolicy()
        {
            FactorsPolicy insidePolicy = new FactorsPolicy(null, true);
            _directoryClientContext.SetOutsideConditionalGeofencePolicy(insidePolicy);
        }

        [When(@"I set the outside Policy factors to ""(.*)""")]
        public void WhenISetTheOutsidePolicyFactorsTo(string factors)
        {
            FactorsPolicy currentPolicy = ((_directoryClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside as FactorsPolicy);
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
            _directoryClientContext.SetOutsideConditionalGeofencePolicy(newPolicy);
        }

        [Then(@"the outside Policy should be a FactorsPolicy")]
        public void ThenTheOutsidePolicyShouldBeAFactorsPolicy()
        {
            Assert.IsInstanceOfType((_directoryClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside, typeof(FactorsPolicy));
        }

        [When(@"I set the outside Policy to a new MethodAmountPolicy")]
        public void WhenISetTheOutsidePolicyToANewMethodAmountPolicy()
        {
            MethodAmountPolicy outsidePolicy = new MethodAmountPolicy(null, 0);
            _directoryClientContext.SetOutsideConditionalGeofencePolicy(outsidePolicy);
        }

        [When(@"I set the outside Policy amount to ""(.*)""")]
        public void WhenISetTheOutsidePolicyAmountTo(int amount)
        {
            MethodAmountPolicy currentPolicy = ((_directoryClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside as MethodAmountPolicy);
            MethodAmountPolicy newPolicy = new MethodAmountPolicy(
                fences: currentPolicy.Fences,
                amount: amount,
                denyRootedJailbroken: currentPolicy.DenyRootedJailbroken,
                denyEmulatorSimulator: currentPolicy.DenyEmulatorSimulator
            );
            _directoryClientContext.SetOutsideConditionalGeofencePolicy(newPolicy);
        }

        [Then(@"the outside Policy should be a MethodAmountPolicy")]
        public void ThenTheOutsidePolicyShouldBeAMethodAmountPolicy()
        {
            Assert.IsInstanceOfType((_directoryClientContext.LoadedAdvancedServicePolicy as ConditionalGeoFencePolicy).Outside, typeof(MethodAmountPolicy));
        }
    }
}
