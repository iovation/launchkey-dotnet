using CommandLine;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
	class OrgOptions
	{
		[Option('o', "org-id", HelpText = "The unique ID of the organization being managed", Required = true)]
		public string OrganizationId { get; set; }

		[Option('p', "private-key", HelpText = "Path to a file containing your LaunchKey private key for the organization", Required = true)]
		public string PrivateKeyPath { get; set; }

		[Option('a', "api-url", HelpText = "URL of the API", Required = false)]
		public string APIURL { get; set; }
	}

	[Verb("org-directory-device-list", HelpText = "(Using Organization Credentials) List all devices linked to a directory user")]
	class OrgDirectoryListDevicesOptions : OrgOptions
	{
		[Option('d', "directory-id", HelpText = "The unique ID of the directory being managed", Required = true)]
		public string DirectoryId { get; set; }

		[Option('u', "user-id", HelpText = "The unique ID of the user the device is being linked for. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }		
	}

	[Verb("org-directory-update-webhook", HelpText = "(Using Organization Credentials) Update the Webhook URL of a directory")]
	class OrgDirectoryUpdateWebhookURLOptions : OrgOptions
	{
		[Option('w', "webhook-url", HelpText = "The webhook URL to set for the directory being managed", Required = true)]
		public string WebhookUrl { get; set; }

		[Option('d', "directory-id", HelpText = "The unique ID of the directory being managed", Required = true)]
		public string DirectoryId { get; set; }
	}

	[Verb("org-service-auth", HelpText = "(Using Organization Credentials) Authenticate a directory user against a service")]
	class OrgServiceAuthOptions : OrgOptions
	{
		[Option('s', "service-id", HelpText = "The unique ID of the service being managed", Required = true)]
		public string ServiceId { get; set; }

		[Option('u', "user-id", HelpText = "The unique ID of the directory user to authenticate. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }

		[Option('w', "use-webhooks", HelpText = "Whether to open a port to receive a response via a Webhook instead of polling", Required = false)]
		public bool? UseWebhook { get; set; }
	}

	class DirectoryOptions
	{
		[Option('d', "directory-id", HelpText = "The unique ID of the directory being managed", Required = true)]
		public string DirectoryId { get; set; }

		[Option('p', "private-key", HelpText = "Path to a file containing your LaunchKey private key for the directory", Required = true)]
		public string PrivateKeyPath { get; set; }

		[Option('a', "api-url", HelpText = "URL of the API", Required = false)]
		public string APIURL { get; set; }
	}

	[Verb("directory-device-link", HelpText = "Link a device for a directory user")]
	class DirectoryLinkDeviceOptions : DirectoryOptions
	{
		[Option('u', "user-id", HelpText = "The unique ID of the user the device is being linked for. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }

		[Option('t', "ttl", HelpText = "Time to Live in seconds for the linking code that will be generated. This must be between 300 and 86400.")]
		public int? TTL { get; set; }

		[Option('w', "use-webhooks", HelpText = "Whether to open a port to receive a response via a Webhook instead of polling", Required = false)]
		public bool? UseWebhook { get; set; }
	}

 	[Verb("directory-device-unlink", HelpText = "Unlink a device for a directory user")]
	class DirectoryUnlinkDeviceOptions : DirectoryOptions
	{
		[Option('u', "user-id", HelpText = "The unique ID of the user the device is being linked for. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }

		[Option('e', "device-id", HelpText = "The ID of the device to unlink. This can be retrieved with the devices-list command.", Required = true)]
		public string DeviceId { get; set; }
	}

	[Verb("directory-device-list", HelpText = "List all devices linked to a directory user")]
	class DirectoryListDevicesOptions : DirectoryOptions
	{
		[Option('u', "user-id", HelpText = "The unique ID of the user the device is being linked for. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }		
	}

	[Verb("directory-session-list", HelpText = "List all active sessions for a given user within a directory")]
	class DirectorySessionListOptions : DirectoryOptions
	{
		[Option('u', "user-id", HelpText = "The unique ID of the user to list sessions for. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }
	}

	[Verb("directory-service-auth", HelpText = "Authenticate a directory user against a directory service")]
	class DirectoryServiceAuthOptions : DirectoryOptions
	{
		[Option('s', "service-id", HelpText = "The unique ID of the service within the directory", Required = true)]
		public string ServiceId { get; set; }

		[Option('u', "user-id", HelpText = "The unique ID of the directory user to authenticate. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }

		[Option('w', "use-webhooks", HelpText = "Whether to open a port to receive a response via a Webhook instead of polling", Required = false)]
		public bool? UseWebhook { get; set; }
	}

	[Verb("directory-service-session-start", HelpText = "Start a session for a directory user against a directory service")]
	class DirectoryServiceSessionStartOptions : DirectoryOptions
	{
		[Option('s', "service-id", HelpText = "The unique ID of the service within the directory", Required = true)]
		public string ServiceId { get; set; }

		[Option('u', "user-id", HelpText = "The unique ID of the directory user to start a session for. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }
	}

	[Verb("directory-service-session-end", HelpText = "End a session for a directory user against a directory service")]
	class DirectoryServiceSessionEndOptions : DirectoryOptions
	{
		[Option('s', "service-id", HelpText = "The unique ID of the service within the directory", Required = true)]
		public string ServiceId { get; set; }

		[Option('u', "user-id", HelpText = "The unique ID of the directory user to end a session for. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }
	}

	[Verb("directory-session-purge", HelpText = "End all sessions across all child services for a directory user")]
	class DirectorySessionPurgeOptions : DirectoryOptions
	{
		[Option('u', "user-id", HelpText = "The unique ID of the directory user to purge sessions for. This should be in the format of a GUID/UUID.", Required = true)]
		public string UserId { get; set; }
	}

	class ServiceOptions
	{
		[Option('p', "private-key", HelpText = "Path to a file containing your LaunchKey private key for the service", Required = true)]
		public string PrivateKeyPath { get; set; }

		[Option('s', "service-id", HelpText = "The unique ID of the service", Required = true)]
		public string ServiceId { get; set; }

		[Option('a', "api-url", HelpText = "URL of the API", Required = false)]
		public string APIURL { get; set; }
	}

	[Verb("service-auth-policy", HelpText = "Authorize a user against a service with an auth policy")]
	class ServiceAuthWithPolicy : ServiceOptions
	{
		[Option('j', "jailbreak", HelpText = "Whether to force jailbreak detection")]
		public bool JailbreakDetection { get; set; }

		[Option('f', "geofence", HelpText = "Use a geofence. Expected format is lat:lon:radius:name")]
		public string Geofence { get; set; }

		[Option('c', "factors", HelpText = "Minimum number of factors to require")]
		public int? Factors { get; set; }

		[Option('u', "username", HelpText = "The username to authorize", Required = true)]
		public string Username { get; set; }
	}

	class AuthOptions : ServiceOptions
	{ 
		[Option('u', "username", HelpText = "The username to authorize", Required = true)]
		public string Username { get; set; }

		[Option('c', "context", HelpText = "The context value of the authorization request.")]
		public string Context { get; set; }

		[Option('l', "ttl", HelpText = "Time to Live in seconds for the request. This must be between 30 and 600.")]
		public int? TTL { get; set; }

		[Option('t', "title", HelpText = "[Directory Service Only] Title of the authorization request.")]
		public string AuthTitle { get; set; }

		[Option('h', "push-title", HelpText = "[Directory Service Only] Title of the push message for the authorization request.")]
		public string PushTitle { get; set; }

		[Option('b', "push-body", HelpText = "[Directory Service Only] Body of the push message for the authorization request.")]
		public string PushBody { get; set; }

		[Option('f', "fraud-denial-reasons", HelpText = "[Directory Service Only] The number of denial reasons flagged as fraud for the authorization request.")]
		public int? FraudDenialReasons { get; set; }

		[Option('n', "non-fraud-denial-reasons", HelpText = "[Directory Service Only] The number of denial reasons not flagged as fraud for the authorization request.")]
		public int? NonFraudDenialReasons { get; set; }
	}

	[Verb("service-auth", HelpText = "Authorize a user against a service using a polling method")]
	class ServiceAuthOptions : AuthOptions
	{
	
		[Option('w', "use-webhooks", HelpText = "Whether to open a port to receive a response via a Webhook instead of polling", Required = false)]
		public bool? UseWebhook { get; set; }

        [Option('o', "use-advanced-webhooks", HelpText = "Whether to use the new HandleAdvancedWebhook instead of deprecated HandleWebhook", Required = false)]
        public bool? AdvancedWebhook { get; set; }
	}

	[Verb("service-auth-cancel", HelpText = "Cancel an existing authorization request for a user")]
	class ServiceAuthCancelOptions : ServiceOptions
	{
		[Option('r', "auth-request", HelpText = "The ID of the authorization request you wish to cancel", Required = true)]
		public string AuthRequestId { get; set; }
	}

	[Verb("service-session-start", HelpText = "Start a session for a user")]
	class ServiceSessionStartOptions : ServiceOptions
	{
		[Option('u', "username", HelpText = "The username to start the session for", Required = true)]
		public string Username { get; set; }
	}

	[Verb("service-session-end", HelpText = "End a session for a user")]
	class ServiceSessionEndOptions : ServiceOptions
	{
		[Option('u', "username", HelpText = "The username to end the session for", Required = true)]
		public string Username { get; set; }
	}
}
