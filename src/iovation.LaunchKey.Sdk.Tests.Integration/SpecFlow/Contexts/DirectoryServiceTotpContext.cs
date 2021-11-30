using System;
using iovation.LaunchKey.Sdk.Client;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    public class DirectoryServiceTotpContext
    {
        private readonly TestConfiguration _testConfiguration;
        private readonly DirectoryClientContext _directoryClientContext;
        public bool CurrentVerifyUserResponse;

        public DirectoryServiceTotpContext(
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

        public void VerifyUserTotpCode(string userId, string totpCode)
        {
            CurrentVerifyUserResponse = GetServiceClientForCurrentService().VerifyTotp(userId, totpCode);
        }
    }
}
