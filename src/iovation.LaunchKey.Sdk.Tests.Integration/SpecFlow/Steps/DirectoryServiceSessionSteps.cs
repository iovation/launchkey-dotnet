using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Steps
{
	[Binding]
    public class DirectoryServiceSessionSteps
    {
		private readonly CommonContext _commonContext;
		private readonly DirectoryClientContext _directoryClientContext;
		private readonly DirectoryServiceClientContext _directoryServiceClientContext;

		public DirectoryServiceSessionSteps(
			CommonContext commonContext,
			DirectoryClientContext directoryClientContext,
			DirectoryServiceClientContext directoryServiceClientContext)
		{
			_commonContext = commonContext;
			_directoryClientContext = directoryClientContext;
			_directoryServiceClientContext = directoryServiceClientContext;
		}

		[When(@"I send a Session Start request with no Auth Request ID")]
		public void WhenISendASessionStartRequestWithNoAuthRequestID()
		{
			_directoryServiceClientContext.SessionStart(
				_directoryClientContext.CurrentUserId, null
			);
		}

		[When(@"I send a Session Start request with Auth Request ID ""(.*)""")]
		public void WhenISendASessionStartRequestWithAuthRequestID(string requestId)
		{
			_directoryServiceClientContext.SessionStart(
				_directoryClientContext.CurrentUserId, requestId
			);
		}

		[When(@"I attempt to send a Session Start request for user ""(.*)""")]
		public void WhenIAttemptToSendASessionStartRequestForUser(string userId)
		{
			try
			{
				_directoryServiceClientContext.SessionStart(
					userId, null
				);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

		[When(@"I send a Session End request")]
		public void WhenISendASessionEndRequest()
		{
			_directoryServiceClientContext.SessionEnd(_directoryClientContext.CurrentUserId);
		}

		[Given(@"I sent a Session Start request")]
		public void GivenISentASessionStartRequest()
		{
			_directoryServiceClientContext.SessionStart(_directoryClientContext.CurrentUserId, null);
		}

		[When(@"I attempt to send a Session End request for user ""(.*)""")]
		public void WhenIAttemptToSendASessionEndRequestForUser(string userId)
		{
			try
			{
				_directoryServiceClientContext.SessionEnd(userId);
			}
			catch (BaseException e)
			{
				_commonContext.RecordException(e);
			}
		}

	}
}
