using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Service;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    public class DirectoryServiceClientContext
    {
		private readonly TestConfiguration _testConfiguration;
		private readonly DirectoryClientContext _directoryClientContext;

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
			return GetServiceClientForCurrentService()
				.GetAuthorizationResponse(authId);
		}

		public void Authorize(string userId, string context, AuthPolicy authPolicy)
		{
			GetServiceClientForCurrentService()
				.CreateAuthorizationRequest(userId, context, authPolicy);
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
