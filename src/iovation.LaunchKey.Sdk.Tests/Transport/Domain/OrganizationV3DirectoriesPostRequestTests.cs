using System;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class OrganizationV3DirectoriesPostRequestTests
	{
		[TestMethod]
		public void Constructor_ShouldSetProperties()
		{
			var request = new OrganizationV3DirectoriesPostRequest("n");
			Assert.AreEqual("n", request.Name);
		}
	}
}
