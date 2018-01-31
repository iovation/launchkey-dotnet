using System;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class ServiceV3AuthsPostRequestTests
	{
		[TestMethod]
		public void Constructor_ShouldSetProperties()
		{
			var authPolicy = new AuthPolicy(null, null, null, null, null, null);
			var o = new ServiceV3AuthsPostRequest("un", authPolicy, "ctx");

			Assert.AreSame(o.AuthPolicy, authPolicy);
			Assert.AreEqual(o.Context, "ctx");
			Assert.AreEqual(o.Username, "un");
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_JustUserId()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", null, null);
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"username\":\"my-unique-user-identifier\"}",json);
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_WithContext()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", null, "Authorizing charge for $12.34 at iovation.com");
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"username\":\"my-unique-user-identifier\",\"context\":\"Authorizing charge for $12.34 at iovation.com\"}",json);
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_WithContextAndPolicy()
		{
			var encoder = new JsonNetJsonEncoder();
			var policy = new AuthPolicy(2, null, null, null, null, 
				new System.Collections.Generic.List<AuthPolicy.Location>
				{
					new AuthPolicy.Location
					{
						Radius = 60,
						Latitude = 27.175,
						Longitude = 78.0422
					}
				}
			);
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", policy, null);
			var json = encoder.EncodeObject(o);
			var expected = "{\"username\":\"my-unique-user-identifier\",\"policy\":{\"minimum_requirements\":[{\"requirement\":\"authenticated\",\"any\":2,\"knowledge\":0,\"inherence\":0,\"possession\":0}],\"factors\":[{\"factor\":\"geofence\",\"requirement\":\"forced requirement\",\"priority\":1,\"attributes\":{\"locations\":[{\"radius\":60.0,\"latitude\":27.175,\"longitude\":78.0422}]}}]}}";

			Assert.AreEqual(expected, json);
		}
	}
}
