using System;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps.OrgClient
{
	public class CreatedServiceInfo
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public CreatedServiceInfo(Guid id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}