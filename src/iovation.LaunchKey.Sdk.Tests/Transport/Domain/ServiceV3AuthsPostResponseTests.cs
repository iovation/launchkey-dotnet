using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class ServiceV3AuthsPostResponseTests
	{
		[TestMethod]
		public void ShouldDeserialize()
		{
			var json = "{\"auth_request\": \"e4629564-f693-11e7-8e81-328aef89fa8b\"}";
			var o = JsonConvert.DeserializeObject<ServiceV3AuthsPostResponse>(json);

			Assert.AreEqual(Guid.Parse("e4629564-f693-11e7-8e81-328aef89fa8b"), o.AuthRequest);
		}
	}
}
