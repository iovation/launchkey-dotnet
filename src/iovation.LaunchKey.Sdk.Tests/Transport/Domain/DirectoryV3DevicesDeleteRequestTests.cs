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
	public class DirectoryV3DevicesDeleteRequestTests
	{
		[TestMethod]
		public void Constructor_ShouldSetProperties()
		{
			var deviceGuid = Guid.NewGuid();
			var o = new DirectoryV3DevicesDeleteRequest("id", deviceGuid);
			Assert.AreEqual(o.Identifier, "id");
			Assert.AreEqual(o.DeviceId, deviceGuid);
		}

		[TestMethod]
		public void ShouldSerializeCorrectly()
		{
			var encoder = new JsonNetJsonEncoder();
			var deviceGuid = Guid.Parse("c4491b0f-70e4-44c0-82e7-d48127d5a77b");
			var o = new DirectoryV3DevicesDeleteRequest("id", deviceGuid);
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"identifier\":\"id\",\"device_id\":\"c4491b0f-70e4-44c0-82e7-d48127d5a77b\"}", json);
		}
	}
}
