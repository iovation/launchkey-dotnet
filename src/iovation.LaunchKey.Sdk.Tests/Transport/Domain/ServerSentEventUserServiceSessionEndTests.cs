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
	public class ServerSentEventUserServiceSessionEndTests
	{
		[TestMethod]
		public void ShouldDeserialize()
		{
			var json = "{\"api_time\": \"2018-01-11T05:22:17Z\", \"service_user_hash\": \"hash\"}";
			var o = JsonConvert.DeserializeObject<ServerSentEventUserServiceSessionEnd>(json);
			Assert.AreEqual("hash", o.UserHash);
			Assert.AreEqual(new DateTime(2018, 1, 11, 5, 22, 17, DateTimeKind.Utc), o.ApiTime);
		}
	}
}
