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
            Assert.AreEqual(InvalidRequestException.FromErrorCode("ARG-001", "Important error"), new InvalidParameters("Important error", null, "ARG-001"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("ARG-002", "Important error"), new InvalidRoute("Important error", null, "ARG-002"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-001", "Important error"), new ServiceNameTaken("Important error", null, "SVC-001"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-002", "Important error"), new InvalidPolicyInput("Important error", null, "SVC-002"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-003", "Important error"), new PolicyFailure("Important error", null, "SVC-003"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-004", "Important error"), new ServiceNotFound("Important error", null, "SVC-004"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("DIR-001", "Important error"), new InvalidDirectoryIdentifier("Important error", null, "DIR-001"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("KEY-001", "Important error"), new InvalidPublicKey("Important error", null, "KEY-001"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("KEY-002", "Important error"), new PublicKeyAlreadyInUse("Important error", null, "KEY-002"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("KEY-003", "Important error"), new PublicKeyDoesNotExist("Important error", null, "KEY-003"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("KEY-004", "Important error"), new LastRemainingKey("Important error", null, "KEY-004"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("ORG-003", "Important error"), new DirectoryNameInUse("Important error", null, "ORG-003"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("ORG-005", "Important error"), new LastRemainingSDKKey("Important error", null, "ORG-005"));
            Assert.AreEqual(InvalidRequestException.FromErrorCode("ORG-006", "Important error"), new InvalidSDKKey("Important error", null, "ORG-006"));
        }

        [TestMethod]
        public void FromStatusCodeSvc005_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = true;
            errorData["expires"] = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData), new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", true, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithNoErrorData_ShouldSetAllNulls()
        {
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-005", "Important error", null), new AuthorizationInProgress("Important error", null, "SVC-005", null, false, null));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithNoAuthRequest_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = true;
            errorData["expires"] = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData), new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", true, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithNoMyAuth_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["expires"] = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData), new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", false, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithInvalidMyAuth_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = "Not a boolean";
            errorData["expires"] = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData), new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", false, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithNoExpires_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = true;
            errorData["expires"] = null;
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData), new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", true, null));
        }

        [TestMethod]
        public void FromStatusCodeSvc005WithInvalidExpires_ShouldReturnExpectedData()
        {
            IDictionary<string, Object> errorData = new Dictionary<string, object>();
            errorData["auth_request"] = "adc0d351-d8a8-11e8-9fe8-acde48001122";
            errorData["from_same_service"] = true;
            errorData["expires"] = "Not a Date";
            Assert.AreEqual(InvalidRequestException.FromErrorCode("SVC-005", "Important error", errorData), new AuthorizationInProgress("Important error", null, "SVC-005", "adc0d351-d8a8-11e8-9fe8-acde48001122", true, null));
        }
    }
}
