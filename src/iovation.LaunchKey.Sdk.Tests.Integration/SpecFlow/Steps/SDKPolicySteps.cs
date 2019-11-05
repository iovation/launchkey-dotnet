using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    [Scope(Feature = "SDK Policy Object Creation Limitations")]
    public class SDKPolicySteps
    {

        IPolicy policy;
        private CommonContext _commonContext;

        public SDKPolicySteps(CommonContext commonContext)
        {
            _commonContext = commonContext;
        }

        [When("I create a new Factors Policy")]
        public void WhenICreateANewFactorsPolicy()
        {
            policy = new FactorsPolicy(null);
        }

        [When("I create a new MethodAmountPolicy")]
        public void WhenICreateANewMethodPolicy()
        {
            policy = new MethodAmountPolicy(null);
        }

        [When(@"I set the amount to ""(.*)""")]
        public void WhenISetTheAmount(int amount)
        {
            policy = new MethodAmountPolicy(null, amount);
        }

        [When(@"I set the factors to ""(.*)""")]
        public void WhenISetTheFactorsTo(string factors)
        {
            bool requireKnowledge = factors.ToLower().Contains("knowledge");
            bool requirePossession = factors.ToLower().Contains("possession");
            bool requireInherence = factors.ToLower().Contains("inherence");

            policy = new FactorsPolicy(
                fences: (policy as FactorsPolicy).Fences,
                requireInherenceFactor: requireInherence,
                requireKnowledgeFactor: requireKnowledge,
                requirePossessionFactor: requirePossession,
                denyEmulatorSimulator: policy.DenyEmulatorSimulator,
                denyRootedJailbroken: policy.DenyRootedJailbroken
            );
        }

        [When(@"I set deny_emulator_simulator on the Factors Policy to True")]
        public void WhenISetDeny_Emulator_SimulatorOnThePolicyToTrue()
        {
            policy = new FactorsPolicy(
                fences: (policy as FactorsPolicy).Fences,
                requireInherenceFactor: (policy as FactorsPolicy).RequireInherenceFactor,
                requireKnowledgeFactor: (policy as FactorsPolicy).RequireKnowledgeFactor,
                requirePossessionFactor: (policy as FactorsPolicy).RequirePossessionFactor,
                denyEmulatorSimulator: true,
                denyRootedJailbroken: policy.DenyRootedJailbroken
            );
        }

        [When(@"I set deny_rooted_jailbroken on the Factors Policy to True")]
        public void WhenISetDeny_Rooted_JailbrokenOnThePolicyToTrue()
        {
            policy = new FactorsPolicy(
                fences: (policy as FactorsPolicy).Fences,
                requireInherenceFactor: (policy as FactorsPolicy).RequireInherenceFactor,
                requireKnowledgeFactor: (policy as FactorsPolicy).RequireKnowledgeFactor,
                requirePossessionFactor: (policy as FactorsPolicy).RequirePossessionFactor,
                denyEmulatorSimulator: policy.DenyEmulatorSimulator,
                denyRootedJailbroken: true
            );
        }

        [When(@"I attempt to create a new Conditional Geofence Policy with the inside policy set to the new policy")]
        public void WhenIAttemptToCreateANewConditionalGeofencePolicyWithTheInsidePolicySetToTheNewPolicy()
        {
            try
            {
                policy = new ConditionalGeoFencePolicy(
                    inside: policy,
                    outside: new MethodAmountPolicy(null),
                    fences: null,
                    denyRootedJailbroken: false,
                    denyEmulatorSimulator: false
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to create a new Conditional Geofence Policy with the outside policy set to the new policy")]
        public void WhenIAttemptToCreateANewConditionalGeofencePolicyWithTheOutsidePolicySetToTheNewPolicy()
        {
            try
            {
                policy = new ConditionalGeoFencePolicy(
                    inside: new MethodAmountPolicy(null),
                    outside: policy,
                    fences: null,
                    denyRootedJailbroken: false,
                    denyEmulatorSimulator: false
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I set deny_emulator_simulator on the Method Amount Policy to True")]
        public void WhenISetDeny_Emulator_SimulatorOnTheMethodPolicyToTrue()
        {
            policy = new MethodAmountPolicy(
                fences: policy.Fences,
                amount: 0,
                denyEmulatorSimulator: true,
                denyRootedJailbroken: policy.DenyRootedJailbroken
            );
        }

        [When(@"I set deny_rooted_jailbroken on the Method Amount Policy to True")]
        public void WhenISetDeny_Rooted_JailbrokenOnTheMethodPolicyToTrue()
        {
            policy = new MethodAmountPolicy(
                fences: policy.Fences,
                amount: 0,
                denyEmulatorSimulator: policy.DenyEmulatorSimulator,
                denyRootedJailbroken: true
            );
        }

        [Given(@"I have any Conditional Geofence Policy")]
        public void GivenIHaveAnyConditionalGeofencePolicy()
        {
            policy = new ConditionalGeoFencePolicy(
                inside: new MethodAmountPolicy(null),
                outside: new MethodAmountPolicy(null),
                fences: null,
                denyRootedJailbroken: false,
                denyEmulatorSimulator: false
            );
        }

        [When(@"I attempt to set the inside policy to any Conditional Geofence Policy")]
        public void WhenIAttemptToSetTheInsidePolicyToAnyConditionalGeofencePolicy()
        {
            ConditionalGeoFencePolicy interim_policy = new ConditionalGeoFencePolicy(
                inside: new MethodAmountPolicy(null),
                outside: new MethodAmountPolicy(null),
                fences: null,
                denyRootedJailbroken: false,
                denyEmulatorSimulator: false
            );

            try
            {
                policy = new ConditionalGeoFencePolicy(
                    outside: new MethodAmountPolicy(null),
                    inside: interim_policy,
                    fences: null,
                    denyRootedJailbroken: false,
                    denyEmulatorSimulator: false
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }
    }
}
