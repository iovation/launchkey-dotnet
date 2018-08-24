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
namespace iovation.LaunchKey.Sdk.Tests.Integration.Features.Org_Client.Directories.Public_Keys
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class OrganizationClientsCanRemoveDirectoryPublicKeysFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "org-client-directory-public-keys-remove.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Organization clients can remove Directory Public Keys", "  In order to manage the Public Keys in the Directory Public Key rotation\r\n  As a" +
                    "n Organization client\r\n  I can remove a Public Key from an Directory", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Organization clients can remove Directory Public Keys")))
            {
                global::iovation.LaunchKey.Sdk.Tests.Integration.Features.Org_Client.Directories.Public_Keys.OrganizationClientsCanRemoveDirectoryPublicKeysFeature.FeatureSetup(null);
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
    testRunner.And("I added a Public Key to the Directory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Removing Public Key actually removes from list")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization clients can remove Directory Public Keys")]
        public virtual void RemovingPublicKeyActuallyRemovesFromList()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Removing Public Key actually removes from list", null, ((string[])(null)));
#line 10
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 11
    testRunner.Given("I added another Public Key to the Directory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 12
    testRunner.When("I remove the current Directory Public Key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
    testRunner.And("I retrieve the current Directory\'s Public Keys", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
    testRunner.Then("the last current Directory\'s Public Key is not in the list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Removing the last Public Key throws LastRemainingKey exception")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization clients can remove Directory Public Keys")]
        public virtual void RemovingTheLastPublicKeyThrowsLastRemainingKeyException()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Removing the last Public Key throws LastRemainingKey exception", null, ((string[])(null)));
#line 16
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 17
    testRunner.When("I attempt to remove the current Directory Public Key", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
    testRunner.Then("a com.iovation.launchkey.sdk.error.LastRemainingKey exception is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Attempting to update a Public Key for an invalid Directory throws a Forbidden exc" +
            "eption")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization clients can remove Directory Public Keys")]
        public virtual void AttemptingToUpdateAPublicKeyForAnInvalidDirectoryThrowsAForbiddenException()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Attempting to update a Public Key for an invalid Directory throws a Forbidden exc" +
                    "eption", null, ((string[])(null)));
#line 20
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 21
    testRunner.When("I attempt to remove a Public Key from the Directory with the ID \"eba60cb8-c649-11" +
                    "e7-abc4-cec278b6b50a\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 22
    testRunner.Then("a com.iovation.launchkey.sdk.error.Forbidden exception is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Attempting to update an invalid Public Key for a Directory throws a Forbidden exc" +
            "eption")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Organization clients can remove Directory Public Keys")]
        public virtual void AttemptingToUpdateAnInvalidPublicKeyForADirectoryThrowsAForbiddenException()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Attempting to update an invalid Public Key for a Directory throws a Forbidden exc" +
                    "eption", null, ((string[])(null)));
#line 24
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
  this.FeatureBackground();
#line 25
    testRunner.When("I attempt to remove a Public Key identified by \"aa:bb:cc:dd:ee:ff:11:22:33:44:55:" +
                    "66:77:88:99:00\" from the Directory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
    testRunner.Then("a com.iovation.launchkey.sdk.error.PublicKeyDoesNotExist exception is thrown", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
