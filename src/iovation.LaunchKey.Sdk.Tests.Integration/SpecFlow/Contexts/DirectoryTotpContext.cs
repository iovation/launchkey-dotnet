using System;
using System.Collections.Generic;
using System.Text;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Directory;
using OtpNet;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    public class DirectoryTotpContext : IDisposable
    {
        private readonly TestConfiguration _testConfiguration;
        private readonly OrgClientContext _orgClientContext;
        private readonly List<string> _activeUserIds = new List<string>();
        public DirectoryUserTotp CurrentGenerateUserTotpResponse;

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
            string userId = Util.UniqueName("TOTP");
            CurrentGenerateUserTotpResponse = GetDirectoryClient().GenerateUserTotp(userId);
            _activeUserIds.Add(userId);
        }
        
        public void GenerateUserTotp(string userId)
        {
            CurrentGenerateUserTotpResponse = GetDirectoryClient().GenerateUserTotp(userId);
            _activeUserIds.Add(userId);
        }

        public void RemoveTotpCodeForUser()
        {
            GetDirectoryClient().RemoveUserTotp(Util.UniqueName("TOTP"));
        }

        public void RemoveTotpCodeForUser(string userId)
        {
            GetDirectoryClient().RemoveUserTotp(userId);
        }

        public string GetCodeForCurrentUserTotpResponse()
        {
            byte[] byteSecret = Encoding.ASCII.GetBytes(CurrentGenerateUserTotpResponse.Secret);

            OtpHashMode hashMode;
            switch (CurrentGenerateUserTotpResponse.Algorithm)
            {
                case "SHA256":
                    hashMode = OtpHashMode.Sha256;
                    break;
                case "SHA512":
                    hashMode = OtpHashMode.Sha512;
                    break;
                default:
                    hashMode = OtpHashMode.Sha1;
                    break;
            }
            
            var totp = new Totp(
                byteSecret, 
                mode: hashMode,
                step: CurrentGenerateUserTotpResponse.Period, 
                totpSize: CurrentGenerateUserTotpResponse.Digits
            );
            return totp.ComputeTotp();
        }

        public void Dispose()
        {
            foreach (var userId in _activeUserIds)
            {
                // This is failing with "HTTP Error: [403] The subject Directory must be valid and active. The parent Organization must be ..."
                // GetDirectoryClient().RemoveUserTotp(userId);
            }
        }
    }
}
