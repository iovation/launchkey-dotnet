using System;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Contexts
{
	public class CreatedDirectoryInfo
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public CreatedDirectoryInfo(Guid id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}