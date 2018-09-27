using System;
using System.Threading;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
	class OrgExamples
	{
		public static int DoServiceAuth(string orgId, string privateKey, string serviceId, string userId, string apiURL)
		{
			var serviceClient = ClientFactories.MakeOrganizationServiceClient(orgId, privateKey, serviceId, apiURL);
			try
			{
				var authorizationRequest = serviceClient.CreateAuthorizationRequest(userId);
				while (true)
				{
					Console.WriteLine("checking auth");

					// poll for a response
					var authResponse = serviceClient.GetAuthorizationResponse(authorizationRequest.Id);

					// if we got one, process it
					if (authResponse != null)
					{
						Console.WriteLine($"Auth response was {authResponse.Authorized}");
						return 0;
					}

					// if not, we are still waiting on the user. wait a bit ... 
					Thread.Sleep(1000);
				}
			}
			catch (AuthorizationRequestTimedOutError)
			{
				Console.WriteLine("user never replied.");
				return 1;
			}
			catch (BaseException e)
			{
				Console.WriteLine($"Error while authorizing user {userId} against service ID {serviceId}. Error: {e.Message}");
				return 1;
			}
		}

		public static int DoDirectoryDeviceList(string orgId, string privateKey, string directoryId, string userId, string apiURL)
		{
			var directoryClient = ClientFactories.MakeOrganizationDirectoryClient(orgId, privateKey, directoryId, apiURL);
			try
			{
				var deviceListResponse = directoryClient.GetLinkedDevices(userId);

				Console.WriteLine();
				Console.WriteLine($"Devices ({deviceListResponse.Count} found): ");
				Console.WriteLine("-------- ");
				Console.WriteLine();

				foreach (var device in deviceListResponse)
				{
					Console.WriteLine($" > Device {device.Id}");
					Console.WriteLine($"    Type: {device.Type}");
					Console.WriteLine($"    Status: {device.Status}");
					Console.WriteLine($"    Name: {device.Name}");
				}
				Console.WriteLine();
			}
			catch (BaseException e)
			{
				Console.WriteLine($"There was an error listing the devices for the user with ID {userId}. Error: {e.Message}");
			}

			return 0;
		}
		
	}
}
