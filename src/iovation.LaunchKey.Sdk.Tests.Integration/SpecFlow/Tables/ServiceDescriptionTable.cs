using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Tables
{
	class ServiceDescriptionTable
	{
		public string Description { get; set; }
		public string Icon { get; set; }
		public string CallbackUrl { get; set; }
		public bool Active { get; set; }
	}
}