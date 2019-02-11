using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Error;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Error
{
    [TestClass]
    public class InvalidRequestExceptionTests
    {
        [TestMethod]
        public void FromStatusCode_ShouldReturnExpectedTypes()
        {
            Assert.AreEqual(new InvalidParameters("Important error", null, "ARG-001"), InvalidRequestException.FromErrorCode("ARG-001", "Important error"));
            Assert.AreEqual(new InvalidRoute("Important error", null, "ARG-002"), InvalidRequestException.FromErrorCode("ARG-002", "Important error"));
            Assert.AreEqual(new ServiceNameTaken("Important error", null, "SVC-001"), InvalidRequestException.FromErrorCode("SVC-001", "Important error"));
            Assert.AreEqual(new InvalidPolicyInput("Important error", null, "SVC-002"), InvalidRequestException.FromErrorCode("SVC-002", "Important error"));
            Assert.AreEqual(new PolicyFailure("Important error", null, "SVC-003"), InvalidRequestException.FromErrorCode("SVC-003", "Important error"));
            Assert.AreEqual(new ServiceNotFound("Important error", null, "SVC-004"), InvalidRequestException.FromErrorCode("SVC-004", "Important error"));
            // SVC-005 covered inother tests
            Assert.AreEqual(new AuthorizationResponseExists("Important error", null, "SVC-006"), InvalidRequestException.FromErrorCode("SVC-006", "Important error"));
            Assert.AreEqual(new AuthorizationRequestCanceled("Important error", null, "SVC-007"), InvalidRequestException.FromErrorCode("SVC-007", "Important error"));
            Assert.AreEqual(new InvalidDirectoryIdentifier("Important error", null, "DIR-001"), InvalidRequestException.FromErrorCode("DIR-001", "Important error"));
            Assert.AreEqual(new InvalidPublicKey("Important error", null, "KEY-001"), InvalidRequestException.FromErrorCode("KEY-001", "Important error"));
            Assert.AreEqual(new PublicKeyAlreadyInUse("Important error", null, "KEY-002"), InvalidRequestException.FromErrorCode("KEY-002", "Important error"));
            Assert.AreEqual(new PublicKeyDoesNotExist("Important error", null, "KEY-003"), InvalidRequestException.FromErrorCode("KEY-003", "Important error"));
            Assert.AreEqual(new LastRemainingKey("Important error", null, "KEY-004"), InvalidRequestException.FromErrorCode("KEY-004", "Important error"));
            Assert.AreEqual(new DirectoryNameInUse("Important error", null, "ORG-003"), InvalidRequestException.FromErrorCode("ORG-003", "Important error"));
            Assert.AreEqual(new LastRemainingSDKKey("Important error", null, "ORG-005"), InvalidRequestException.FromErrorCode("ORG-005", "Important error"));
            Assert.AreEqual(new InvalidSDKKey("Important error", null, "ORG-006"), InvalidRequestException.FromErrorCode("ORG-006", "Important error"));
        }

        [TestMethod]
        public void FromStatusCodeSvc005_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = true;
            errorData["expires"] = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(
                new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", true, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData)
            );
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithNoErrorData_ShouldSetAllNulls()
        {
            Assert.AreEqual(new AuthorizationInProgress("Important error", null, "SVC-005", null, false, null), InvalidRequestException.FromErrorCode("SVC-005", "Important error", null));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithNoAuthRequest_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = true;
            errorData["expires"] = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(
                new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", true, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithNoMyAuth_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["expires"] = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(
                new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", false, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithInvalidMyAuth_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = "Not a boolean";
            errorData["expires"] = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(
                new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", false, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithNoExpires_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = true;
            errorData["expires"] = null;
            Assert.AreEqual(
                new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", true, null),
                InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithInvalidExpires_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = true;
            errorData["expires"] = "Not a Date";
            Assert.AreEqual(
                new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", true, null),
                InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData));
        }
    }
}
