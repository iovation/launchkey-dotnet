// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace iovation.LaunchKey.Sdk.Tests.Integration.Features.Service_Client.Auth_Request
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class ServiceClientAuthorizationRequestGetDeviceResponsePolicyFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "service-client-auth-request-get-device-response-policy.feature"
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
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Service Client Authorization Request: Get Device Response Policy", "  In order to understand an auth response\n  As a Directory Service\n  I can retrie" +
                    "ve an Authorization Requests that been responded to and determine\n  the policy t" +
                    "hat was used", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Service Client Authorization Request: Get Device Response Policy")))
            {
                global::iovation.LaunchKey.Sdk.Tests.Integration.Features.Service_Client.Auth_Request.ServiceClientAuthorizationRequestGetDeviceResponsePolicyFeature.FeatureSetup(null);
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
    testRunner.Given("I created a Directory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
    testRunner.And("I have added an SDK Key to the Directory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
    testRunner.And("I created a Directory Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
    testRunner.And("I have a linked Device", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Verify that geofences without names received from a device can be parsed")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Service Client Authorization Request: Get Device Response Policy")]
        public virtual void VerifyThatGeofencesWithoutNamesReceivedFromADeviceCanBeParsed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify that geofences without names received from a device can be parsed", null, ((string[])(null)));
#line 13
   this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
   this.FeatureBackground();
#line 14
    testRunner.Given("the current Authorization Policy requires a geofence with a radius of 150.0, a la" +
                    "titude of 23.4, and a longitude of -56.7", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
    testRunner.And("the current Authorization Policy requires a geofence with a radius of 100.0, a la" +
                    "titude of -23.4, and a longitude of 56.7", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
    testRunner.When("I make a Policy based Authorization request for the User", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
    testRunner.And("I deny the auth request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
    testRunner.And("I get the response for the Authorization request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
    testRunner.Then("the Authorization response should contain a geofence with a radius of 150.0, a la" +
                    "titude of 23.4, and a longitude of -56.7", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 20
    testRunner.And("the Authorization response should contain a geofence with a radius of 100.0, a la" +
                    "titude of -23.4, and a longitude of 56.7", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Verify that geofences containing names received from a device can be parsed")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Service Client Authorization Request: Get Device Response Policy")]
        public virtual void VerifyThatGeofencesContainingNamesReceivedFromADeviceCanBeParsed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify that geofences containing names received from a device can be parsed", null, ((string[])(null)));
#line 22
   this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
   this.FeatureBackground();
#line 23
    testRunner.Given("the current Authorization Policy requires a geofence with a radius of 150.0, a la" +
                    "titude of 23.4, a longitude of -56.7, and a name of \"geo 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 24
    testRunner.And("the current Authorization Policy requires a geofence with a radius of 100.0, a la" +
                    "titude of -23.4, a longitude of 56.7, and a name of \"geo 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
    testRunner.When("I make a Policy based Authorization request for the User", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
    testRunner.And("I deny the auth request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
    testRunner.And("I get the response for the Authorization request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
    testRunner.Then("the Authorization response should contain a geofence with a radius of 150.0, a la" +
                    "titude of 23.4, a longitude of -56.7, and a name of \"geo 1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 29
    testRunner.And("the Authorization response should contain a geofence with a radius of 100.0, a la" +
                    "titude of -23.4, a longitude of 56.7, and a name of \"geo 2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Verify that required factor counts received from a device can be parsed")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Service Client Authorization Request: Get Device Response Policy")]
        public virtual void VerifyThatRequiredFactorCountsReceivedFromADeviceCanBeParsed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify that required factor counts received from a device can be parsed", null, ((string[])(null)));
#line 31
   this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
   this.FeatureBackground();
#line 32
    testRunner.Given("the current Authorization Policy requires 3 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 33
    testRunner.When("I make a Policy based Authorization request for the User", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 34
    testRunner.And("I receive the auth request and acknowledge the failure message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
    testRunner.And("I get the response for the Authorization request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
    testRunner.Then("the Authorization response should require 3 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Verify that required factor types received from a device can be parsed")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Service Client Authorization Request: Get Device Response Policy")]
        public virtual void VerifyThatRequiredFactorTypesReceivedFromADeviceCanBeParsed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify that required factor types received from a device can be parsed", null, ((string[])(null)));
#line 38
   this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 7
   this.FeatureBackground();
#line 39
    testRunner.Given("the current Authorization Policy requires inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 40
    testRunner.And("the current Authorization Policy requires possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
    testRunner.When("I make a Policy based Authorization request for the User", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 42
    testRunner.And("I receive the auth request and acknowledge the failure message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
    testRunner.And("I get the response for the Authorization request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
    testRunner.Then("the Authorization response should require inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 45
    testRunner.And("the Authorization response should require possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
    testRunner.And("the Authorization response should not require knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
