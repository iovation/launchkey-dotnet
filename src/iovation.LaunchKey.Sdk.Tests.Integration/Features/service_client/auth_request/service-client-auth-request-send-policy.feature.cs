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
    public partial class ServiceClientAuthorizationRequestCanSendPolicyFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "service-client-auth-request-send-policy.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Service Client Authorization Request: Can Send Policy", "  In order to begin an authorization request\n  As a Directory Service\n  I can cre" +
                    "ate an Authorization Request for a User", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Service Client Authorization Request: Can Send Policy")))
            {
                global::iovation.LaunchKey.Sdk.Tests.Integration.Features.Service_Client.Auth_Request.ServiceClientAuthorizationRequestCanSendPolicyFeature.FeatureSetup(null);
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
#line 9
  #line 10
    testRunner.Given("I created a Directory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 11
    testRunner.And("I created a Directory Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Making a request with a quantity of required factors for an invalid User Throws E" +
            "ntityNotFound")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Service Client Authorization Request: Can Send Policy")]
        public virtual void MakingARequestWithAQuantityOfRequiredFactorsForAnInvalidUserThrowsEntityNotFound()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Making a request with a quantity of required factors for an invalid User Throws E" +
                    "ntityNotFound", null, ((string[])(null)));
#line 13
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 9
  this.FeatureBackground();
#line 14
    testRunner.Given("the current Authorization Policy requires 3 factors", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
    testRunner.When("I attempt to make an Policy based Authorization request for the User identified b" +
                    "y \"User does not matter\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 16
    testRunner.Then("a EntityNotFound error occurs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Making a request with required factor types for an invalid User Throws EntityNotF" +
            "ound")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Service Client Authorization Request: Can Send Policy")]
        public virtual void MakingARequestWithRequiredFactorTypesForAnInvalidUserThrowsEntityNotFound()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Making a request with required factor types for an invalid User Throws EntityNotF" +
                    "ound", null, ((string[])(null)));
#line 18
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 9
  this.FeatureBackground();
#line 19
    testRunner.Given("the current Authorization Policy requires inherence", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 20
    testRunner.And("the current Authorization Policy requires knowledge", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
    testRunner.And("the current Authorization Policy requires possession", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
    testRunner.When("I attempt to make an Policy based Authorization request for the User identified b" +
                    "y \"User does not matter\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
    testRunner.Then("a EntityNotFound error occurs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Making a request with a couple geofences for an invalid User Throws EntityNotFoun" +
            "d")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Service Client Authorization Request: Can Send Policy")]
        public virtual void MakingARequestWithACoupleGeofencesForAnInvalidUserThrowsEntityNotFound()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Making a request with a couple geofences for an invalid User Throws EntityNotFoun" +
                    "d", null, ((string[])(null)));
#line 25
  this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 9
  this.FeatureBackground();
#line 26
    testRunner.Given("the current Authorization Policy requires a geofence with a radius of 150.0, a la" +
                    "titude of 23.4, and a longitude of -56.7", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 27
    testRunner.And("the current Authorization Policy requires a geofence with a radius of 100.0, a la" +
                    "titude of -23.4, and a longitude of 56.7", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
    testRunner.When("I attempt to make an Policy based Authorization request for the User identified b" +
                    "y \"User does not matter\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
    testRunner.Then("a EntityNotFound error occurs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
