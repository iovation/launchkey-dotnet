using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain;
using iovation.LaunchKey.Sdk.Domain.Directory;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    // TODO: Cleanup TOTP for the user since it is not deleted with the directory
    public class DirectoryTotpContext
        /// I think you can implement via the dispose() method the cleanup for TOTP
        //public class DirectoryTotpContext : IDisposable
    {
        private readonly TestConfiguration _testConfiguration;
        private readonly OrgClientContext _orgClientContext;
        public DirectoryUserTotp currentGenerateUserTotpResponse;

        public DirectoryTotpContext(TestConfiguration testConfiguration, OrgClientContext orgClientContext)
        {
            _testConfiguration = testConfiguration;
            _orgClientContext = orgClientContext;
        }

        private IDirectoryClient GetDirectoryClient()
        {
            return _testConfiguration.GetDirectoryClient(_orgClientContext.LastCreatedDirectory.Id.ToString());
        }

        public void GenerateUserTotp()
        {
            throw new NotImplementedException();
        }

        public void RemoveTotpCodeForUser()
        {
            throw new NotImplementedException();
        }

        public void RemoveTotpCodeForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public string getCodeForCurrentUserTotpResponse()
        {
            throw new NotImplementedException("You have to find a TOTP Library first!");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
