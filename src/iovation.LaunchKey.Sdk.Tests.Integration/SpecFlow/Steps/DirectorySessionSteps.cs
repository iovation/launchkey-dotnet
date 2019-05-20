using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public class DirectorySessionSteps
    {
        private readonly CommonContext _commonContext;
        private readonly DirectoryClientContext _directoryClientContext;

        public DirectorySessionSteps(CommonContext commonContext, DirectoryClientContext directoryClientContext)
        {
            _commonContext = commonContext;
            _directoryClientContext = directoryClientContext;
        }

        [When(@"I delete the Sessions for the current User")]
        public void WhenIDeleteTheSessionsForTheCurrentUser()
        {
            _directoryClientContext.EndAllServiceSessionsForCurrentUser();
        }

        [When(@"I retrieve the Session list for the current User")]
        public void WhenIRetrieveTheSessionListForTheCurrentUser()
        {
            _directoryClientContext.LoadSessionsForCurrentUser();
        }

        [Then(@"the Service User Session List has (.*) Sessions")]
        public void ThenTheServiceUserSessionListHasSessions(int p0)
        {
            Assert.AreEqual(p0, _directoryClientContext.LoadedSessions.Count);
        }

        [When(@"I attempt to delete the Sessions for the User ""(.*)""")]
        public void WhenIAttemptToDeleteTheSessionsForTheUser(string p0)
        {
            try
            {
                _directoryClientContext.EndAllServiceSessions(p0);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to retrieve the Session list for the User ""(.*)""")]
        public void WhenIAttemptToRetrieveTheSessionListForTheUser(string p0)
        {
            try
            {
                _directoryClientContext.LoadSessions(p0);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Then(@"all of the devices should be active")]
        public void ThenAllOfTheDevicesShouldBeActive()
        {
            //Sleep to factor in network latency
            System.Threading.Thread.Sleep(1000);
            _directoryClientContext.LoadDevicesForCurrentUser();
            var loadedDevices = _directoryClientContext.LoadedDevices;

            foreach (var device in loadedDevices)
            {
                var deviceStatus = device.Status;

                if (device.Status.StatusCode != 1)
                {
                    throw new System.Exception($"All Devices should be linked. Device status was {device.Status.Text}");
                }
            }
        }

        [Then(@"all of the devices should be inactive")]
        public void ThenAllOfTheDevicesShouldBeInactive()
        {
            _directoryClientContext.LoadDevicesForCurrentUser();
            var loadedDevices = _directoryClientContext.LoadedDevices;

            foreach (var device in loadedDevices)
            {
                var deviceStatus = device.Status;

                if (device.Status.StatusCode == 1)
                {
                    throw new System.Exception($"All Devices should be unlinked. Device status was {device.Status.Text}");
                }
            }
        }
    }
}
