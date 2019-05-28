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
    internal class SharedServiceHelpers
    {
        private static String naForNull(AuthorizationResponseType? value)
        {
            return naForNull(value.ToString());
        }

        private static String naForNull(int? value)
        {
            return naForNull(value.ToString());
        }

        private static String naForNull(AuthorizationResponseReason? value)
        {
            return naForNull(value.ToString());
        }

        private static String naForNull(AuthMethodType? value)
        {
            return naForNull(value.ToString());
        }

        private static String naForNull(bool? value)
        {
            return naForNull(value.ToString());
        }

        private static String naForNull(String value)
        {
            return String.IsNullOrEmpty(value) ? "N/A" : value;
        }

        public static int HandleWebhook(IWebhookHandler handler)
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
                    IWebhookPackage webhookPackage = handler.HandleWebhook(headers, body);

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
            Console.WriteLine($"       RequiredFactors:   {naForNull(authResponse.AuthPolicy.RequiredFactors)}");
            Console.WriteLine($"       RequiredKnowledge: {naForNull(authResponse.AuthPolicy.RequireKnowledgeFactor)}");
            Console.WriteLine($"       RequiredInherence: {naForNull(authResponse.AuthPolicy.RequireInherenceFactor)}");
            Console.WriteLine($"       RequiredPosession: {naForNull(authResponse.AuthPolicy.RequirePosessionFactor)}");
            Console.WriteLine($"       Location Count: {String.Join(", ", authResponse.AuthPolicy.Locations.Count)}");
            Console.WriteLine($"       Locations:");
            foreach (var item in authResponse.AuthPolicy.Locations)
            {
                Console.WriteLine(naForNull(item.ToString()));
            }
            Console.WriteLine($"    Auth Methods:");
            foreach (var item in authResponse.AuthMethods)
            {
                Console.WriteLine($"       Auth Method: {naForNull(item.Method)}");
                Console.WriteLine($"          Set: {naForNull(item.Set)}");
                Console.WriteLine($"          Active: {naForNull(item.Active)}");
                Console.WriteLine($"          Allowed: {naForNull(item.Allowed)}");
                Console.WriteLine($"          Supported: {naForNull(item.Supported)}");
                Console.WriteLine($"          User Required: {naForNull(item.UserRequired)}");
                Console.WriteLine($"          Passed: {naForNull(item.Passed)}");
                Console.WriteLine($"          Error: {naForNull(item.Error)}");
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

        public static int DoAuthorizationRequest(IServiceClient serviceClient, string username, string context = null, AuthPolicy policy = null, string title = null, int? ttl = null, string pushTitle = null, string pushBody = null, int? fraudDenialreasons = null, int? nonFraudDenialreasons = null, bool polling = true)
        {

            try
            {
                IList<DenialReason> denialReasons = GetDenialReasons(fraudDenialreasons, nonFraudDenialreasons);
                AuthorizationRequest authorizationRequest = serviceClient.CreateAuthorizationRequest(username, context, policy, title, ttl, pushTitle, pushBody, denialReasons);
                Console.WriteLine($"Auth Request Started: {authorizationRequest.Id}");
                if (polling)
                {
                    return PollForResponse(serviceClient, authorizationRequest);
                }
                else
                {
                    return HandleWebhook(serviceClient);
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
