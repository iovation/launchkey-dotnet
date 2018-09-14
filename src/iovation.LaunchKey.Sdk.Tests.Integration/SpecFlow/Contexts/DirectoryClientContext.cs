using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Directory;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
    public class DirectoryClientContext
    {
		private readonly TestConfiguration _testConfiguration;
		private readonly OrgClientContext _orgClientContext;

		public List<Session> LoadedSessions => _loadedSessions;

		private string _currentUserId;
		private List<Session> _loadedSessions;

		public DirectoryClientContext(TestConfiguration testConfiguration, OrgClientContext orgClientContext)
		{
			_testConfiguration = testConfiguration;
			_orgClientContext = orgClientContext;
		}

		private IDirectoryClient GetDirectoryClient()
		{
			return _testConfiguration.GetDirectoryClient(_orgClientContext.LastCreatedDirectory.Id.ToString());
		}

		public void LinkDevice(string userId)
		{
			GetDirectoryClient().LinkDevice(userId);
			_currentUserId = userId;
		}

		public void EndAllServiceSessions(string userId)
		{
			GetDirectoryClient().EndAllServiceSessions(userId);
		}

		public void EndAllServiceSessionsForCurrentUser()
		{
			EndAllServiceSessions(_currentUserId);
		}

		public void LoadSessions(string userId)
		{
			_loadedSessions = GetDirectoryClient().GetAllServiceSessions(userId);
		}

		public void LoadSessionsForCurrentUser()
		{
			LoadSessions(_currentUserId);
		}
	}
}
