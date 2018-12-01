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
			var o = new ServiceV3AuthsPostRequest("un", authPolicy, "ctx", "title", 999, "Push Title", "Push Body");

			Assert.AreSame(o.AuthPolicy, authPolicy);
			Assert.AreEqual(o.Context, "ctx");
			Assert.AreEqual(o.Username, "un");
			Assert.AreEqual(o.Title, "title");
			Assert.AreEqual(o.TTL, 999);
			Assert.AreEqual(o.PushTitle, "Push Title");
			Assert.AreEqual(o.PushBody, "Push Body");
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_JustUserId()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", null, null, null, null, null, null);
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"username\":\"my-unique-user-identifier\"}", json);
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_WithContext()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", null, "Authorizing charge for $12.34 at iovation.com", null, null, null, null);
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"username\":\"my-unique-user-identifier\",\"context\":\"Authorizing charge for $12.34 at iovation.com\"}", json);
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_WithTitle()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", null, null, "Title", null, null, null);
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"username\":\"my-unique-user-identifier\",\"title\":\"Title\"}", json);
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_WithTTL()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", null, null, null, 999, null, null);
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"username\":\"my-unique-user-identifier\",\"ttl\":999}", json);
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_WithPolicy()
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
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", policy, null, null, null, null, null);
			var json = encoder.EncodeObject(o);
			var expected = "{\"username\":\"my-unique-user-identifier\",\"policy\":{\"minimum_requirements\":[{\"requirement\":\"authenticated\",\"any\":2}],\"factors\":[{\"factor\":\"geofence\",\"requirement\":\"forced requirement\",\"priority\":1,\"attributes\":{\"locations\":[{\"radius\":60.0,\"latitude\":27.175,\"longitude\":78.0422}]}}]}}";

			Assert.AreEqual(expected, json);
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_WithPushTitle()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", null, null, null, null, "Push Title", null);
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"username\":\"my-unique-user-identifier\",\"push_title\":\"Push Title\"}", json);
		}

		[TestMethod]
		public void ShouldSerializeCorrectly_WithPushBody()
		{
			var encoder = new JsonNetJsonEncoder();
			var o = new ServiceV3AuthsPostRequest("my-unique-user-identifier", null, null, null, null, null, "Push Body");
			var json = encoder.EncodeObject(o);
			Assert.AreEqual("{\"username\":\"my-unique-user-identifier\",\"push_body\":\"Push Body\"}", json);
		}
	}
}
