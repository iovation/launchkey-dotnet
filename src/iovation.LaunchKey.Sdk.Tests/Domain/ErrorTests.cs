using iovation.LaunchKey.Sdk.Json;
using DomainError = iovation.LaunchKey.Sdk.Domain.Error;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Tests.Domain
{
	[TestClass]
	public class ErrorTests
	{
		[TestMethod]
		public void ShouldDeserializeErrorCodeAndDetailCorrectly()
		{
			JsonNetJsonEncoder encoder = new JsonNetJsonEncoder();
			string data = "{\"error_code\":\"Error Code\",\"error_detail\":\"Error Detail\"}";
			DomainError actual = encoder.DecodeObject<DomainError>(data);
			Assert.AreEqual("Error Code", actual.ErrorCode);
			Assert.AreEqual("Error Detail", actual.ErrorDetail);
		}

		[TestMethod]
		public void ShouldDeserializeBusySignalErrorDataCorrectly()
		{
			var expected = new Dictionary<String, Object>();
			expected.Add("auth_request", "adc0d351-d8a8-11e8-9fe8-acde48001122");
			expected.Add("my_auth", true);
			expected.Add("expires", new DateTime(2018, 11, 28, 22, 4, 44, DateTimeKind.Utc));

			JsonNetJsonEncoder encoder = new JsonNetJsonEncoder();
			string data = "{" +
				"\"error_code\": \"SVC-005\"," +
				"\"error_detail\": \"An auth already exists. Another one cannot be created until it is either responded to or expires in 250 seconds.\"," +
				"\"error_data\": {" +
					"\"auth_request\": \"adc0d351-d8a8-11e8-9fe8-acde48001122\"," +
					"\"my_auth\": true," +
					"\"expires\": \"2018-11-28T22:04:44Z\"" +
				"}" +
			"}";
			IDictionary<String, Object> actual = encoder.DecodeObject<DomainError>(data).ErrorData;
			Assert.AreEqual(3, actual.Count, message: "Unexpected number of items in ErrorData");
			Assert.IsTrue(actual.ContainsKey("auth_request"), "ErrorData has no auth_request key");
			Assert.AreEqual("adc0d351-d8a8-11e8-9fe8-acde48001122", actual["auth_request"]);
			Assert.IsTrue(actual.ContainsKey("my_auth"), "ErrorData has no my_auth key");
			Assert.AreEqual(true, actual["my_auth"]);
			Assert.IsTrue(actual.ContainsKey("expires"), "ErrorData has no expires key");
			Assert.AreEqual(new DateTime(2018, 11, 28, 22, 4, 44, DateTimeKind.Utc), actual["expires"]);
		}
	}
}
