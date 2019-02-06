using System;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
	public class CommonContext
	{
		private Exception _exception;

		public void RecordException(Exception ex)
		{
			_exception = ex;
		}

		public Exception GetLastException()
		{
			return _exception;
		}
	}
}