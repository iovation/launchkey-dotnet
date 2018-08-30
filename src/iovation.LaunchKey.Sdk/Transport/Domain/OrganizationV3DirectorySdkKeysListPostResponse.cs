using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class OrganizationV3DirectorySdkKeysListPostResponse
	{
		public List<Guid> SdkKeys { get; }

		public OrganizationV3DirectorySdkKeysListPostResponse(List<Guid> sdkKeys)
		{
			SdkKeys = sdkKeys;
		}
	}
}