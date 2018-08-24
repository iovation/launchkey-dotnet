﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace iovation.LaunchKey.Sdk.Tests.Integration.Features.Directory_Client.Services.Policy
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class DirectoryClientCanRemoveDirectoryServicePolicyFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "directory-client-service-policy-remove.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Directory Client can remove Directory Service Policy", "  In order to manage the Authorization Policy of a Directory Service\r\n  As a Dire" +
                    "ctory Client\r\n  I can remove the Authorization Policy of a Directory Service", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Directory Client can remove Directory Service Policy")))
            {
                global::iovation.LaunchKey.Sdk.Tests.Integration.Features.Directory_Client.Services.Policy.DirectoryClientCanRemoveDirectoryServicePolicyFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
  #line 7
    testRunner.Given("I created a Directory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
    testRunner.And("I created a Directory Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Removing Policy when there is no Policy results in blank Policy")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Directory Client can remove Directory Service Policy")]
        public virtual void RemovingPolicyWhenThereIsNoPolicyResultsInBlankPolicy()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Removing Policy when there is no Policy results in blank Policy", null, ((string[])(null)));
#line 10
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 11
    testRunner.When("I remove the Policy for the Directory Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 12
    testRunner.And("I retrieve the Policy for the Current Directory Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
    testRunner.Then("the Directory Service Policy has no requirement for number of factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Removing existing Policy results in blank policy")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Directory Client can remove Directory Service Policy")]
        public virtual void RemovingExistingPolicyResultsInBlankPolicy()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Removing existing Policy results in blank policy", null, ((string[])(null)));
#line 15
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 16
    testRunner.Given("the Directory Service Policy is set to require 1 factor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 17
    testRunner.And("I set the Policy for the Directory Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
    testRunner.When("I remove the Policy for the Directory Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
    testRunner.And("I retrieve the Policy for the Current Directory Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
    testRunner.Then("the Directory Service Policy has no requirement for number of factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Removing the policy for invalid Service throws Forbidden")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Directory Client can remove Directory Service Policy")]
        public virtual void RemovingThePolicyForInvalidServiceThrowsForbidden()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Removing the policy for invalid Service throws Forbidden", null, ((string[])(null)));
#line 22
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 23
    testRunner.When("I attempt to remove the Policy for the Directory Service with the ID \"eba60cb8-c6" +
                    "49-11e7-abc4-cec278b6b50a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
    testRunner.Then("a com.iovation.launchkey.sdk.error.ServiceNotFound exception is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
