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
    public partial class OrganizationClientCanSetOrganizationServicePolicyFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "org-client-service-policy-set.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Organization Client can set Organization Service Policy", "  In order to manage the Authorization Policy of an Organization Service\r\n  As an" +
                    " Organization Client\r\n  I can set the Authorization Policy of an Organization Se" +
                    "rvice", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Organization Client can set Organization Service Policy")))
            {
                global::iovation.LaunchKey.Sdk.Tests.Integration.Features.Org_Client.Services.Policy.OrganizationClientCanSetOrganizationServicePolicyFeature.FeatureSetup(null);
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
    testRunner.Given("I created an Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Setting the policy for invalid Service throws Forbidden")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can set Organization Service Policy")]
        public virtual void SettingThePolicyForInvalidServiceThrowsForbidden()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setting the policy for invalid Service throws Forbidden", null, ((string[])(null)));
#line 9
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 10
    testRunner.When("the Organization Service Policy is set to require 2 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 11
    testRunner.And("I attempt to set the Policy for the Organization Service with the ID \"eba60cb8-c6" +
                    "49-11e7-abc4-cec278b6b50a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
    testRunner.Then("a com.iovation.launchkey.sdk.error.ServiceNotFound exception is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Setting the required factors will set only the factors and all else will be empty" +
            " or null")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can set Organization Service Policy")]
        public virtual void SettingTheRequiredFactorsWillSetOnlyTheFactorsAndAllElseWillBeEmptyOrNull()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setting the required factors will set only the factors and all else will be empty" +
                    " or null", null, ((string[])(null)));
#line 14
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 15
    testRunner.When("the Organization Service Policy is set to require 2 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 16
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
    testRunner.And("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
    testRunner.Then("the Organization Service Policy requires 2 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
    testRunner.And("the Organization Service Policy has no requirement for inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
    testRunner.And("the Organization Service Policy has no requirement for knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
    testRunner.And("the Organization Service Policy has no requirement for possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
    testRunner.And("the Organization Service Policy has 0 locations", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
    testRunner.And("the Organization Service Policy has 0 time fences", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
    testRunner.And("the Organization Service Policy has no requirement for jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Setting the individual factors will set only the factors and all else will be emp" +
            "ty or null")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can set Organization Service Policy")]
        public virtual void SettingTheIndividualFactorsWillSetOnlyTheFactorsAndAllElseWillBeEmptyOrNull()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setting the individual factors will set only the factors and all else will be emp" +
                    "ty or null", null, ((string[])(null)));
#line 26
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 27
    testRunner.When("the Organization Service Policy is set to require inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
    testRunner.And("the Organization Service Policy is set to require knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
    testRunner.And("the Organization Service Policy is set to require possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
    testRunner.And("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
    testRunner.And("the Organization Service Policy has no requirement for number of factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
    testRunner.And("the Organization Service Policy does require inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
    testRunner.And("the Organization Service Policy does require knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
    testRunner.And("the Organization Service Policy does require possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
    testRunner.And("the Organization Service Policy has 0 locations", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
    testRunner.And("the Organization Service Policy has 0 time fences", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
    testRunner.And("the Organization Service Policy has no requirement for jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Setting jail break protection will only set jail break protection and everything " +
            "else will be empty of null")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can set Organization Service Policy")]
        public virtual void SettingJailBreakProtectionWillOnlySetJailBreakProtectionAndEverythingElseWillBeEmptyOfNull()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setting jail break protection will only set jail break protection and everything " +
                    "else will be empty of null", null, ((string[])(null)));
#line 40
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 41
    testRunner.And("the Organization Service Policy is set to require jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
    testRunner.And("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
    testRunner.Then("the Organization Service Policy does require jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 45
    testRunner.And("the Organization Service Policy has no requirement for number of factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
    testRunner.And("the Organization Service Policy has no requirement for inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
    testRunner.And("the Organization Service Policy has no requirement for knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
    testRunner.And("the Organization Service Policy has no requirement for possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
    testRunner.And("the Organization Service Policy has 0 locations", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
    testRunner.And("the Organization Service Policy has 0 time fences", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("When setting Time Fences, they are set and nothing else as expected")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can set Organization Service Policy")]
        public virtual void WhenSettingTimeFencesTheyAreSetAndNothingElseAsExpected()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("When setting Time Fences, they are set and nothing else as expected", null, ((string[])(null)));
#line 53
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
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
#line 54
    testRunner.When("the Organization Service Policy is set to have the following Time Fences:", ((string)(null)), table1, "When ");
#line 58
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
    testRunner.And("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
#line 60
    testRunner.Then("the Organization Service Policy has the following Time Fences:", ((string)(null)), table2, "Then ");
#line 64
    testRunner.And("the Organization Service Policy has no requirement for number of factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
    testRunner.And("the Organization Service Policy has no requirement for inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
    testRunner.And("the Organization Service Policy has no requirement for knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
    testRunner.And("the Organization Service Policy has no requirement for possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
    testRunner.And("the Organization Service Policy has 0 locations", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Geofence locations work as expected")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can set Organization Service Policy")]
        public virtual void GeofenceLocationsWorkAsExpected()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Geofence locations work as expected", null, ((string[])(null)));
#line 71
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
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
#line 72
    testRunner.When("the Organization Service Policy is set to have the following Geofence locations:", ((string)(null)), table3, "When ");
#line 76
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
    testRunner.And("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
#line 78
    testRunner.Then("the Organization Service Policy has the following Geofence locations:", ((string)(null)), table4, "Then ");
#line 82
    testRunner.And("the Organization Service Policy has no requirement for number of factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
    testRunner.And("the Organization Service Policy has no requirement for inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
    testRunner.And("the Organization Service Policy has no requirement for knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
    testRunner.And("the Organization Service Policy has no requirement for possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
    testRunner.And("the Organization Service Policy has 0 time fences", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Setting required factors, locations, and fences properly set the values.")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can set Organization Service Policy")]
        public virtual void SettingRequiredFactorsLocationsAndFencesProperlySetTheValues_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setting required factors, locations, and fences properly set the values.", null, ((string[])(null)));
#line 88
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 89
    testRunner.When("the Organization Service Policy is set to require 2 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 90
    testRunner.And("the Organization Service Policy is set to require jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Days",
                        "Start Hour",
                        "Start Minute",
                        "End Hour",
                        "End Minute",
                        "Time Zone"});
            table5.AddRow(new string[] {
                        "Week Days",
                        "Monday,Tuesday,Wednesday,Thursday,Friday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/Los_Angeles"});
            table5.AddRow(new string[] {
                        "Week Ends",
                        "Saturday,Sunday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/New_York"});
#line 91
    testRunner.And("the Organization Service Policy is set to have the following Time Fences:", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Latitude",
                        "Longitude",
                        "Radius"});
            table6.AddRow(new string[] {
                        "Location Alpha",
                        "12.3",
                        "23.4",
                        "500"});
            table6.AddRow(new string[] {
                        "Location Beta",
                        "32.1",
                        "43.2",
                        "1000"});
#line 95
    testRunner.And("the Organization Service Policy is set to have the following Geofence locations:", ((string)(null)), table6, "And ");
#line 99
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
    testRunner.And("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
    testRunner.Then("the Organization Service Policy requires 2 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 102
    testRunner.And("the Organization Service Policy does require jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
    testRunner.And("the Organization Service Policy has no requirement for inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 104
    testRunner.And("the Organization Service Policy has no requirement for knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
    testRunner.And("the Organization Service Policy has no requirement for possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Days",
                        "Start Hour",
                        "Start Minute",
                        "End Hour",
                        "End Minute",
                        "Time Zone"});
            table7.AddRow(new string[] {
                        "Week Days",
                        "Monday,Tuesday,Wednesday,Thursday,Friday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/Los_Angeles"});
            table7.AddRow(new string[] {
                        "Week Ends",
                        "Saturday,Sunday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/New_York"});
#line 106
    testRunner.And("the Organization Service Policy has the following Time Fences:", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Latitude",
                        "Longitude",
                        "Radius"});
            table8.AddRow(new string[] {
                        "Location Alpha",
                        "12.3",
                        "23.4",
                        "500"});
            table8.AddRow(new string[] {
                        "Location Beta",
                        "32.1",
                        "43.2",
                        "1000"});
#line 110
    testRunner.And("the Organization Service Policy has the following Geofence locations:", ((string)(null)), table8, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Setting individual factors, locations, and fences properly set the values.")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization Client can set Organization Service Policy")]
        public virtual void SettingIndividualFactorsLocationsAndFencesProperlySetTheValues_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setting individual factors, locations, and fences properly set the values.", null, ((string[])(null)));
#line 115
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 116
    testRunner.When("the Organization Service Policy is set to require inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 117
    testRunner.And("the Organization Service Policy is set to require knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
    testRunner.And("the Organization Service Policy is set to require possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
    testRunner.And("the Organization Service Policy is set to require jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Latitude",
                        "Longitude",
                        "Radius"});
            table9.AddRow(new string[] {
                        "Location Alpha",
                        "12.3",
                        "23.4",
                        "500"});
            table9.AddRow(new string[] {
                        "Location Beta",
                        "32.1",
                        "43.2",
                        "1000"});
#line 120
    testRunner.And("the Organization Service Policy is set to have the following Geofence locations:", ((string)(null)), table9, "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Days",
                        "Start Hour",
                        "Start Minute",
                        "End Hour",
                        "End Minute",
                        "Time Zone"});
            table10.AddRow(new string[] {
                        "Week Days",
                        "Monday,Tuesday,Wednesday,Thursday,Friday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/Los_Angeles"});
            table10.AddRow(new string[] {
                        "Week Ends",
                        "Saturday,Sunday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/New_York"});
#line 124
    testRunner.And("the Organization Service Policy is set to have the following Time Fences:", ((string)(null)), table10, "And ");
#line 128
    testRunner.And("I set the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
    testRunner.And("I retrieve the Policy for the Current Organization Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
    testRunner.Then("the Organization Service Policy has no requirement for number of factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 131
    testRunner.And("the Organization Service Policy does require jail break protection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 132
    testRunner.And("the Organization Service Policy does require inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
    testRunner.And("the Organization Service Policy does require knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
    testRunner.And("the Organization Service Policy does require possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Days",
                        "Start Hour",
                        "Start Minute",
                        "End Hour",
                        "End Minute",
                        "Time Zone"});
            table11.AddRow(new string[] {
                        "Week Days",
                        "Monday,Tuesday,Wednesday,Thursday,Friday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/Los_Angeles"});
            table11.AddRow(new string[] {
                        "Week Ends",
                        "Saturday,Sunday",
                        "0",
                        "0",
                        "23",
                        "59",
                        "America/New_York"});
#line 135
    testRunner.And("the Organization Service Policy has the following Time Fences:", ((string)(null)), table11, "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Latitude",
                        "Longitude",
                        "Radius"});
            table12.AddRow(new string[] {
                        "Location Alpha",
                        "12.3",
                        "23.4",
                        "500"});
            table12.AddRow(new string[] {
                        "Location Beta",
                        "32.1",
                        "43.2",
                        "1000"});
#line 139
    testRunner.And("the Organization Service Policy has the following Geofence locations:", ((string)(null)), table12, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
