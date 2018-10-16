using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}
