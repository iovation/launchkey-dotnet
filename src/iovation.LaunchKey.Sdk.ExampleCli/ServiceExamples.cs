using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Service;
using iovation.LaunchKey.Sdk.Domain.Webhook;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
	class ServiceExamples
	{
		public static int DoSessionStart(string username, string serviceId, string privateKey, string apiURL)
		{
			var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL);
			try
			{
				serviceClient.SessionStart(username);
			}
			catch (BaseException e)
			{
				Console.WriteLine($"There was an error starting the session for user {username} and service id {serviceId}. Error: {e.Message}");
			}

			return 0;
		}

		public static int DoSessionEnd(string username, string serviceId, string privateKey, string apiURL)
		{
			var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL);
			try
			{
				serviceClient.SessionEnd(username);
			}
			catch (BaseException e)
			{
				Console.WriteLine($"There was an error ending the session for user {username} and service id {serviceId}. Error: {e.Message}");
			}

			return 0;
		}

		private static IWebhookPackage WaitForWebhookResponse(IServiceClient serviceClient)
		{
			if (!HttpListener.IsSupported)
			{
				Console.WriteLine("Sorry, your OS does not support the default windows HTTP listener. Webhook demo cannot proceed.");
				Environment.Exit(1);
			}
			Console.WriteLine("Webhook: Starting HTTP listener.");
			var listener = new HttpListener();
			listener.Prefixes.Add("http://localhost:9876/");
			listener.Start();
			
			Console.WriteLine("Webhook: Waiting for a request ... ");
			var context = listener.GetContext();
			Console.WriteLine("Webhook: Request received");
			using (var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
			{
				var body = reader.ReadToEnd();
				var headers = new Dictionary<string, List<string>>();
				foreach (var headerName in context.Request.Headers.AllKeys)
				{
					headers.Add(headerName, new List<string>());
					foreach (var headerValue in context.Request.Headers.GetValues(headerName))
					{
						headers[headerName].Add(headerValue);
					}
				}
				return serviceClient.HandleWebhook(headers, body);
			}
		}

		public static int DoServiceAuthorizationWebhook(string username, string serviceId, string privateKey, string apiURL)
		{
			var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL);

			try
			{
				serviceClient.CreateAuthorizationRequest(username);
				var webhookPackage = WaitForWebhookResponse(serviceClient);
				var authPackage = webhookPackage as AuthorizationResponseWebhookPackage;
				if (authPackage != null)
				{
					Console.WriteLine($"Authorization webhook received: {authPackage.AuthorizationResponse.Authorized}");
				}
				else
				{
					Console.WriteLine($"Error: received a webhook package but it was not for an authorization!");
				}
				return 0;
			}
			catch (AuthorizationRequestTimedOutError)
			{
				Console.WriteLine("user never replied.");
				return 1;
			}
			catch (BaseException e)
			{
				Console.WriteLine($"Error while authorizing user {username} against service ID {serviceId}. Error: {e.Message}");
				return 1;
			}
		}

		public static int DoServiceAuthorization(string username, string serviceId, string privateKey, string apiURL)
		{
			var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL);

			try
			{
				var authorizationRequest = serviceClient.CreateAuthorizationRequest(username);
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
				Console.WriteLine($"Error while authorizing user {username} against service ID {serviceId}. Error: {e.Message}");
				return 1;
			}
		}

		public static int DoServiceAuthorizationWithPolicy(string username, string serviceId, string privateKey, bool jailbreakDetection, int? factors, string geofence, string apiURL)
		{
			var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL);

			List<Location> locations = null;
			// parse geofence input
			if (!string.IsNullOrWhiteSpace(geofence))
			{
				try
				{
					var parts = geofence.Split(':');
					if (parts.Length != 3)
					{
						Console.WriteLine("geofence should be in the format lat:lon:radius");
						return 1;
					}
					var lat = double.Parse(parts[0]);
					var lon = double.Parse(parts[1]);
					var rad = double.Parse(parts[2]);

					locations = new List<Location>();
					locations.Add(new Location(rad, lat, lon));
				}
				catch (FormatException)
				{
					Console.WriteLine("geofence parsing failed");
					return 1;
				}
			}
			Console.WriteLine($"Using policy: factors: {factors}, locations: {locations?.Count}, geofence: {geofence}, jailbreak: {jailbreakDetection}");
			try
			{
				var policy = new AuthPolicy(
					jailbreakDetection: jailbreakDetection,
					locations: locations,
					requiredFactors: factors
				);
				var authorizationRequest = serviceClient.CreateAuthorizationRequest(username, null,  policy);
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
				Console.WriteLine($"Error while authorizing user {username} against service ID {serviceId}. Error: {e.Message}");
				return 1;
			}
		}
	}
}