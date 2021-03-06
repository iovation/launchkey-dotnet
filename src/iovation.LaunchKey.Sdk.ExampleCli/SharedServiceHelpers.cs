using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Service;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using iovation.LaunchKey.Sdk.Domain.Webhook;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
    internal class SharedServiceHelpers
    {
        private static String PrintNull(double? value)
        {
            return PrintNull(value.ToString());
        }

        private static String PrintNull(AuthMethodType? value)
        {
            return PrintNull(value.ToString());
        }

        public static String PrintNull(AuthorizationResponseType? value)
        {
            return PrintNull(value.ToString());
        }

        public static String PrintNull(AuthorizationResponseReason? value)
        {
            return PrintNull(value.ToString());
        }

        private static String PrintNull(bool? value)
        {
            return PrintNull(value.ToString());
        }

        private static String PrintNull(String value)
        {
            return String.IsNullOrEmpty(value) ? "None" : value;
        }

        public static int HandleWebhook(IWebhookHandler handler, bool? advancedWebhook = false)
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
            if (advancedWebhook == true)
            {
                Console.WriteLine("Listening for an AdvancedWebhook!");
            }
            Console.WriteLine("Note: If using a reverse proxy (like ngrok) make sure you set the host header to localhost:9876");
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

                IWebhookPackage webhookPackage;
                if (advancedWebhook == true)
                {
                    Console.WriteLine("Listening for an AdvancedWebhook!");
                    webhookPackage = handler.HandleAdvancedWebhook(headers, body, "POST", "");
                }
                else
                {
                    webhookPackage = handler.HandleWebhook(headers, body, "POST", "");
                }

                if (webhookPackage is AuthorizationResponseWebhookPackage)
                {
                    var authPackage = webhookPackage as AuthorizationResponseWebhookPackage;
                    PrintAuthorizationResponse(authPackage.AuthorizationResponse);
                }
                else if (webhookPackage is ServiceUserSessionEndWebhookPackage)
                {
                    var sessionEndPackage = webhookPackage as ServiceUserSessionEndWebhookPackage;
                    Console.WriteLine($"Session remotely ended for Service User Hash {sessionEndPackage.ServiceUserHash} at {sessionEndPackage.LogoutRequested}");
                }
                else if (webhookPackage is DirectoryUserDeviceLinkCompletionWebhookPackage)
                {
                    var message = ((DirectoryUserDeviceLinkCompletionWebhookPackage)webhookPackage).DeviceLinkData;
                    Console.WriteLine($"You have a new linked device, congratulations!");
                    Console.WriteLine($"     DeviceID:          {message.DeviceId}");
                    Console.WriteLine($"     DevicePubKey:      \n{message.DevicePublicKey}");
                    Console.WriteLine($"     DevicePubKeyID:    {message.DevicePublicKeyId}");
                }
                else if (webhookPackage is AdvancedAuthorizationResponseWebhookPackage)
                {
                    var authPackage = webhookPackage as AdvancedAuthorizationResponseWebhookPackage;
                    PrintAdvancedAuthorizationResponse(authPackage.AdvancedAuthorizationResponse);
                }
                else
                {
                    Console.WriteLine("Error: received a webhook package but it was not for a known webhook type!");
                    return 1;
                }

                return 0;
            }
        }

        private static void PrintAuthorizationResponse(AuthorizationResponse authResponse)
        {
            Console.WriteLine($"Auth response was:");
            Console.WriteLine($"    Authorized:     {authResponse.Authorized}");
            Console.WriteLine($"    Type:           {PrintNull(authResponse.Type)}");
            Console.WriteLine($"    Reason:         {PrintNull(authResponse.Reason)}");
            Console.WriteLine($"    Denial Reason:  {PrintNull(authResponse.DenialReason)}");
            Console.WriteLine($"    Fraud:          {authResponse.Fraud}");
            Console.WriteLine($"    Auth Request:   {authResponse.AuthorizationRequestId}");
            Console.WriteLine($"    Device Pins:    {String.Join(", ", authResponse.DevicePins)}");
            Console.WriteLine($"    Org User Hash:  {authResponse.OrganizationUserHash}");
            Console.WriteLine($"    Svc User Hash:  {authResponse.ServiceUserHash}");
            Console.WriteLine($"    User Push ID:   {authResponse.UserPushId}");
            Console.WriteLine($"    Device ID:      {authResponse.DeviceId}");

            if (authResponse.AuthPolicy == null)
            {
                Console.WriteLine($"    AuthPolicy: None");
            }
            else
            {
                Console.WriteLine($"    AuthPolicy:");
                Console.WriteLine($"       RequiredFactors:   {PrintNull(authResponse.AuthPolicy.RequiredFactors)}");
                Console.WriteLine($"       RequiredKnowledge: {PrintNull(authResponse.AuthPolicy.RequireKnowledgeFactor)}");
                Console.WriteLine($"       RequiredInherence: {PrintNull(authResponse.AuthPolicy.RequireInherenceFactor)}");
                Console.WriteLine($"       RequiredPosession: {PrintNull(authResponse.AuthPolicy.RequirePosessionFactor)}");

                if (authResponse.AuthPolicy.Locations.Count > 0)
                {
                    Console.WriteLine($"       Geofence Count: {String.Join(", ", authResponse.AuthPolicy.Locations.Count)}");
                    Console.WriteLine($"       Geofences:");
                    foreach (var item in authResponse.AuthPolicy.Locations)
                    {
                        Console.WriteLine($"          Latitude:  {PrintNull(item.Latitude)}");
                        Console.WriteLine($"          Longitude: {PrintNull(item.Longitude)}");
                        Console.WriteLine($"          Radius:    {PrintNull(item.Radius)}");
                        Console.WriteLine($"          Name:      {PrintNull(item.Name)} \n");
                    }
                }
                else
                {
                    Console.WriteLine($"       Geofences: None");
                }
            }

            if (authResponse.AuthMethods == null)
            {
                Console.WriteLine($"    Auth Methods: None");
            }
            else
            {
                Console.WriteLine($"    Auth Methods:");
                foreach (var item in authResponse.AuthMethods)
                {
                    Console.WriteLine($"       Auth Method: {PrintNull(item.Method)}");
                    Console.WriteLine($"          Set: {PrintNull(item.Set)}");
                    Console.WriteLine($"          Active: {PrintNull(item.Active)}");
                    Console.WriteLine($"          Allowed: {PrintNull(item.Allowed)}");
                    Console.WriteLine($"          Supported: {PrintNull(item.Supported)}");
                    Console.WriteLine($"          User Required: {PrintNull(item.UserRequired)}");
                    Console.WriteLine($"          Passed: {PrintNull(item.Passed)}");
                    Console.WriteLine($"          Error: {PrintNull(item.Error)}");
                }
            }
        }


        private static void PrintAdvancedAuthorizationResponse(AdvancedAuthorizationResponse authResponse)
        {
            Console.WriteLine($"Auth response was:");
            Console.WriteLine($"    Authorized:     {authResponse.Authorized}");
            Console.WriteLine($"    Type:           {PrintNull(authResponse.Type)}");
            Console.WriteLine($"    Reason:         {PrintNull(authResponse.Reason)}");
            Console.WriteLine($"    Denial Reason:  {PrintNull(authResponse.DenialReason)}");
            Console.WriteLine($"    Fraud:          {authResponse.Fraud}");
            Console.WriteLine($"    Auth Request:   {authResponse.AuthorizationRequestId}");
            Console.WriteLine($"    Device Pins:    {String.Join(", ", authResponse.DevicePins)}");
            Console.WriteLine($"    Org User Hash:  {authResponse.OrganizationUserHash}");
            Console.WriteLine($"    Svc User Hash:  {authResponse.ServiceUserHash}");
            Console.WriteLine($"    User Push ID:   {authResponse.UserPushId}");
            Console.WriteLine($"    Device ID:      {authResponse.DeviceId}");

            if (authResponse.Policy == null)
            {
                Console.WriteLine($"    AuthResponse: None");
            }
            else
            {
                Console.WriteLine($"    AuthPolicy:");
                Console.WriteLine($"       Requirement:       {PrintNull(authResponse.Policy.Requirement.ToString())}");
                Console.WriteLine($"       Amount:            {PrintNull(authResponse.Policy.Amount)}");
                Console.WriteLine($"       RequiredKnowledge: {PrintNull(authResponse.Policy.KnowledgeRequired)}");
                Console.WriteLine($"       RequiredInherence: {PrintNull(authResponse.Policy.InherenceRequired)}");
                Console.WriteLine($"       RequiredPosession: {PrintNull(authResponse.Policy.PossessionRequired)}");

                if (authResponse.Policy.Fences.Count > 0)
                {
                    Console.WriteLine($"       Fence Count: {String.Join(", ", authResponse.Policy.Fences.Count)}");
                    Console.WriteLine($"       Fences:");
                    foreach (IFence item in authResponse.Policy.Fences)
                    {
                        if (item is TerritoryFence)
                        {
                            Console.WriteLine($"          Type:       TerritoryFence");
                            Console.WriteLine($"          AdminArea:  {PrintNull(((TerritoryFence)item).AdministrativeArea)}");
                            Console.WriteLine($"          Country:    {PrintNull(((TerritoryFence)item).Country)}");
                            Console.WriteLine($"          Name:       {PrintNull(((TerritoryFence)item).Name)}");
                            Console.WriteLine($"          PostalCode: {PrintNull(((TerritoryFence)item).PostalCode)}");
                        }
                        else if (item is GeoCircleFence)
                        {
                            Console.WriteLine($"          Type:      GeoCircleFence");
                            Console.WriteLine($"          Latitude:  {PrintNull(((GeoCircleFence)item).Latitude)}");
                            Console.WriteLine($"          Longitude: {PrintNull(((GeoCircleFence)item).Longitude)}");
                            Console.WriteLine($"          Radius:    {PrintNull(((GeoCircleFence)item).Radius)}");
                            Console.WriteLine($"          Name:      {PrintNull(((GeoCircleFence)item).Name)} \n");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"       Geofences: None");
                }
            }

            if (authResponse.AuthMethods == null)
            {
                Console.WriteLine($"    Auth Methods: None");
            }
            else
            {
                Console.WriteLine($"    Auth Methods:");
                foreach (var item in authResponse.AuthMethods)
                {
                    Console.WriteLine($"       Auth Method: {PrintNull(item.Method)}");
                    Console.WriteLine($"          Set: {PrintNull(item.Set)}");
                    Console.WriteLine($"          Active: {PrintNull(item.Active)}");
                    Console.WriteLine($"          Allowed: {PrintNull(item.Allowed)}");
                    Console.WriteLine($"          Supported: {PrintNull(item.Supported)}");
                    Console.WriteLine($"          User Required: {PrintNull(item.UserRequired)}");
                    Console.WriteLine($"          Passed: {PrintNull(item.Passed)}");
                    Console.WriteLine($"          Error: {PrintNull(item.Error)}");
                }
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

        private static int PollForResponse(IServiceClient serviceClient, AuthorizationRequest authorizationRequest)
        {
            try
            {
                Console.Write("Checking for response");
                while (true)
                {
                    Console.Write(".");
                    var authResponse = serviceClient.GetAuthorizationResponse(authorizationRequest.Id);

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
                Console.WriteLine("User did not reply before the Authorization request expired.");
            }
            catch (AuthorizationRequestCanceled)
            {
                Console.WriteLine($"Authorization request was canceled.");
            }
            catch (BaseException e)
            {
                Console.WriteLine($"Error while retrievieng authorization request {authorizationRequest.Id}. Error: {e.Message}");
            }
            return 1;
        }

        public static int DoAuthorizationRequest(IServiceClient serviceClient, string username, bool? useWebhook, bool? advancedWebhook = null, string context = null, AuthPolicy policy = null, string title = null, int? ttl = null, string pushTitle = null, string pushBody = null, int? fraudDenialreasons = null, int? nonFraudDenialreasons = null)
        {

            try
            {
                IList<DenialReason> denialReasons = GetDenialReasons(fraudDenialreasons, nonFraudDenialreasons);
                AuthorizationRequest authorizationRequest = serviceClient.CreateAuthorizationRequest(username, context, policy, title, ttl, pushTitle, pushBody, denialReasons);
                Console.WriteLine($"Auth Request Started: {authorizationRequest.Id}");
                if(authorizationRequest.DeviceIds != null)
                    Console.WriteLine($"Auth Request Sent to Devices: {string.Join(",", authorizationRequest.DeviceIds)}");

                if (useWebhook == true)
                {
                    return HandleWebhook(serviceClient, advancedWebhook);
                }
                else
                {
                    return PollForResponse(serviceClient, authorizationRequest);
                }
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
                Console.WriteLine($"Error while authorizing user {username}. Error: {e.Message}");
                return 1;
            }
        }

        public static int DoAuthorizationCancel(IServiceClient serviceClient, string authorizationRequestId)
        {
            int returnValue = 0;
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

        public static int DoSessionStart(IServiceClient serviceClient, string userId)
        {
            try
            {
                Console.WriteLine("Sending request to start session ... ");
                serviceClient.SessionStart(userId);
                Console.WriteLine("Request completed.");
                return 0;
            }
            catch (BaseException e)
            {
                Console.WriteLine($"Error starting a session for the user {userId}. Error: {e.Message}");
            }
            return 1;
        }

        public static int DoSessionEnd(IServiceClient serviceClient, string userId)
        {
            try
            {
                Console.WriteLine("Sending request to end session ... ");
                serviceClient.SessionEnd(userId);
                Console.WriteLine("Request completed.");
            }
            catch (BaseException e)
            {
                Console.WriteLine($"Error ending session for the user {userId}. Error: {e.Message}");
            }
            return 1;
        }
    }
}
