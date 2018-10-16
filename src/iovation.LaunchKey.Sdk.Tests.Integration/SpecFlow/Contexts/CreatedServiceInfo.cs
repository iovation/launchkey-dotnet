using System;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
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