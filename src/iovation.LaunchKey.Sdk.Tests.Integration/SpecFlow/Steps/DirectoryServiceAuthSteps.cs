using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

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

        private List<Location> _locations;

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
            _locations = new List<Location>
            {
                new Location(radius, lat, lon)
            };
        }

        [When(@"I make an Authorization request")]
        [Given(@"I made an Authorization request")]
        public void WhenIMakeAnAuthorizationRequest()
        {
            Console.WriteLine(_directoryClientContext.CurrentUserId);
            Console.WriteLine($"LastService: {_directoryClientContext.LastCreatedService}");

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

        [Then(@"the Authorization response should be approved")]
        public void ThenTheAuthResponseShouldBeApproved()
        {
            var currentAuth = _directoryServiceClientContext._lastAuthorizationResponse;
            if( currentAuth is null)
            {
                throw new Exception("Auth response was not found when it was expected");
            }

            if( currentAuth.Authorized != true)
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

    }
}
