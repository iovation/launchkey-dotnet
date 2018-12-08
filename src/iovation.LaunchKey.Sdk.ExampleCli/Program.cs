using CommandLine;


namespace iovation.LaunchKey.Sdk.ExampleCli
{
	class Program
	{
		static void Main(string[] args)
		{
			CommandLine.Parser.Default
				.ParseArguments<
					ServiceAuthOptions,
					ServiceAuthWebhookOptions,
					ServiceAuthWithPolicy,
					ServiceSessionStartOptions,
					ServiceSessionEndOptions,
					DirectoryLinkDeviceOptions,
					DirectoryUnlinkDeviceOptions,
					DirectoryListDevicesOptions,
					DirectorySessionListOptions,
					DirectorySessionPurgeOptions,
					DirectoryServiceAuthOptions,
					DirectoryServiceSessionStartOptions,
					DirectoryServiceSessionEndOptions,
					OrgDirectoryListDevicesOptions,
					OrgServiceAuthOptions>(args)
				.MapResult(
					// service functions
					(ServiceAuthOptions opts) => ServiceExamples.DoServiceAuthorization(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.APIURL, opts.Context, opts.TTL, opts.AuthTitle, opts.PushTitle, opts.PushBody, opts.FraudDenialReasons, opts.NonFraudDenialReasons),
					(ServiceAuthWebhookOptions opts) => ServiceExamples.DoServiceAuthorizationWebhook(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.APIURL, opts.Context, opts.TTL, opts.AuthTitle, opts.PushTitle, opts.PushBody, opts.FraudDenialReasons, opts.NonFraudDenialReasons),
					(ServiceAuthWithPolicy opts) => ServiceExamples.DoServiceAuthorizationWithPolicy(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.JailbreakDetection, opts.Factors, opts.Geofence, opts.APIURL),
					(ServiceSessionStartOptions opts) => ServiceExamples.DoSessionStart(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.APIURL),
					(ServiceSessionEndOptions opts) => ServiceExamples.DoSessionEnd(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.APIURL),

					// directory functions
					(DirectoryLinkDeviceOptions opts) => DirectoryExamples.DoDeviceLink(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.APIURL),
					(DirectoryUnlinkDeviceOptions opts) => DirectoryExamples.DoDeviceUnlink(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.DeviceId, opts.APIURL),
					(DirectoryListDevicesOptions opts) => DirectoryExamples.DoDeviceList(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.APIURL),
					(DirectorySessionListOptions opts) => DirectoryExamples.DoSessionList(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.APIURL),
					(DirectorySessionPurgeOptions opts) => DirectoryExamples.DoSessionPurge(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.APIURL),
					(DirectoryServiceAuthOptions opts) => DirectoryExamples.DoDirectoryServiceAuth(opts.DirectoryId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId, opts.APIURL),
					(DirectoryServiceSessionStartOptions opts) => DirectoryExamples.DoDirectoryServiceSessionStart(opts.DirectoryId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId, opts.APIURL),
					(DirectoryServiceSessionEndOptions opts) => DirectoryExamples.DoDirectoryServiceSessionEnd(opts.DirectoryId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId, opts.APIURL),


					// org functions
					(OrgDirectoryListDevicesOptions opts) => OrgExamples.DoDirectoryDeviceList(opts.OrganizationId, opts.PrivateKeyPath, opts.DirectoryId, opts.UserId, opts.APIURL),
					(OrgServiceAuthOptions opts) => OrgExamples.DoServiceAuth(opts.OrganizationId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId, opts.APIURL),

					// errors
					(errs) => 1
				);
		}
	}
}
