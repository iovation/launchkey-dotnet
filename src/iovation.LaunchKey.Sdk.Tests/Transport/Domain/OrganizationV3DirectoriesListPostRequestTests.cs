using System;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class OrganizationV3DirectoriesListPostRequestTests
	{
		[TestMethod]
		public void Constructor_ShouldSetProperties()
		{
			var request = new OrganizationV3DirectoriesListPostRequest(new System.Collections.Generic.List<Guid> {TestConsts.DefaultOrgId});

			Assert.IsNotNull(request.DirectoryIds);
			Assert.IsTrue(request.DirectoryIds.Count == 1);
			Assert.IsTrue(request.DirectoryIds[0] == TestConsts.DefaultOrgId);
		}
	}
}
