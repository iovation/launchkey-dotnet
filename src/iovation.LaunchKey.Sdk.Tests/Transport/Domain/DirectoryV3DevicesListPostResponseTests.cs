using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class DirectoryV3DevicesListPostResponseTests
	{
		[TestMethod]
		public void ShouldDeserialize()
		{
			var data = "[{\"status\": 1, \"updated\": \"2018-01-08T18:13:31Z\", \"name\": \"john\'s phone devin test\", \"created\": \"2018-01-08T18:13:31Z\", \"type\": \"IOS\", \"id\": \"77616b20-f49f-11e7-a34e-967362b04474\"}]";
			var encoder = new JsonNetJsonEncoder();
			var obj = encoder.DecodeObject<List<DirectoryV3DevicesListPostResponse.Device>>(data);

			Assert.IsTrue(obj.Count == 1);
			Assert.IsTrue(obj[0].Updated == new DateTime(2018, 1, 8, 18, 13, 31, DateTimeKind.Utc));
			Assert.IsTrue(obj[0].Created == new DateTime(2018, 1, 8, 18, 13, 31, DateTimeKind.Utc));
			Assert.IsTrue(obj[0].Name == "john\'s phone devin test");
			Assert.IsTrue(obj[0].Id == Guid.Parse("77616b20-f49f-11e7-a34e-967362b04474"));
			Assert.IsTrue(obj[0].Status == 1);
			Assert.IsTrue(obj[0].Type == "IOS");
		}

		[TestMethod]
		public void Constructor_ShouldSetDevices()
		{
			var devices = new List<DirectoryV3DevicesListPostResponse.Device>();
			var response = new DirectoryV3DevicesListPostResponse(devices);
			Assert.AreSame(response.Devices, devices);
		}
	}
}