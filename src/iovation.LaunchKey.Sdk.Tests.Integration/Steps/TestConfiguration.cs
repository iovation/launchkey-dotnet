using System.IO;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps
{
	public class TestConfiguration
	{
		public string OrgPrivateKey { get; }
		public string OrgId { get; set; }

		public TestConfiguration()
		{
			OrgPrivateKey = File.ReadAllText(Path.Combine("Secrets", "OrgPrivateKey.txt"));
			OrgId = File.ReadAllText(Path.Combine("Secrets", "OrgId.txt"));
		}
	}
}