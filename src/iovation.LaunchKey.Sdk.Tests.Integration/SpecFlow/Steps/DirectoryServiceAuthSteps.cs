using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
    [Binding]
    public class DirectoryServiceAuthSteps
    {
        private readonly CommonContext _commonContext;
        private readonly DirectoryClientContext _directoryClientContext;
        private readonly DirectoryServiceClientContext _directoryServiceClientContext;

        private AuthorizationResponse _lastAuthResponse;

        private int? _numFactors;
        private bool? _inherence;
        private bool? _knowledge;
        private bool? _possession;
        private bool? _jailbreak;

        private List<Location> _locations = new List<Location>();

        public DirectoryServiceAuthSteps(
            CommonContext commonContext,
            DirectoryClientContext directoryClientContext,
            DirectoryServiceClientContext directoryServiceClientContext)
        {
            _commonContext = commonContext;
            _directoryClientContext = directoryClientContext;
            _directoryServiceClientContext = directoryServiceClientContext;
        }

        [When(@"I get the response for Authorization request ""(.*)""")]
        public void WhenIGetTheResponseForAuthorizationRequest(string authId)
        {
            _lastAuthResponse = _directoryServiceClientContext.GetAuthResponse(authId);
        }

        [Then(@"the Authorization response is not returned")]
        public void ThenTheAuthorizationResponseIsNotReturned()
        {
            Assert.IsNull(_lastAuthResponse);
        }

        [Given(@"the current Authorization Policy requires (.*) factors")]
        public void GivenTheCurrentAuthorizationPolicyRequiresFactors(int numFactors)
        {
            _numFactors = numFactors;
        }

        [When(@"I attempt to make an Policy based Authorization request for the User identified by ""(.*)""")]
        public void WhenIAttemptToMakeAnPolicyBasedAuthorizationRequestForTheUserIdentifiedBy(string userId)
        {
            try
            {
                _directoryServiceClientContext.Authorize(
                    userId,
                    null,
                    new AuthPolicy(
                        _numFactors,
                        _knowledge,
                        _inherence,
                        _possession,
                        _jailbreak,
                        _locations
                    )
                );
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [Given(@"the current Authorization Policy requires inherence")]
        public void GivenTheCurrentAuthorizationPolicyRequiresInherence()
        {
            _inherence = true;
        }

        [Given(@"the current Authorization Policy requires knowledge")]
        public void GivenTheCurrentAuthorizationPolicyRequiresKnowledge()
        {
            _knowledge = true;
        }

        [Given(@"the current Authorization Policy requires possession")]
        public void GivenTheCurrentAuthorizationPolicyRequiresPossession()
        {
            _possession = true;
        }

        [Given(@"the current Authorization Policy requires a geofence with a radius of (.*), a latitude of (.*), and a longitude of (.*)")]
        public void GivenTheCurrentAuthorizationPolicyRequiresAGeofenceWithARadiusOfALatitudeOfAndALongitudeOf(double radius, double lat, double lon)
        {
            _locations.Add(new Location(radius, lat, lon, null));
        }

        [When(@"I make an Authorization request")]
        [Given(@"I made an Authorization request")]
        public void WhenIMakeAnAuthorizationRequest()
        {
            System.Threading.Thread.Sleep(1000);

            _directoryServiceClientContext.Authorize(
                _directoryClientContext.CurrentUserId,
                null,
                null
            );
        }

        [When(@"I attempt to make an Authorization request")]
        public void WhenIAttemptToMakeAnAuthorizationRequest()
        {
            try
            {
                WhenIMakeAnAuthorizationRequest();
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to make an Authorization request for the User identified by ""(.*)""")]
        public void WhenIAttemptToMakeAnAuthorizationRequestForTheUserIdentifiedBy(string userId)
        {
            try
            {
                _directoryServiceClientContext.Authorize(userId, null, null);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I attempt to make an Authorization request with the context value ""(.*)""")]
        public void WhenIAttemptToMakeAnAuthorizationRequestWithTheContextValue(string context)
        {
            try
            {
                _directoryServiceClientContext.Authorize(Guid.NewGuid().ToString(), context, null);
            }
            catch (BaseException e)
            {
                _commonContext.RecordException(e);
            }
        }

        [When(@"I get the response for the Authorization request")]
        public void WhenIGetTheResponseForTheAuthRequest()
        {
            string authRequestID = _directoryServiceClientContext._lastAuthorizationRequest.Id;
            AuthorizationResponse authResponse = _directoryServiceClientContext.GetAuthResponse(authRequestID);
        }

        [When(@"I get the response for the Advanced Authorization request")]
        public void WhenIGetTheResponseForTheAdvancedAuthRequest()
        {
            string authRequestID = _directoryServiceClientContext._lastAuthorizationRequest.Id;
            AdvancedAuthorizationResponse authResponse = _directoryServiceClientContext.GetAdvancedAuthorizationResponse(authRequestID);
        }

        [Then(@"the Authorization response should be approved")]
        public void ThenTheAuthResponseShouldBeApproved()
        {
            var currentAuth = _directoryServiceClientContext._lastAuthorizationResponse;
            if (currentAuth is null)
            {
                throw new Exception("Auth response was not found when it was expected");
            }

            if (currentAuth.Authorized != true)
            {
                throw new Exception($"Auth was not approved when it should have been: {currentAuth.ToString()}");
            }
        }

        [Then(@"the Authorization response should be denied")]
        public void ThenTheAuthResponseShouldBeDenied()
        {
            var currentAuth = _directoryServiceClientContext._lastAuthorizationResponse;
            if (currentAuth is null)
            {
                throw new Exception("Auth response was not found when it was expected");
            }

            if (currentAuth.Authorized != false)
            {
                throw new Exception($"Auth was approved when it should not have been: {currentAuth.ToString()}");
            }
        }

        [Then(@"the Authorization response should contain the following methods:")]
        public void ThenTheAuthorizationResponseShouldContainTheFollowingMethods(Table table)
        {
            var authTestMethods = table.CreateSet<AuthMethod>();
            var authResponseMethods = _directoryServiceClientContext._lastAuthorizationResponse.AuthMethods;

            foreach (AuthMethod authTestMethod in authTestMethods)
            {
                foreach (var authResponseMethod in authResponseMethods)
                {
                    if (authTestMethod.Method == authResponseMethod.Method)
                    {
                        Assert.AreEqual(authTestMethod.Active, authResponseMethod.Active);
                        Assert.AreEqual(authTestMethod.Allowed, authResponseMethod.Allowed);
                        Assert.AreEqual(authTestMethod.Supported, authResponseMethod.Supported);
                        Assert.AreEqual(authTestMethod.Error, authResponseMethod.Error);
                        Assert.AreEqual(authTestMethod.Passed, authResponseMethod.Passed);
                        Assert.AreEqual(authTestMethod.Set, authResponseMethod.Set);
                        Assert.AreEqual(authTestMethod.UserRequired, authResponseMethod.UserRequired);
                        break;
                    }
                }
            }
        }

        [When(@"I make a Policy based Authorization request for the User")]
        public void WhenIMakeAPolicyAuthorizationRequest()
        {
            System.Threading.Thread.Sleep(1000);

            _directoryServiceClientContext.Authorize(
                _directoryClientContext.CurrentUserId,
                null,
                new AuthPolicy(
                    _numFactors,
                    _knowledge,
                    _inherence,
                    _possession,
                    _jailbreak,
                    _locations
                )
            );
        }

        [Then(@"the Authorization response should contain a geofence with a radius of (.*), a latitude of (.*), and a longitude of (.*)")]
        public void ThenTheAuthorizationResponseShouldContainAGeofence(double radius, double latitude, double longitude)
        {
            var locations = _directoryServiceClientContext._lastAuthorizationResponse.AuthPolicy.Locations;
            var testLocation = new Location(radius, latitude, longitude, "");
            Console.WriteLine(testLocation);
            Console.WriteLine(locations);
            CollectionAssert.Contains(locations, testLocation);
        }

        [Given(@"the current Authorization Policy requires a geofence with a radius of (.*), a latitude of (.*), a longitude of (.*), and a name of ""(.*)""")]
        public void GivenTheCurrentAuthorizationPolicyRequiresAGeofenceWithARadiusOfALatitudeOfALongitudeOfAndNamed(double radius, double latitude, double longitude, string name)
        {
            _locations.Add(new Location(radius, latitude, longitude, name));
        }

        [Then(@"the Authorization response should contain a geofence with a radius of (.*), a latitude of (.*), a longitude of (.*), and a name of ""(.*)""")]
        public void ThenTheAuthorizationResponseShouldContainAGeofenceWithARadiusOfALatitudeOfALongitudeOfAndANameOf(double radius, double latitude, double longitude, string name)
        {
            var locations = _directoryServiceClientContext._lastAuthorizationResponse.AuthPolicy.Locations;
            var testLocation = new Location(radius, latitude, longitude, name);
            CollectionAssert.Contains(locations, testLocation);
        }

        [Then(@"the Authorization response should require inherence")]
        public void ThenTheAuthorizationResponseShouldRequireInherence()
        {
            Assert.AreEqual(true, _directoryServiceClientContext._lastAuthorizationResponse.AuthPolicy.RequireInherenceFactor);
        }

        [Then(@"the Authorization response should require possession")]
        public void ThenTheAuthorizationResponseShouldRequirePossession()
        {
            Assert.AreEqual(true, _directoryServiceClientContext._lastAuthorizationResponse.AuthPolicy.RequirePosessionFactor);
        }

        [Then(@"the Authorization response should require knowledge")]
        public void ThenTheAuthorizationResponseShouldRequireKnowledge()
        {
            Assert.AreEqual(true, _directoryServiceClientContext._lastAuthorizationResponse.AuthPolicy.RequireKnowledgeFactor);
        }

        [Then(@"the Authorization response should not require inherence")]
        public void ThenTheAuthorizationResponseShouldNotRequireInherence()
        {
            Assert.AreEqual(false, _directoryServiceClientContext._lastAuthorizationResponse.AuthPolicy.RequireInherenceFactor);
        }

        [Then(@"the Authorization response should not require possession")]
        public void ThenTheAuthorizationResponseShouldNotRequirePossession()
        {
            Assert.AreEqual(false, _directoryServiceClientContext._lastAuthorizationResponse.AuthPolicy.RequirePosessionFactor);
        }


        [Then(@"the Authorization response should not require knowledge")]
        public void ThenTheAuthorizationResponseShouldNotRequireKnowledge()
        {
            Assert.AreEqual(false, _directoryServiceClientContext._lastAuthorizationResponse.AuthPolicy.RequireKnowledgeFactor);
        }

        [Then(@"the Authorization response should require (.*) factors")]
        public void ThenTheAuthorizationResponseShouldRequireFactors(int numOfFactors)
        {
            Assert.AreEqual(numOfFactors, _directoryServiceClientContext._lastAuthorizationResponse.AuthPolicy.RequiredFactors);
        }

        [Then(@"the Advanced Authorization response should require Inherence")]
        public void ThenTheAdvancedAuthorizationResponseShouldRequireInherence()
        {
            Assert.AreEqual(true, _directoryServiceClientContext._lastAdvancedAuthorizationResponse.Policy.InherenceRequired);
        }

        [Then(@"the Advanced Authorization response should have the requirement ""(.*)""")]
        public void ThenTheAdvancedAuthorizationResponseShouldHaveTheRequirement(string type)
        {
            Assert.AreEqual(type, _directoryServiceClientContext._lastAdvancedAuthorizationResponse.Policy.Requirement.ToString());
        }

        [Then(@"the Advanced Authorization response should have amount set to (.*)")]
        public void ThenTheAdvancedAuthorizationResponseShouldHaveAmountSetTo(int amount)
        {
            Assert.AreEqual(amount, _directoryServiceClientContext._lastAdvancedAuthorizationResponse.Policy.Amount);
        }

        [Then(@"the Advanced Authorization response should contain a GeoCircleFence with a radius of (.*), a latitude of (.*), a longitude of (.*), and a name of ""(.*)""")]
        public void ThenTheAdvancedAuthorizationResponseShouldContainAGeoCircleFence(int radius, double latitude, double longitude, string name)
        {
            bool fenceFound = false;
            foreach (var fence in _directoryServiceClientContext._lastAdvancedAuthorizationResponse.Policy.Fences)
            {
                if (fence.Name == name)
                {
                    Assert.AreEqual(radius, (fence as GeoCircleFence).Radius);
                    Assert.AreEqual(latitude, (fence as GeoCircleFence).Latitude);
                    Assert.AreEqual(longitude, (fence as GeoCircleFence).Longitude);
                    fenceFound = true;
                }
            }
            Assert.IsTrue(fenceFound);
        }

        [Then(@"the Advanced Authorization response should contain a TerritoryFence with a country of ""(.*)"", a administrative area of ""(.*)"", a postal code of ""(.*)"", and a name of ""(.*)""")]
        public void ThenTheAdvancedAuthorizationResponseShouldContainATerritoryFence(string country, string adminArea, string postalCode, string name)
        {
            bool fenceFound = false;
            foreach (var fence in _directoryServiceClientContext._lastAdvancedAuthorizationResponse.Policy.Fences)
            {
                if (fence.Name == name)
                {
                    Assert.AreEqual(country, (fence as TerritoryFence).Country);
                    Assert.AreEqual(adminArea, (fence as TerritoryFence).AdministrativeArea);
                    Assert.AreEqual(postalCode, (fence as TerritoryFence).PostalCode);
                    fenceFound = true;
                }
            }
            Assert.IsTrue(fenceFound);
        }

        [Then(@"the Authorization Request response Device IDs matches the current Devices list")]
        public void ThenTheAuthorizationRequestResponseDeviceIDsMatchesTheCurrentDevicesList()
        {
            Client.AuthorizationRequest authorizationRequest = _directoryServiceClientContext._lastAuthorizationRequest;
            _directoryClientContext.LoadDevicesForCurrentUser();
            if(authorizationRequest is null)
                throw new Exception("Expected Auth Request to be present but was null");

            List<string> deviceIDs = authorizationRequest.DeviceIds;

            if (deviceIDs is null || deviceIDs.Count == 0)
                throw new Exception("Expected device IDs to be present in Auth Request but was null");

            var currentDevices = _directoryClientContext.LoadedDevices;

            foreach(var device in currentDevices)
            {
                if (deviceIDs.Contains(device.Id) == false)
                {
                    throw new Exception(string.Format("Expected Device ID {0} not found in device list {1}", device, currentDevices));
                }

            }
        }

    }
}
