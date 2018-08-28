using System;
using System.IO;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps
{
	public class TestConfiguration
	{
		public string OrgPrivateKey { get; }
		public string OrgId { get; set; }

		public TestConfiguration()
		{
			var keyPath = Path.Combine("Secrets", "OrgPrivateKey.txt");
			var idPath = Path.Combine("Secrets", "OrgId.txt");

			if (!File.Exists(keyPath) || !File.Exists(idPath))
				throw new Exception($"Test configuration is invalid -- files with secrets should exist: {keyPath}, {idPath}");

			OrgPrivateKey = File.ReadAllText(keyPath);
			OrgId = File.ReadAllText(idPath);
		}
	}
}