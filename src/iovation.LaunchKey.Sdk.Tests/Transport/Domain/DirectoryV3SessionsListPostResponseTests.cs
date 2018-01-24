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
	public class DirectoryV3SessionsListPostResponseTests
	{
		[TestMethod]
		public void ShouldDeserialize()
		{
			var json = "[{\"auth_request\": null, \"service_id\": \"1f8d859a-f0c2-11e7-8d6e-b2432b556c77\", \"service_icon\": \"https://d2882u593o0m3.cloudfront.net/blank.png\", \"service_name\": \"Jogo White Label Service\", \"date_created\": \"2018-01-11T05:20:55Z\"}]";
			var obj = JsonConvert.DeserializeObject<List<DirectoryV3SessionsListPostResponse.Session>>(json);

			Assert.IsTrue(obj.Count == 1);

			var s = obj[0];
			Assert.IsNull(s.AuthRequest);
			Assert.AreEqual(Guid.Parse("1f8d859a-f0c2-11e7-8d6e-b2432b556c77"), s.ServiceId);
			Assert.AreEqual("https://d2882u593o0m3.cloudfront.net/blank.png", s.ServiceIcon);
			Assert.AreEqual("Jogo White Label Service", s.ServiceName);
			Assert.AreEqual(new DateTime(2018, 1, 11, 5, 20, 55, DateTimeKind.Utc), s.Created);
		}

		[TestMethod]
		public void Constructor_ShouldSetSessions()
		{
			var sessions = new List<DirectoryV3SessionsListPostResponse.Session>();
			var response = new DirectoryV3SessionsListPostResponse(sessions);

			Assert.AreSame(response.Sessions, sessions);
		}
	}
}
