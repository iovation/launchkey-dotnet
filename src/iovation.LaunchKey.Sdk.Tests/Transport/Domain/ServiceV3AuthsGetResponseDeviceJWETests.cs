using System;
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
                           "\"type\": \"DENIED\"," +
                           "\"reason\": \"FRAUDULENT\"," +
                           "\"denial_reason\": \"DEN2\"," +
                           "\"device_id\": \"5d1acf5c-dc5d-11e7-9ea1-0469f8dc10a5\"," +
                           "\"service_pins\": [" +
                           "    \"2648\"," +
                           "    \"2046\"," +
                           "    \"0583\"," +
                           "    \"2963\"," +
                           "    \"2046\"" +
                           "]," +
                           "\"auth_request\": \"8c3c0268-f692-11e7-bd2e-7692096aba47\"," +
                           "\"auth_policy\": {" +
                           "    \"requirement\": null," +
                           "    \"geofences\": []" +
                           "}," +
                           "\"auth_methods\": [" +
                           "    {" +
                           "        \"method\": \"pin_code\"," +
                           "        \"set\": true," +
                           "        \"active\": true," +
                           "        \"allowed\": true," +
                           "        \"supported\": true," +
                           "        \"user_required\": false," +
                           "        \"passed\": true," +
                           "        \"error\": false" +
                           "    }" +
                           "]" +
                        "}";
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
            Assert.AreEqual(null, o.AuthPolicy.Requirement);
            Assert.AreEqual(0, o.AuthPolicy.Geofences.Length);
            Assert.AreEqual(1, o.AuthMethods.Length);
        }

        [TestMethod]
        public void AuthPolicyRequirementAmount()
        {
            var json = "{" +
                "    \"type\": \"DENIED\"," +
                "    \"reason\": \"FRAUDULENT\"," +
                "    \"denial_reason\": \"DEN2\"," +
                "    \"device_id\": \"5d1acf5c-dc5d-11e7-9ea1-0469f8dc10a5\"," +
                "    \"service_pins\": [\"2648\", \"2046\", \"0583\", \"2963\", \"2046\"]," +
                "    \"auth_request\": \"8c3c0268-f692-11e7-bd2e-7692096aba47\"," +
                "    \"auth_policy\": { \"requirement\": \"amount\", " +
                "                       \"amount\": 2," +
                "                       \"geofences\": [" +
                "                            {\"latitude\": 36.120825, \"longitude\": -115.157216, \"radius\": 200, \"name\": null}" +
                "                       ]}" +
                "}";
            var o = JsonConvert.DeserializeObject<ServiceV3AuthsGetResponseDeviceJWE>(json);

            Assert.AreEqual("amount", o.AuthPolicy.Requirement);
            Assert.AreEqual(2, o.AuthPolicy.Amount);
            Assert.AreEqual(36.120825, o.AuthPolicy.Geofences[0].Latitude);
            Assert.AreEqual(-115.157216, o.AuthPolicy.Geofences[0].Longitude);
            Assert.AreEqual(200, o.AuthPolicy.Geofences[0].Radius);
            Assert.AreEqual(null, o.AuthPolicy.Geofences[0].Name);

        }

        [TestMethod]
        public void AuthPolicyRequirementTypes()
        {
            var json = "{" +
                "    \"type\": \"DENIED\"," +
                "    \"reason\": \"FRAUDULENT\"," +
                "    \"denial_reason\": \"DEN2\"," +
                "    \"device_id\": \"5d1acf5c-dc5d-11e7-9ea1-0469f8dc10a5\"," +
                "    \"service_pins\": [\"2648\", \"2046\", \"0583\", \"2963\", \"2046\"]," +
                "    \"auth_request\": \"8c3c0268-f692-11e7-bd2e-7692096aba47\"," +
                "    \"auth_policy\": { \"requirement\": \"types\", " +
                "                       \"types\": [\"knowledge\"]," +
                "                       \"geofences\": [" +
                "                            {\"latitude\": 36.121020, \"longitude\": -115.156460, \"radius\": 550, \"name\": \"HQ North\"}," +
                "                            {\"latitude\": 34.121020, \"longitude\": -113.156460, \"radius\": 150, \"name\": \"HQ West\"}" +
                "                       ]}" +
                "}";
            var o = JsonConvert.DeserializeObject<ServiceV3AuthsGetResponseDeviceJWE>(json);

            Assert.AreEqual("types", o.AuthPolicy.Requirement);
            Assert.AreEqual("knowledge", o.AuthPolicy.Types[0]);
            Assert.AreEqual(2, o.AuthPolicy.Geofences.Length);
            Assert.AreEqual(36.121020, o.AuthPolicy.Geofences[0].Latitude);
            Assert.AreEqual(-115.156460, o.AuthPolicy.Geofences[0].Longitude);
            Assert.AreEqual(550, o.AuthPolicy.Geofences[0].Radius);
            Assert.AreEqual("HQ North", o.AuthPolicy.Geofences[0].Name);
            Assert.AreEqual(34.121020, o.AuthPolicy.Geofences[1].Latitude);
            Assert.AreEqual(-113.156460, o.AuthPolicy.Geofences[1].Longitude);
            Assert.AreEqual(150, o.AuthPolicy.Geofences[1].Radius);
            Assert.AreEqual("HQ West", o.AuthPolicy.Geofences[1].Name);
        }

        [TestMethod]
        public void ParseAuthMethods()
        {
            var json = "{" +
                           "\"type\": \"DENIED\"," +
                           "\"reason\": \"FRAUDULENT\"," +
                           "\"denial_reason\": \"DEN2\"," +
                           "\"device_id\": \"5d1acf5c-dc5d-11e7-9ea1-0469f8dc10a5\"," +
                           "\"service_pins\": [" +
                           "    \"2648\"," +
                           "    \"2046\"," +
                           "    \"0583\"," +
                           "    \"2963\"," +
                           "    \"2046\"" +
                           "]," +
                           "\"auth_request\": \"8c3c0268-f692-11e7-bd2e-7692096aba47\"," +
                           "\"auth_policy\": {" +
                           "    \"requirement\": null," +
                           "    \"geofences\": []" +
                           "}," +
                           "\"auth_methods\": [" +
                             "  {" +
                             "    \"method\": \"wearables\"," +
                             "    \"set\": false," +
                             "    \"active\": false," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  },  " +
                             "  {" +
                             "    \"method\": \"geofencing\"," +
                             "    \"set\": null," +
                             "    \"active\": true," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  }," +
                             "  {" +
                             "    \"method\": \"locations\"," +
                             "    \"set\": true," +
                             "    \"active\": true," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": true," +
                             "    \"passed\": true," +
                             "    \"error\": false" +
                             "  }," +
                             " {" +
                             "    \"method\": \"pin_code\"," +
                             "    \"set\": true," +
                             "    \"active\": true," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": true," +
                             "    \"passed\": true," +
                             "    \"error\": false" +
                             "  }," +
                             "  {" +
                             "    \"method\": \"circle_code\"," +
                             "    \"set\": false," +
                             "    \"active\": false," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  }," +
                             "  {" +
                             "    \"method\": \"face\"," +
                             "    \"set\": false," +
                             "    \"active\": false," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  }," +
                             "  {" +
                             "    \"method\": \"fingerprint\"," +
                             "    \"set\": false," +
                             "    \"active\": false," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  }" +
                             "]" +
                        "}";

            var o = JsonConvert.DeserializeObject<ServiceV3AuthsGetResponseDeviceJWE>(json);

            Assert.AreEqual(7, o.AuthMethods.Length);
            Assert.AreEqual("wearables", o.AuthMethods[0].Method);
            Assert.AreEqual(false, o.AuthMethods[0].Set);
            Assert.AreEqual(false, o.AuthMethods[0].Active);
            Assert.AreEqual(true, o.AuthMethods[0].Allowed);
            Assert.AreEqual(true, o.AuthMethods[0].Supported);
            Assert.AreEqual(null, o.AuthMethods[0].UserRequired);
            Assert.AreEqual(null, o.AuthMethods[0].Passed);
            Assert.AreEqual(null, o.AuthMethods[0].Error);

            Assert.AreEqual("geofencing", o.AuthMethods[1].Method);
            Assert.AreEqual(null, o.AuthMethods[1].Set);
            Assert.AreEqual(true, o.AuthMethods[1].Active);
            Assert.AreEqual(true, o.AuthMethods[1].Allowed);
            Assert.AreEqual(true, o.AuthMethods[1].Supported);
            Assert.AreEqual(null, o.AuthMethods[1].UserRequired);
            Assert.AreEqual(null, o.AuthMethods[1].Passed);
            Assert.AreEqual(null, o.AuthMethods[1].Error);

            Assert.AreEqual("locations", o.AuthMethods[2].Method);
            Assert.AreEqual(true, o.AuthMethods[2].Set);
            Assert.AreEqual(true, o.AuthMethods[2].Active);
            Assert.AreEqual(true, o.AuthMethods[2].Allowed);
            Assert.AreEqual(true, o.AuthMethods[2].Supported);
            Assert.AreEqual(true, o.AuthMethods[2].UserRequired);
            Assert.AreEqual(true, o.AuthMethods[2].Passed);
            Assert.AreEqual(false, o.AuthMethods[2].Error);

            Assert.AreEqual("pin_code", o.AuthMethods[3].Method);
            Assert.AreEqual(true, o.AuthMethods[3].Set);
            Assert.AreEqual(true, o.AuthMethods[3].Active);
            Assert.AreEqual(true, o.AuthMethods[3].Allowed);
            Assert.AreEqual(true, o.AuthMethods[3].Supported);
            Assert.AreEqual(true, o.AuthMethods[3].UserRequired);
            Assert.AreEqual(true, o.AuthMethods[3].Passed);
            Assert.AreEqual(false, o.AuthMethods[3].Error);

            Assert.AreEqual("circle_code", o.AuthMethods[4].Method);
            Assert.AreEqual(false, o.AuthMethods[4].Set);
            Assert.AreEqual(false, o.AuthMethods[4].Active);
            Assert.AreEqual(true, o.AuthMethods[4].Allowed);
            Assert.AreEqual(true, o.AuthMethods[4].Supported);
            Assert.AreEqual(null, o.AuthMethods[4].UserRequired);
            Assert.AreEqual(null, o.AuthMethods[4].Passed);
            Assert.AreEqual(null, o.AuthMethods[4].Error);

            Assert.AreEqual("face", o.AuthMethods[5].Method);
            Assert.AreEqual(false, o.AuthMethods[5].Set);
            Assert.AreEqual(false, o.AuthMethods[5].Active);
            Assert.AreEqual(true, o.AuthMethods[5].Allowed);
            Assert.AreEqual(true, o.AuthMethods[5].Supported);
            Assert.AreEqual(null, o.AuthMethods[5].UserRequired);
            Assert.AreEqual(null, o.AuthMethods[5].Passed);
            Assert.AreEqual(null, o.AuthMethods[5].Error);

            Assert.AreEqual("fingerprint", o.AuthMethods[6].Method);
            Assert.AreEqual(false, o.AuthMethods[6].Set);
            Assert.AreEqual(false, o.AuthMethods[6].Active);
            Assert.AreEqual(true, o.AuthMethods[6].Allowed);
            Assert.AreEqual(true, o.AuthMethods[6].Supported);
            Assert.AreEqual(null, o.AuthMethods[6].UserRequired);
            Assert.AreEqual(null, o.AuthMethods[6].Passed);
            Assert.AreEqual(null, o.AuthMethods[6].Error);

        }

        [TestMethod]
        public void TestMissingAuthPolicy()
        {
            var json = "{" +
                           "\"type\": \"DENIED\"," +
                           "\"reason\": \"FRAUDULENT\"," +
                           "\"denial_reason\": \"DEN2\"," +
                           "\"device_id\": \"5d1acf5c-dc5d-11e7-9ea1-0469f8dc10a5\"," +
                           "\"service_pins\": [" +
                           "    \"2648\"," +
                           "    \"2046\"," +
                           "    \"0583\"," +
                           "    \"2963\"," +
                           "    \"2046\"" +
                           "]," +
                           "\"auth_request\": \"8c3c0268-f692-11e7-bd2e-7692096aba47\"" +
                        "}";

            var o = JsonConvert.DeserializeObject<ServiceV3AuthsGetResponseDeviceJWE>(json);

            Assert.AreEqual(null, o.AuthPolicy);
            Assert.AreEqual(null, o.AuthMethods);

        }

        [TestMethod]
        public void TestMissingAuthPolicyGeofence()
        {
            var json = "{" +
                           "\"type\": \"DENIED\"," +
                           "\"reason\": \"FRAUDULENT\"," +
                           "\"denial_reason\": \"DEN2\"," +
                           "\"device_id\": \"5d1acf5c-dc5d-11e7-9ea1-0469f8dc10a5\"," +
                           "\"service_pins\": [" +
                           "    \"2648\"," +
                           "    \"2046\"," +
                           "    \"0583\"," +
                           "    \"2963\"," +
                           "    \"2046\"" +
                           "]," +
                           "\"auth_request\": \"8c3c0268-f692-11e7-bd2e-7692096aba47\"," +
                           "\"auth_policy\": {" +
                           "    \"requirement\": null" +
                           "}," +
                           "\"auth_methods\": [" +
                             "  {" +
                             "    \"method\": \"wearables\"," +
                             "    \"set\": false," +
                             "    \"active\": false," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  },  " +
                             "  {" +
                             "    \"method\": \"geofencing\"," +
                             "    \"set\": null," +
                             "    \"active\": true," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  }," +
                             "  {" +
                             "    \"method\": \"locations\"," +
                             "    \"set\": true," +
                             "    \"active\": true," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": true," +
                             "    \"passed\": true," +
                             "    \"error\": false" +
                             "  }," +
                             " {" +
                             "    \"method\": \"pin_code\"," +
                             "    \"set\": true," +
                             "    \"active\": true," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": true," +
                             "    \"passed\": true," +
                             "    \"error\": false" +
                             "  }," +
                             "  {" +
                             "    \"method\": \"circle_code\"," +
                             "    \"set\": false," +
                             "    \"active\": false," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  }," +
                             "  {" +
                             "    \"method\": \"face\"," +
                             "    \"set\": false," +
                             "    \"active\": false," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  }," +
                             "  {" +
                             "    \"method\": \"fingerprint\"," +
                             "    \"set\": false," +
                             "    \"active\": false," +
                             "    \"allowed\": true," +
                             "    \"supported\": true," +
                             "    \"user_required\": null," +
                             "    \"passed\": null," +
                             "    \"error\": null" +
                             "  }" +
                             "]" +
                        "}";

            var o = JsonConvert.DeserializeObject<ServiceV3AuthsGetResponseDeviceJWE>(json);

            Assert.AreEqual(null, o.AuthPolicy.Geofences);

        }

    }

}
