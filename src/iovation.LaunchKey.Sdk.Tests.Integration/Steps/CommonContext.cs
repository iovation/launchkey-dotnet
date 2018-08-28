using System;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps
{
	public class CommonContext
	{
		public void RecordException(Exception ex)
		{
			ScenarioContext.Current.Add("LastException", ex);
		}

		public Exception GetLastException()
		{
			var exception = ScenarioContext.Current.Get<Exception>("LastException");
			return exception;
		}
	}
}