﻿using System;
using System.IO;
using iovation.LaunchKey.Sdk.Client;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow
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

		public IOrganizationClient GetOrgClient()
		{
			var factory = new FactoryFactoryBuilder().Build();
			var organizationFactory = factory.MakeOrganizationFactory(OrgId, OrgPrivateKey);
			return organizationFactory.MakeOrganizationClient();
		}

		public IDirectoryClient GetDirectoryClient(string directoryId)
		{
			var factory = new FactoryFactoryBuilder().Build();
			var orgFactory = factory.MakeOrganizationFactory(OrgId, OrgPrivateKey);
			return orgFactory.MakeDirectoryClient(directoryId);
		}

		public IServiceClient GetServiceClient(string serviceId)
		{
			var factory = new FactoryFactoryBuilder().Build();
			var orgFactory = factory.MakeOrganizationFactory(OrgId, OrgPrivateKey);
			return orgFactory.MakeServiceClient(serviceId);
		}
	}
}