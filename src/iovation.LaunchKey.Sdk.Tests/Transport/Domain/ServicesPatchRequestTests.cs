using System;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class ServicesPatchRequestTests
	{
		[TestMethod]
		public void Constructor_ShouldSetProperties()
		{
			var req = new ServicesPatchRequest(TestConsts.DefaultServiceId, "n", "d", new Uri("http://a.com"), new Uri("http://b.com"), true);

			Assert.AreEqual(TestConsts.DefaultServiceId, req.ServiceId);
			Assert.AreEqual("n", req.Name);
			Assert.AreEqual("d", req.Description);
			Assert.AreEqual(new Uri("http://a.com"), req.Icon);
			Assert.AreEqual(new Uri("http://b.com"), req.CallbackUrl);
			Assert.AreEqual(true, req.Active);
		}
	}
}
