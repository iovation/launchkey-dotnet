﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Error;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace iovation.LaunchKey.Sdk.Tests.Integration.Steps
{
	[Binding]
    public class CommonSteps
	{
		private readonly CommonContext _commonContext;

		public CommonSteps(CommonContext commonContext)
		{
			_commonContext = commonContext;
		}

		[Then(@"an? (.+) error occurs")]
		public void ThrowExceptionStep(string errorName)
		{
			var exception = _commonContext.GetLastException();
			Assert.IsNotNull(exception, "An exception was not thrown when one was expected.");
			Assert.AreEqual($"iovation.LaunchKey.Sdk.Error.{errorName}", exception.GetType().ToString(), "Exception was thrown but was the wrong type.");
		}
	}
}
