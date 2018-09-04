using System;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow
{
    public class Util
    {
		public static string UniqueName(string prefix)
		{
			return prefix + Guid.NewGuid().ToString("n");
		}

		public static string UniqueServiceName()
		{
			return UniqueName("Service");
		}

		public static string UniqueDirectoryName()
		{
			return UniqueName("Directory");
		}
	}
}
