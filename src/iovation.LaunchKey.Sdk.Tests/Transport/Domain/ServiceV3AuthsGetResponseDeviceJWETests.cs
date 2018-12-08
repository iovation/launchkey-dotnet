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
	public class ServiceV3AuthsGetResponseDeviceJWETests
	{
		[TestMethod]
		public void ShouldDeserialize()
		{
			var json = "{" +
				"    \"type\": \"DENIED\"," +
				"    \"reason\": \"FRAUDULENT\"," +
				"    \"denial_reason\": \"DEN2\"," +
				"    \"device_id\": \"5d1acf5c-dc5d-11e7-9ea1-0469f8dc10a5\"," +
				"    \"service_pins\": [\"2648\", \"2046\", \"0583\", \"2963\", \"2046\"]," +
				"    \"auth_request\": \"8c3c0268-f692-11e7-bd2e-7692096aba47\"" +
				"}"; ;
			var o = JsonConvert.DeserializeObject<ServiceV3AuthsGetResponseDeviceJWE>(json);

			Assert.AreEqual("DENIED", o.Type);
			Assert.AreEqual("FRAUDULENT", o.Reason);
			Assert.AreEqual("DEN2", o.DenialReason);
			Assert.AreEqual(Guid.Parse("8c3c0268-f692-11e7-bd2e-7692096aba47"), o.AuthorizationRequestId);
			Assert.AreEqual("5d1acf5c-dc5d-11e7-9ea1-0469f8dc10a5", o.DeviceId);
			Assert.AreEqual("2648", o.ServicePins[0]);
			Assert.AreEqual("2046", o.ServicePins[1]);
			Assert.AreEqual("0583", o.ServicePins[2]);
			Assert.AreEqual("2963", o.ServicePins[3]);
			Assert.AreEqual("2046", o.ServicePins[4]);
		}
	}
}
