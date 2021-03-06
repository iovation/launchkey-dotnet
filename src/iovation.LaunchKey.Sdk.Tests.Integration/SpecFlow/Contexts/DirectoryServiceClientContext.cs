﻿using System;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Service;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    public class DirectoryServiceClientContext
    {
        private readonly TestConfiguration _testConfiguration;
        private readonly DirectoryClientContext _directoryClientContext;
        public AuthorizationRequest _lastAuthorizationRequest;
        public AuthorizationResponse _lastAuthorizationResponse;
        public AdvancedAuthorizationResponse _lastAdvancedAuthorizationResponse;

        public DirectoryServiceClientContext(
            TestConfiguration testConfiguration,
            DirectoryClientContext directoryClientContext)
        {
            _testConfiguration = testConfiguration;
            _directoryClientContext = directoryClientContext;
        }

        private IServiceClient GetServiceClientForCurrentService()
        {
            if (_directoryClientContext.LastCreatedService == null)
                throw new Exception("Expected to have created a directory service before this.");

            return _testConfiguration.GetServiceClient(
                _directoryClientContext.LastCreatedService.Id.ToString()
            );
        }

        public AuthorizationResponse GetAuthResponse(string authId)
        {
            AuthorizationResponse authResponse = GetServiceClientForCurrentService().GetAuthorizationResponse(authId);
            _lastAuthorizationResponse = authResponse;
            return authResponse;
        }

        public AdvancedAuthorizationResponse GetAdvancedAuthorizationResponse(string authId)
        {
            AdvancedAuthorizationResponse authResponse = GetServiceClientForCurrentService().GetAdvancedAuthorizationResponse(authId);
            _lastAdvancedAuthorizationResponse = authResponse;
            return authResponse;
        }

        public void Authorize(string userId, string context, AuthPolicy authPolicy)
        {
            var authRequest = GetServiceClientForCurrentService()
                .CreateAuthorizationRequest(userId, context, authPolicy);
           
            _lastAuthorizationRequest = authRequest;
        }

        public void SessionStart(string userId, string requestId)
        {
            GetServiceClientForCurrentService()
                .SessionStart(userId, requestId);
        }

        public void SessionEnd(string userId)
        {
            GetServiceClientForCurrentService()
                .SessionEnd(userId);
        }
    }
}
