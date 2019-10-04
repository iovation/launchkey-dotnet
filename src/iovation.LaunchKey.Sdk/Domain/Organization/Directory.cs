using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Organization
{
    public class Directory
    {
        /// <summary>
        /// The unique ID of the directory
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the directory
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Whether this directory is active
        /// </summary>
        public bool Active { get; }

        /// <summary>
        /// A list of service IDs representing services within this directory
        /// </summary>
        public List<Guid> ServiceIds { get; }

        /// <summary>
        /// A list of SDK key IDs representing SDK keys within this directory
        /// </summary>
        public List<Guid> SdkKeys { get; }

        /// <summary>
        /// The key used for push notifications on Android devices
        /// </summary>
        public string AndroidKey { get; }

        /// <summary>
        /// The SHA256 fingerprint of the certificate used for push notifications in APNS
        /// </summary>
        public string IosCertificateFingerprint { get; }

        /// <summary>
        /// Whether to enable the user to select a reason why they denied a transaction
        /// </summary>
        public bool? DenialContextInquiryEnabled { get; }

        /// <summary>
        /// The webhook URL for the directory
        /// </summary>
        public string WebhookUrl { get; }

        public Directory(Guid id, string name, bool active, List<Guid> serviceIds, List<Guid> sdkKeys, string androidKey, string iosCertificateFingerprint, bool? denialContextInquiryEnabled = null, string webhookUrl = null)
        {
            Id = id;
            Name = name;
            Active = active;
            ServiceIds = serviceIds;
            SdkKeys = sdkKeys;
            AndroidKey = androidKey;
            IosCertificateFingerprint = iosCertificateFingerprint;
            DenialContextInquiryEnabled = denialContextInquiryEnabled;
            WebhookUrl = webhookUrl;
        }
    }
}