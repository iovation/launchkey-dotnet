﻿using CommandLine;


namespace iovation.LaunchKey.Sdk.ExampleCli
{
    class Program
    {
        static void Main(string[] args)
        {
            //You can only have 16 of these active at any given time!
            CommandLine.Parser.Default
                .ParseArguments<
                    ServiceAuthOptions,
                    ServiceAuthWithPolicy,
                    ServiceAuthCancelOptions,
                    ServiceSessionStartOptions,
                    ServiceSessionEndOptions,
                    DirectoryLinkDeviceOptions,
                    DirectoryUnlinkDeviceOptions,
                    DirectoryListDevicesOptions,
                    DirectorySessionListOptions,
                    DirectorySessionPurgeOptions,
                    DirectoryServiceAuthOptions,
                    OrgDirectoryListDevicesOptions,
                    OrgDirectoryUpdateWebhookURLOptions,
                    OrgServiceAuthOptions>(args)
                .MapResult(
                    // service functions
                    (ServiceAuthOptions opts) => ServiceExamples.DoServiceAuthorization(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.APIURL, opts.Context, opts.TTL, opts.AuthTitle, opts.PushTitle, opts.PushBody, opts.FraudDenialReasons, opts.NonFraudDenialReasons, opts.UseWebhook, opts.AdvancedWebhook),
                    (ServiceAuthWithPolicy opts) => ServiceExamples.DoServiceAuthorizationWithPolicy(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.JailbreakDetection, opts.Factors, opts.Geofence, opts.APIURL),
                    (ServiceAuthCancelOptions opts) => ServiceExamples.DoServiceAuthorizationCancel(opts.ServiceId, opts.PrivateKeyPath, opts.APIURL, opts.AuthRequestId),
                    (ServiceSessionStartOptions opts) => ServiceExamples.DoSessionStart(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.APIURL),
                    (ServiceSessionEndOptions opts) => ServiceExamples.DoSessionEnd(opts.Username, opts.ServiceId, opts.PrivateKeyPath, opts.APIURL),

                    // directory functions
                    (DirectoryLinkDeviceOptions opts) => DirectoryExamples.DoDeviceLink(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.APIURL, opts.TTL, opts.UseWebhook),
                    (DirectoryUnlinkDeviceOptions opts) => DirectoryExamples.DoDeviceUnlink(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.DeviceId, opts.APIURL),
                    (DirectoryListDevicesOptions opts) => DirectoryExamples.DoDeviceList(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.APIURL),
                    (DirectorySessionListOptions opts) => DirectoryExamples.DoSessionList(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.APIURL),
                    (DirectorySessionPurgeOptions opts) => DirectoryExamples.DoSessionPurge(opts.DirectoryId, opts.PrivateKeyPath, opts.UserId, opts.APIURL),
                    (DirectoryServiceAuthOptions opts) => DirectoryExamples.DoDirectoryServiceAuth(opts.DirectoryId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId, opts.APIURL, opts.UseWebhook),


                    // org functions
                    (OrgDirectoryListDevicesOptions opts) => OrgExamples.DoDirectoryDeviceList(opts.OrganizationId, opts.PrivateKeyPath, opts.DirectoryId, opts.UserId, opts.APIURL),
                    (OrgDirectoryUpdateWebhookURLOptions opts) => OrgExamples.DoUpdateDirectoryWebhookUrl(opts.OrganizationId, opts.PrivateKeyPath, opts.DirectoryId, opts.APIURL, opts.WebhookUrl),
                    (OrgServiceAuthOptions opts) => OrgExamples.DoServiceAuth(opts.OrganizationId, opts.PrivateKeyPath, opts.ServiceId, opts.UserId, opts.APIURL, opts.UseWebhook),

                    // errors
                    (errs) => 1
                );
        }
    }
}
