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
namespace iovation.LaunchKey.Sdk.Tests.Integration.Features.Org_Client.Services.Policy
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class OrganizationClientCanRetrieveOrganizationServicePolicyFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "org-client-service-policy-get.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Organization Client can retrieve Organization Service Policy", "  In order to manage the Authorization Policy of an Organization Service\r\n  As an" +
                    " Organization Client\r\n  I can retrieve the Authorization Policy of an Organizati" +
                    "on Service", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Organization Client can retrieve Organization Service Policy")))
            {
                global::iovation.LaunchKey.Sdk.Tests.Integration.Features.Org_Client.Services.Policy.OrganizationClientCanRetrieveOrganizationServicePolicyFeature.FeatureSetup(null);
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
#line 7
  #line 8
    testRunner.Given("I created an Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting the policy when none is set returns a blank Policy")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can retrieve Organization Service Policy")]
        public virtual void GettingThePolicyWhenNoneIsSetReturnsABlankPolicy()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting the policy when none is set returns a blank Policy", null, ((string[])(null)));
#line 10
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
  this.FeatureBackground();
#line 11
    testRunner.When("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 12
    testRunner.Then("the Organization Service Policy has no requirement for inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
    testRunner.Then("the Organization Service Policy has no requirement for knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 14
    testRunner.Then("the Organization Service Policy has no requirement for possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 15
    testRunner.Then("the Organization Service Policy has no requirement for number of factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting the required factors works as expected")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can retrieve Organization Service Policy")]
        public virtual void GettingTheRequiredFactorsWorksAsExpected()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting the required factors works as expected", null, ((string[])(null)));
#line 17
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
  this.FeatureBackground();
#line 18
    testRunner.Given("the Organization Service Policy is set to require 2 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
    testRunner.When("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
    testRunner.Then("the Organization Service Policy requires 2 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting the factor requirements works as expected")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can retrieve Organization Service Policy")]
        public virtual void GettingTheFactorRequirementsWorksAsExpected()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting the factor requirements works as expected", null, ((string[])(null)));
#line 23
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
  this.FeatureBackground();
#line 24
    testRunner.Given("the Organization Service Policy is set to require inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 25
    testRunner.Given("the Organization Service Policy is set to require knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
    testRunner.Given("the Organization Service Policy is set to require possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 27
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
    testRunner.When("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
    testRunner.Then("the Organization Service Policy does require inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
    testRunner.Then("the Organization Service Policy does require knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 31
    testRunner.Then("the Organization Service Policy does require possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting jail break protection works as expected")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can retrieve Organization Service Policy")]
        public virtual void GettingJailBreakProtectionWorksAsExpected()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting jail break protection works as expected", null, ((string[])(null)));
#line 33
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
  this.FeatureBackground();
#line 34
    testRunner.Given("the Organization Service Policy is set to require jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 35
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
    testRunner.When("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 37
    testRunner.Then("the Organization Service Policy does require jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Time Fences work as expected")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can retrieve Organization Service Policy")]
        public virtual void TimeFencesWorkAsExpected()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Time Fences work as expected", null, ((string[])(null)));
#line 39
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
  this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Days",
                        "Start Hour",
                        "Start Minute",
                        "End Hour",
                        "End Minute",
                        "Time Zone"});
            table1.AddRow(new string[] {
                        "Week Days",
                        "Monday,Tuesday,Wednesday,Thursday,Friday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/Los_Angeles"});
            table1.AddRow(new string[] {
                        "Week Ends",
                        "Saturday,Sunday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/New_York"});
#line 40
    testRunner.Given("the Organization Service Policy is set to have the following Time Fences:", ((string)(null)), table1, "Given ");
#line 44
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
    testRunner.When("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Days",
                        "Start Hour",
                        "Start Minute",
                        "End Hour",
                        "End Minute",
                        "Time Zone"});
            table2.AddRow(new string[] {
                        "Week Days",
                        "Monday,Tuesday,Wednesday,Thursday,Friday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/Los_Angeles"});
            table2.AddRow(new string[] {
                        "Week Ends",
                        "Saturday,Sunday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/New_York"});
#line 46
    testRunner.Then("the Organization Service Policy has the following Time Fences:", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Geofence locations work as expected")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can retrieve Organization Service Policy")]
        public virtual void GeofenceLocationsWorkAsExpected()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Geofence locations work as expected", null, ((string[])(null)));
#line 51
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
  this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Latitude",
                        "Longitude",
                        "Radius"});
            table3.AddRow(new string[] {
                        "Location Alpha",
                        "12.3",
                        "23.4",
                        "500"});
            table3.AddRow(new string[] {
                        "Location Beta",
                        "32.1",
                        "43.2",
                        "1000"});
#line 52
    testRunner.Given("the Organization Service Policy is set to have the following Geofence locations:", ((string)(null)), table3, "Given ");
#line 56
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
    testRunner.When("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Latitude",
                        "Longitude",
                        "Radius"});
            table4.AddRow(new string[] {
                        "Location Alpha",
                        "12.3",
                        "23.4",
                        "500"});
            table4.AddRow(new string[] {
                        "Location Beta",
                        "32.1",
                        "43.2",
                        "1000"});
#line 58
    testRunner.Then("the Organization Service Policy has the following Geofence locations:", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting the policy for invalid Service throws Forbidden")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can retrieve Organization Service Policy")]
        public virtual void GettingThePolicyForInvalidServiceThrowsForbidden()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting the policy for invalid Service throws Forbidden", null, ((string[])(null)));
#line 63
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
  this.FeatureBackground();
#line 64
    testRunner.When("I attempt to retrieve the Policy for the Organization Service with the ID \"eba60c" +
                    "b8-c649-11e7-abc4-cec278b6b50a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 65
    testRunner.Then("a ServiceNotFound error occurs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
