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
        private static String naForNull(AuthorizationResponseType? value)
        {
            return naForNull(value.ToString());
        }

        private static String naForNull(AuthorizationResponseReason? value)
        {
            return naForNull(value.ToString());
        }

        private static String naForNull(String value)
        {
            return String.IsNullOrEmpty(value) ? "N/A" : value;
        }

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

        private static void PrintAuthorizationResponse(AuthorizationResponse authResponse)
        {
            Console.WriteLine($"Auth response was:");
            Console.WriteLine($"    Authorized:     {authResponse.Authorized}");
            Console.WriteLine($"    Type:           {naForNull(authResponse.Type)}");
            Console.WriteLine($"    Reason:         {naForNull(authResponse.Reason)}");
            Console.WriteLine($"    Denial Reason:  {naForNull(authResponse.DenialReason)}");
            Console.WriteLine($"    Fraud:          {authResponse.Fraud}");
            Console.WriteLine($"    Auth Request:   {authResponse.AuthorizationRequestId}");
            Console.WriteLine($"    Device Pins:    {String.Join(", ", authResponse.DevicePins)}");
            Console.WriteLine($"    Org User Hash:  {authResponse.OrganizationUserHash}");
            Console.WriteLine($"    Svc User Hash:  {authResponse.ServiceUserHash}");
            Console.WriteLine($"    User Push ID:   {authResponse.UserPushId}");
            Console.WriteLine($"    Device ID:      {authResponse.DeviceId}");
            Console.WriteLine($"    AuthPolicy:");
            Console.WriteLine($"       RequiredFactors:   {authResponse.AuthPolicy.RequiredFactors}");
            Console.WriteLine($"       RequiredKnowledge: {authResponse.AuthPolicy.RequireKnowledgeFactor}");
            Console.WriteLine($"       RequiredInherence: {authResponse.AuthPolicy.RequireInherenceFactor}");
            Console.WriteLine($"       RequiredPosession: {authResponse.AuthPolicy.RequirePosessionFactor}");
            Console.WriteLine($"       Location Count: {String.Join(", ", authResponse.AuthPolicy.Locations.Count)}");
            Console.WriteLine($"       Locations:");
            foreach (var item in authResponse.AuthPolicy.Locations)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine($"    Auth Methods:");
            foreach(var item in authResponse.AuthMethods)
            {
                Console.WriteLine("       {item.ToString()}");
            }
        }

        private static IList<DenialReason> GetDenialReasons(int? fraud, int? nonFraud)
        {
            List<DenialReason> denialReasons;

            if (fraud == null && nonFraud == null)
            {
                denialReasons = null;
            }
            else
            {
                fraud = fraud == null ? 0 : fraud;
                nonFraud = nonFraud == null ? 0 : nonFraud;
                denialReasons = new List<DenialReason>();
                for (int i = 0; i < Math.Max((int)fraud, (int)nonFraud); i++)
                {
                    if (i < fraud)
                    {
                        var reason = Path.GetRandomFileName().Replace(".", "");
                        var id = $"F{i}";
                        denialReasons.Add(new DenialReason(id, $"{reason} - {id}", true));
                    }
                    if (i < nonFraud)
                    {
                        var reason = Path.GetRandomFileName().Replace(".", "");
                        var id = $"NF{i}";
                        denialReasons.Add(new DenialReason(id, $"{reason} - {id}", false));
                    }
                }
            }
            return denialReasons;
        }
        private static IWebhookPackage WaitForWebhookResponse(IServiceClient serviceClient)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Sorry, your OS does not support the default windows HTTP listener. Webhook demo cannot proceed.");
                Environment.Exit(1);
            }
            Console.WriteLine("Webhook: Starting HTTP listener for localhost on port 9876.");
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

        public static int DoServiceAuthorizationWebhook(string username, string serviceId, string privateKey, string apiURL, string context, int? ttl, string title, string pushTitle, string pushBody, int? fraudDenialreasons, int? nonFraudDenialreasons)
        {
            var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL);

            try
            {
                serviceClient.CreateAuthorizationRequest(username, context: context, title: title, ttl: ttl, pushTitle: pushTitle, pushBody: pushBody, denialReasons: GetDenialReasons(fraudDenialreasons, nonFraudDenialreasons));
                var webhookPackage = WaitForWebhookResponse(serviceClient);
                var authPackage = webhookPackage as AuthorizationResponseWebhookPackage;
                if (authPackage != null)
                {
                    PrintAuthorizationResponse(authPackage.AuthorizationResponse);
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
            catch (AuthorizationRequestCanceled)
            {
                Console.WriteLine($"Authorization request was canceled.");
                return 1;
            }
            catch (AuthorizationInProgress e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"    Auth Request: {e.AuthorizationRequestId}");
                Console.WriteLine($"    Expires: {e.Expires}");
                Console.WriteLine($"    Same Service: {e.FromSameService}");
                return 1;
            }
            catch (BaseException e)
            {
                Console.WriteLine($"Error while authorizing user {username} against service ID {serviceId}. Error: {e.Message}");
                return 1;
            }
        }

        public static int DoServiceAuthorization(string username, string serviceId, string privateKey, string apiURL, string context, int? ttl, string title, string pushTitle, string pushBody, int? fraudDenialreasons, int? nonFraudDenialreasons)
        {
            var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL);

            try
            {
                var authorizationRequest = serviceClient.CreateAuthorizationRequest(username, context: context, title: title, ttl: ttl, pushTitle: pushTitle, pushBody: pushBody, denialReasons: GetDenialReasons(fraudDenialreasons, nonFraudDenialreasons));
                Console.WriteLine($"Auth Request Started: {authorizationRequest.Id}");
                while (true)
                {
                    Console.WriteLine("checking auth");

                    // poll for a response
                    var authResponse = serviceClient.GetAuthorizationResponse(authorizationRequest.Id);

                    // if we got one, process it
                    if (authResponse != null)
                    {
                        PrintAuthorizationResponse(authResponse);
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
            catch (AuthorizationRequestCanceled)
            {
                Console.WriteLine($"Authorization request was canceled.");
                return 1;
            }
            catch (AuthorizationInProgress e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"    Auth Request: {e.AuthorizationRequestId}");
                Console.WriteLine($"    Expires: {e.Expires}");
                Console.WriteLine($"    Same Service: {e.FromSameService}");
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
                        Console.WriteLine("geofence should be in the format lat:lon:radius:name");
                        return 1;
                    }
                    var lat = double.Parse(parts[0]);
                    var lon = double.Parse(parts[1]);
                    var rad = double.Parse(parts[2]);
                    var name = parts[3];

                    locations = new List<Location>();
                    locations.Add(new Location(rad, lat, lon, name));
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
                var authorizationRequest = serviceClient.CreateAuthorizationRequest(username, null, policy);
                while (true)
                {
                    Console.WriteLine("checking auth");

                    // poll for a response
                    var authResponse = serviceClient.GetAuthorizationResponse(authorizationRequest.Id);

                    // if we got one, process it
                    if (authResponse != null)
                    {
                        PrintAuthorizationResponse(authResponse);
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
            catch (AuthorizationInProgress e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"    Auth Request: {e.AuthorizationRequestId}");
                Console.WriteLine($"    Expires: {e.Expires}");
                Console.WriteLine($"    Same Service: {e.FromSameService}");
                return 1;
            }
            catch (AuthorizationRequestCanceled)
            {
                Console.WriteLine($"Authorization request was canceled.");
                return 1;
            }
            catch (BaseException e)
            {
                Console.WriteLine($"Error while authorizing user {username} against service ID {serviceId}. Error: {e.Message}");
                return 1;
            }
        }

        public static object DoServiceAuthorizationCancel(string serviceId, string privateKey, string apiURL, string authorizationRequestId)
        {
            int returnValue = 0;
            IServiceClient serviceClient;
            serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL);
            try
            {
                serviceClient.CancelAuthorizationRequest(authorizationRequestId);
                Console.WriteLine($"Authorization request {authorizationRequestId} was canceled.");
            }
            catch (EntityNotFound)
            {
                Console.WriteLine($"Cannot cancel authorization request {authorizationRequestId} as it cannot be found.");
                returnValue = 1;
            }
            catch (AuthorizationRequestCanceled)
            {
                Console.WriteLine($"Cannot cancel authorization request {authorizationRequestId} as it was already canceled.");
                returnValue = 1;
            }
            catch (AuthorizationResponseExists)
            {
                Console.WriteLine($"Cannot cancel authorization request {authorizationRequestId} as it has already been completed.");
                returnValue = 1;
            }
            return returnValue;
        }

    }
}