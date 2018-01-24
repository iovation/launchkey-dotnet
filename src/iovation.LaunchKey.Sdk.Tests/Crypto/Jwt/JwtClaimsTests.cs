using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Crypto.Jwt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Crypto.Jwt
{
	[TestClass]
	public class JwtClaimsTests
	{
		[TestMethod]
		public void TestSampleDeserializtion()
		{
			var json = "{\"aud\":\"svc:f8329e0e-dd7a-11e7-98b4-469158467b1a\",\"iss\":\"lka\",\"cty\":\"application/json\",\"nbf\":1513716853,\"jti\":\"c70da9cf498049ff9fd465c87d233d36\",\"exp\":1513716858,\"iat\":1513716853,\"response\":{\"status\":400,\"hash\":\"7214a12199440f792227fc9310193a2267f364ee59bfe0495ea690c31033325b\",\"func\":\"S256\"},\"sub\":\"svc:f8329e0e-dd7a-11e7-98b4-469158467b1a\"}";

			var claims = JwtClaims.FromJson(json);

			Assert.AreEqual(claims.Audience, "svc:f8329e0e-dd7a-11e7-98b4-469158467b1a");
			Assert.AreEqual(claims.Issuer, "lka");
			Assert.AreEqual(claims.Subject, "svc:f8329e0e-dd7a-11e7-98b4-469158467b1a");

			Assert.IsNotNull(claims.NotBefore);
			Assert.IsNotNull(claims.IssuedAt);
			Assert.IsNotNull(claims.ExpiresAt);

			Assert.AreEqual(claims.NotBefore, DateTime.Parse("12/19/2017 8:54:13 PM"));
			Assert.AreEqual(claims.IssuedAt, DateTime.Parse("12/19/2017 8:54:13 PM"));
			Assert.AreEqual(claims.ExpiresAt, DateTime.Parse("12/19/2017 8:54:18 PM"));

			Assert.AreEqual(claims.TokenId, "c70da9cf498049ff9fd465c87d233d36");

			Assert.IsNotNull(claims.Response);

			Assert.AreEqual(claims.Response.StatusCode, 400);
			Assert.AreEqual(claims.Response.ContentHash, "7214a12199440f792227fc9310193a2267f364ee59bfe0495ea690c31033325b");
			Assert.AreEqual(claims.Response.ContentHashAlgorithm, "S256");
		}
	}
}
