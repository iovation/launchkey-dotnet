using System;
using System.IO;
using System.Threading;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
	/// <summary>
	/// All samples in this class perform actions using Directory Credentials.
	/// e.g., service and directory operations using a private key issued from a Directory,
	/// acting on the directory itself and its child services
	/// </summary>
	class DirectoryExamples
	{
		/// <summary>
		/// Start a session for a directory service and user
		/// </summary>
		public static int DoDirectoryServiceSessionStart(string directoryId, string privateKey, string serviceId, string userId, string APIURL)
		{
			try
			{
				var serviceKeyContents = File.ReadAllText(privateKey);
				var factoryFactoryBuilder = new FactoryFactoryBuilder();
				if (APIURL != null)
				{
					factoryFactoryBuilder.SetApiBaseUrl(APIURL);
				}
				var factory = factoryFactoryBuilder.Build();
				var directoryFactory = factory.MakeDirectoryFactory(directoryId, serviceKeyContents);
				var serviceClient = directoryFactory.MakeServiceClient(serviceId);

				Console.WriteLine("Sending request to start session ... ");
				serviceClient.SessionStart(userId);
				Console.WriteLine("Request completed.");
			}
			catch (BaseException e)
			{
				Console.WriteLine($"Error starting a session for the directory user {userId} in directory {directoryId}. Error: {e.Message}");
			}
			return 0;
		}

		/// <summary>
		/// End a session for a directory service and user
		/// </summary>
		public static int DoDirectoryServiceSessionEnd(string directoryId, string privateKey, string serviceId, string userId, string APIURL)
		{
			try
			{
				var serviceKeyContents = File.ReadAllText(privateKey);
				var factoryFactoryBuilder = new FactoryFactoryBuilder();
				if (APIURL != null)
				{
					factoryFactoryBuilder.SetApiBaseUrl(APIURL);
				}
				var factory = factoryFactoryBuilder.Build();
				var directoryFactory = factory.MakeDirectoryFactory(directoryId, serviceKeyContents);
				var serviceClient = directoryFactory.MakeServiceClient(serviceId);

				Console.WriteLine("Sending request to end session ... ");
				serviceClient.SessionEnd(userId);
				Console.WriteLine("Request completed.");
			}
			catch (BaseException e)
			{
				Console.WriteLine($"Error ending session for the directory user {userId} in directory {directoryId}. Error: {e.Message}");
			}
			return 0;
		}

		/// <summary>
		/// Authorize a directory user against a directory service
		/// </summary>
		public static int DoDirectoryServiceAuth(string directoryId, string privateKey, string serviceId, string userId, string apiURL)
		{
			try
			{
				var serviceKeyContents = File.ReadAllText(privateKey);
				var factoryFactoryBuilder = new FactoryFactoryBuilder();
				if (apiURL != null)
				{
					factoryFactoryBuilder.SetApiBaseUrl(apiURL);
				}
				var factory = factoryFactoryBuilder.Build();
				var directoryFactory = factory.MakeDirectoryFactory(directoryId, serviceKeyContents);
				var serviceClient = directoryFactory.MakeServiceClient(serviceId);

				Console.WriteLine("Sending service auth request ... ");
				var authorizationRequest = serviceClient.CreateAuthorizationRequest(userId, context: "This is a 60 second auth", title: ".NET Service SDK Test Directory Service Auth", ttl: 60);
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
				Console.WriteLine($"Error while authorizing directory user {userId} within directory {directoryId} against service ID {serviceId}. Error: {e.Message}");
				return 1;
			}
		}

		/// <summary>
		/// List all devices associated with a user for the given directory and user id
		/// </summary>
		public static int DoDeviceList(string directoryId, string privateKey, string userId, string APIURL)
		{
			try
			{
				var serviceKeyContents = File.ReadAllText(privateKey);
				var factoryFactoryBuilder = new FactoryFactoryBuilder();
				if (APIURL != null)
				{
					factoryFactoryBuilder.SetApiBaseUrl(APIURL);
				}
				var factory = factoryFactoryBuilder.Build();
				var directoryFactory = factory.MakeDirectoryFactory(directoryId, serviceKeyContents);
				var directoryClient = directoryFactory.MakeDirectoryClient();

				Console.WriteLine("Sending request to get linked devices ... ");
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

		/// <summary>
		/// Unlink a device from a user
		/// </summary>
		public static int DoDeviceUnlink(string directoryId, string privateKey, string userId, string deviceId, string APIURL)
		{
			try
			{
				var serviceKeyContents = File.ReadAllText(privateKey);
				var factoryFactoryBuilder = new FactoryFactoryBuilder();
				if (APIURL != null)
				{
					factoryFactoryBuilder.SetApiBaseUrl(APIURL);
				}
				var factory = factoryFactoryBuilder.Build();
				var directoryFactory = factory.MakeDirectoryFactory(directoryId, serviceKeyContents);
				var directoryClient = directoryFactory.MakeDirectoryClient();

				Console.WriteLine("Sending request to unlink device ... ");
				directoryClient.UnlinkDevice(userId, deviceId);
				Console.WriteLine($"Successfully sent unlink request for device id {deviceId} on user with ID {userId}.");
			}
			catch (BaseException e)
			{
				Console.WriteLine($"There was an error unlinking the device {deviceId} for the user with ID {userId}. Error: {e.Message}");
			}

			return 0;
		}

		/// <summary>
		/// Link a device to a user. This starts the process which must be completed via the authenticator app for this directory
		/// </summary>
		public static int DoDeviceLink(string directoryId, string privateKey, string userId, string APIURL)
		{
			try
			{
				var serviceKeyContents = File.ReadAllText(privateKey);
				var factoryFactoryBuilder = new FactoryFactoryBuilder();
				if (APIURL != null)
				{
					factoryFactoryBuilder.SetApiBaseUrl(APIURL);
				}
				var factory = factoryFactoryBuilder.Build();
				var directoryFactory = factory.MakeDirectoryFactory(directoryId, serviceKeyContents);
				var directoryClient = directoryFactory.MakeDirectoryClient();

				Console.WriteLine("Sending request to begin device link ... ");
				var deviceLinkResponse = directoryClient.LinkDevice(userId);
				Console.WriteLine($"Successfully sent link request. Use the follwowing code to complete the link: {deviceLinkResponse.Code}");
			}
			catch (BaseException e)
			{
				Console.WriteLine($"There was an error beginning the device link for the user with ID {userId}. Error: {e.Message}");
			}

			return 0;
		}

		/// <summary>
		/// List all active sessions across all services for a user within this directory
		/// </summary>
		public static int DoSessionList(string directoryId, string privateKey, string userId, string APIURL)
		{
			try
			{
				var serviceKeyContents = File.ReadAllText(privateKey);
				var factoryFactoryBuilder = new FactoryFactoryBuilder();
				if (APIURL != null)
				{
					factoryFactoryBuilder.SetApiBaseUrl(APIURL);
				}
				var factory = factoryFactoryBuilder.Build();
				var directoryFactory = factory.MakeDirectoryFactory(directoryId, serviceKeyContents);
				var directoryClient = directoryFactory.MakeDirectoryClient();

				Console.WriteLine("Sending request to get list of sessions ... ");
				var sessions = directoryClient.GetAllServiceSessions(userId);

				Console.WriteLine();
				Console.WriteLine($"Sessions ({sessions.Count} found): ");
				Console.WriteLine("-------- ");
				Console.WriteLine();

				foreach (var session in sessions)
				{
					Console.WriteLine($"Session: serviceId = {session.ServiceId}, serviceName = {session.ServiceName}, created = {session.Created}");
				}
			}
			catch (BaseException e)
			{
				Console.WriteLine($"There was an error getting the active session list for the user {userId} in the directory {directoryId}. Error: {e.Message}");
			}

			return 0;
		}

		/// <summary>
		/// Purges all user sessions across all services for a user within this directory
		/// </summary>
		public static int DoSessionPurge(string directoryId, string privateKey, string userId, string APIURL)
		{
			try
			{
				var serviceKeyContents = File.ReadAllText(privateKey);
				var factoryFactoryBuilder = new FactoryFactoryBuilder();
				if (APIURL != null)
				{
					factoryFactoryBuilder.SetApiBaseUrl(APIURL);
				}
				var factory = factoryFactoryBuilder.Build();
				var directoryFactory = factory.MakeDirectoryFactory(directoryId, serviceKeyContents);
				var directoryClient = directoryFactory.MakeDirectoryClient();

				Console.WriteLine("Sending request to end all sessions ... ");
				directoryClient.EndAllServiceSessions(userId);
				Console.WriteLine("Done.");
			}
			catch (BaseException e)
			{
				Console.WriteLine($"There was an error ending the sessions for the user {userId} in the directory {directoryId}. Error: {e.Message}");
			}
			
			return 0;
		}
	}
}