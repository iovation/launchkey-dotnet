using System;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class DirectoryV3DevicesPostRequestTests
	{
		[TestMethod]
		public void Constructor_ShouldSetProperties()
		{
			var o = new DirectoryV3DevicesPostRequest("id");
			Assert.AreEqual(o.Identifier, "id");
		}

		[TestMethod]
		public void ShouldSerializeCorrectly()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new DirectoryV3DevicesPostRequest("id");
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"identifier\":\"id\"}",json);
		}
	}
}
