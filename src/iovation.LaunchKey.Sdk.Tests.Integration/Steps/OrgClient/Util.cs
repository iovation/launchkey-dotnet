using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps.OrgClient
{
    public class Util
    {
		public static string UniqueName(string prefix)
		{
			return prefix + Guid.NewGuid().ToString("n");
		}
	}
}
