using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class DirectoryV3DevicesListPostRequestTests
	{
		[TestMethod]
		public void Constructor_ShouldSetProperties()
		{
			var o = new DirectoryV3DevicesListPostRequest("id");
			Assert.AreEqual(o.Identifier, "id");
		}

		[TestMethod]
		public void ShouldSerializeCorrectly()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new DirectoryV3DevicesListPostRequest("id");
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"identifier\":\"id\"}", json);
		}
	}
}
