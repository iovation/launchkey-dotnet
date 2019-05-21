using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Appium.MultiTouch;
using TechTalk.SpecFlow.UnitTestProvider;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    [Binding]
    public class AppiumContext
    {
        private AndroidDriver<AndroidElement> driver;
        private ScenarioContext _scenarioContext;
        private TestConfiguration _testConfiguration;

        [BeforeScenario("device_testing")]
        public void BeforeAll()
        {
            var appConfig = _testConfiguration.appiumConfigs;

            //If device configurations are not set up ignore the test
            if(appConfig == null)
            {
                var unitTestRuntimeProvider = (IUnitTestRuntimeProvider)
                    _scenarioContext.GetBindingInstance((typeof(IUnitTestRuntimeProvider)));
                unitTestRuntimeProvider.TestIgnore("ignored");
            }

            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(MobileCapabilityType.BrowserName, "");
            capabilities.SetCapability(MobileCapabilityType.PlatformName, appConfig.PlatformName);
            capabilities.SetCapability(MobileCapabilityType.PlatformVersion, appConfig.PlatformVersion);
            capabilities.SetCapability(MobileCapabilityType.AutomationName, "UIAutomator2");
            capabilities.SetCapability(MobileCapabilityType.DeviceName, appConfig.DeviceName);
            capabilities.SetCapability(MobileCapabilityType.App, appConfig.AppFilePath);
            capabilities.SetCapability("appPackage", "com.launchkey.android.authenticator.demo");
            driver = new AndroidDriver<AndroidElement>(new Uri(appConfig.AppiumURL), capabilities, TimeSpan.FromSeconds(180));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [AfterScenario("device_testing")]
        public void CleanupAppiumSession()
        {
            if(driver != null)
            {
                driver.Quit();
            }
        }

        public AppiumContext(TestConfiguration testConfiguration, ScenarioContext scenarioContext)
        {
            _testConfiguration = testConfiguration;
            this._scenarioContext = scenarioContext;
        }


        public void LinkDevice(string sdkKey, string linkingCode, string deviceName)
        {
            ApproveAlert();
            OpenLinkingMenu();
            FillLinkingMenu(linkingCode);
            FillAuthenticatorSdkKey(sdkKey);
            if (deviceName != "")
            {
                FillDeviceName(deviceName);
            }

            SubmitLinkingForm();

        }

        public void UnlinkDevice()
        {
            ApproveAlert();
            FindByScrollableText("Unlink 2 (Custom UI)").Click();
        }

        public void ApproveRequest()
        {
            OpenAuthMenu();
            TapRefresh();
            System.Threading.Thread.Sleep(1000);
            ApproveAuth();
        }

        public void DenyRequest()
        {
            OpenAuthMenu();
            TapRefresh();
            DenyAuth();
        }

        public void ReceiveAndAcknowledgeAuthFailure()
        {
            OpenAuthMenu();
            TapRefresh();
            DismissFailureMessage();
        }

        private void DismissFailureMessage()
        {
            FindByText("DISMISS").Click();
        }

        private void DenyAuth()
        {
            AndroidElement denyButton = FindByID("com.launchkey.android.authenticator.demo:id/auth_info_action_negative");
            LongPress(denyButton);

            FindByText("I don't approve").Click();

            AndroidElement submitDenyButton = FindByID("com.launchkey.android.authenticator.demo:id/auth_do_action_negative");
            LongPress(submitDenyButton);
        }

        private void ApproveAuth()
        {
            AndroidElement authorizeButton = FindByID("com.launchkey.android.authenticator.demo:id/auth_info_action_positive");
            LongPress(authorizeButton);
        }

        private void TapRefresh()
        {
            FindByID("menu_refresh").Click();
        }

        private void OpenAuthMenu()
        {
            FindByScrollableText("Check for Requests (XML)").Click();
        }

        private void SubmitLinkingForm()
        {
            AndroidElement linkButton = FindByID("com.launchkey.android.authenticator.demo:id/demo_link_button");
            linkButton.Click();
        }

        private void FillDeviceName(string deviceName)
        {
            FindByText("Use custom device name").Click();
            FindByID("demo_link_edit_name").SendKeys(deviceName);

        }

        private void FillAuthenticatorSdkKey(string sdkKey)
        {
            FindByID("com.launchkey.android.authenticator.demo:id/demo_link_edit_key").SendKeys(sdkKey);
        }

        private void FillLinkingMenu(string linkingCode)
        {
            FindByText("Linking code").SendKeys(linkingCode);
        }

        private void OpenLinkingMenu()
        {
            FindByText("Link (Custom UI - Manual)").Click();
        }

        private void ApproveAlert()
        {
            driver.FindElementById("android:id/button1").Click();
        }

        private void LongPress(AndroidElement element)
        {
            var topCorner = element.Location;
            var centerX = topCorner.X + element.Size.Width / 2;
            var centerY = topCorner.Y + element.Size.Height / 2;

            var touch = new TouchAction(driver);
            touch.Press(centerX, centerY).Wait(2000).MoveTo(centerX, centerY).Release();
            touch.Perform();
        }

        private AndroidElement FindByResourceID(string resourceID)
        {
            return driver.FindElementByAccessibilityId(resourceID);
        }

        private AndroidElement FindByText(string searchText)
        {
            return driver.FindElementByAndroidUIAutomator($"new UiSelector().text(\"{searchText}\")");
        }

        private AndroidElement FindByID(string ID)
        {
            return driver.FindElementById(ID);
        }

        private AndroidElement FindByScrollableText(string searchText)
        {
            return driver.FindElementByAndroidUIAutomator($"new UiScrollable(new UiSelector().scrollable(true).instance(0)).scrollIntoView(new UiSelector().textContains(\"{searchText}\").instance(0))");
        }
    }
}
