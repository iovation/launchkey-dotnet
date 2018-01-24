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
					(ServiceAuthOptions opts) => ServiceExamples.DoServiceAuthorization(opts.Username, opts.ServiceId, opts.PrivateKeyPath),
					(ServiceAuthWebhookOptions opts) => ServiceExamples.DoServiceAuthorizationWebhook(opts.Username, opts.ServiceId, opts.PrivateKeyPath),
					(ServiceAuthWithPolicy opts) => ServiceExamples.DoServiceAuthorizationWithPolicy(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.JailbreakDetection, opts.Factors, opts.Geofence),
					(ServiceSessionStartOptions opts) => ServiceExamples.DoSessionStart(opts.Username, opts.ServiceId, opts.PrivateKeyPath),
					(ServiceSessionEndOptions opts) => ServiceExamples.DoSessionEnd(opts.Username, opts.ServiceId, opts.PrivateKeyPath),

					// directory functions
					(DirectoryLinkDeviceOptions opts) => DirectoryExamples.DoDeviceLink(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId),
					(DirectoryUnlinkDeviceOptions opts) => DirectoryExamples.DoDeviceUnlink(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.DeviceId),
					(DirectoryListDevicesOptions opts) => DirectoryExamples.DoDeviceList(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId),
					(DirectorySessionListOptions opts) => DirectoryExamples.DoSessionList(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId),
					(DirectorySessionPurgeOptions opts) => DirectoryExamples.DoSessionPurge(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId),
					(DirectoryServiceAuthOptions opts) => DirectoryExamples.DoDirectoryServiceAuth(opts.DirectoryId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId),
					(DirectoryServiceSessionStartOptions opts) => DirectoryExamples.DoDirectoryServiceSessionStart(opts.DirectoryId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId),
					(DirectoryServiceSessionEndOptions opts) => DirectoryExamples.DoDirectoryServiceSessionEnd(opts.DirectoryId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId),
					
					// org functions
					(OrgDirectoryListDevicesOptions opts) => OrgExamples.DoDirectoryDeviceList(opts.OrganizationId, opts.PrivateKeyPath, opts.DirectoryId, opts.UserId),
					(OrgServiceAuthOptions opts) => OrgExamples.DoServiceAuth(opts.OrganizationId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId),

					// errors
					(errs) => 1
				);
		}
	}
}
