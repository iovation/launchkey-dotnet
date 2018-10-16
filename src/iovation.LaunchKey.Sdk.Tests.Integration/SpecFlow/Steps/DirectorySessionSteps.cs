using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Directory;
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

		[Given(@"I made a Device linking request")]
		public void GivenIMadeADeviceLinkingRequest()
		{
			_directoryClientContext.LinkDevice(Util.UniqueUserName());
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
	}
}
