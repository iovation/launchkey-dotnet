using System;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public class DirectoryDeviceSteps
    {
        private readonly CommonContext _commonContext;
        private readonly DirectoryClientContext _directoryClientContext;
        private readonly OrgClientContext _orgClientContext;
        private readonly AppiumContext _appiumContext;

        public DirectoryDeviceSteps(CommonContext commonContext, OrgClientContext orgClient, DirectoryClientContext directoryClientContext, AppiumContext appiumContext)
        {
            _commonContext = commonContext;
            _directoryClientContext = directoryClientContext;
            _appiumContext = appiumContext;
            _orgClientContext = orgClient;
        }

        [When(@"I make a Device linking request")]
        [Given(@"I have made a Device linking request")]
        [Given(@"I made a Device linking request")]
        public void WhenIMakeADeviceLinkingRequest()
        {
            _directoryClientContext.LinkDevice(Util.UniqueUserName());
        }

        [When(@"I make a Device linking request with a TTL of (.*) seconds")]
        public void WhenIMakeADeviceLinkingRequestWithTTL(int ttl)
        {
            _directoryClientContext.LinkDevice(Util.UniqueUserName(), ttl);
        }

        [Then(@"the Device linking response contains a valid QR Code URL")]
        public void ThenTheDeviceLinkingResponseContainsAValidQRCodeURL()
        {
            var url = new Uri(_directoryClientContext.LastLinkResponse.QrCode);
            Assert.AreEqual("https", url.Scheme);
            Assert.IsTrue(url.IsAbsoluteUri);
        }

        [Then(@"the Device linking response contains a valid Linking Code")]
        public void ThenTheDeviceLinkingResponseContainsAValidLinkingCode()
        {
            Assert.IsTrue(!string.IsNullOrWhiteSpace(_directoryClientContext.LastLinkResponse.Code));
        }

        [When(@"I retrieve the Devices list for the current User")]
        [Given(@"I retrieve the Devices list for the current User")]
        public void WhenIRetrieveTheDevicesListForTheCurrentUser()
        {
            _directoryClientContext.LoadDevicesForCurrentUser();
        }

        [When(@"I retrieve the Devices list for the user ""(.*)""")]
        public void WhenIRetrieveTheDevicesListForTheUser(string userId)
        {
            _directoryClientContext.LoadDevices(userId);
        }

        [Then(@"there should be (.*) Devices? in the Devices list")]
        [Then(@"the Device List has (.*) Devices?")]
        public void ThenThereShouldBeDeviceInTheDevicesList(int p0)
        {
            Assert.IsTrue(_directoryClientContext.LoadedDevices.Count == p0);
        }

        [When(@"I unlink the current Device")]
        public void WhenIUnlinkTheCurrentDevice()
        {
            _directoryClientContext.UnlinkCurrentDevice();
        }

        [When(@"I attempt to unlink the device with the ID ""(.*)""")]
        public void WhenIAttemptToUnlinkTheDeviceWithTheID(string deviceId)
        {
            try
            {
                _directoryClientContext.UnlinkDevice(_directoryClientContext.CurrentUserId, deviceId);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to unlink the device from the User Identifier ""(.*)""")]
        public void WhenIAttemptToUnlinkTheDeviceFromTheUserIdentifier(string userId)
        {
            try
            {
                _directoryClientContext.UnlinkDevice(userId, Guid.NewGuid().ToString());
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Given(@"I have a linked Device")]
        public void GivenIHaveALinkedDevice()
        {
            //Given I made a Device linking request
            _directoryClientContext.LinkDevice(Util.UniqueUserName());

            //When I link my Device
            WhenILinkMyDevice();
        }

        [When(@"I link my device")]
        public void WhenILinkMyDevice()
        {
            string sdkKey = _orgClientContext.AddedSdkKeys[0].ToString();
            string linkingCode = _directoryClientContext.LastLinkResponse.Code;
            _appiumContext.LinkDevice(sdkKey, linkingCode, "FancyDevice");
        }

        [When(@"I link my physical device with the name ""(.*)""")]
        public void WhenILinkMyDeviceByName(string deviceName)
        {
            string sdkKey = _directoryClientContext.AddedServicePublicKeys[0];
            string linkingCode = _directoryClientContext.LastLinkResponse.Code;
            _appiumContext.LinkDevice(sdkKey, linkingCode, deviceName);
        }

        [When(@"I approve the auth request")]
        public void WhenIApproveTheAuthRequest()
        {
            _appiumContext.ApproveRequest();
        }

        [When(@"I deny the auth request")]
        public void WhenIDenyTheAuthRequest()
        {
            _appiumContext.DenyRequest();
        }

        [When(@"I receive the auth request and acknowledge the failure message")]
        public void WhenIAcknowledgeFailureMessage()
        {
            _appiumContext.ReceiveAndAcknowledgeAuthFailure();
        }

        [Then(@"the Device linking response contains a valid Device ID")]
        public void ThenTheDeviceLinkingResponseContainsValidDeviceID()
        {
            Guid deviceID = _directoryClientContext.LastLinkResponse.DeviceId;
            Assert.AreNotEqual(deviceID, null);
        }
    }
}
