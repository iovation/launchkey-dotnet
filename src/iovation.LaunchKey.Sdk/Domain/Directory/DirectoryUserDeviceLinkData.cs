namespace iovation.LaunchKey.Sdk.Domain.Directory
{
    /// <summary>
    /// An API response for beginning a device link.
    /// </summary>
    public class DirectoryUserDeviceLinkData
    {
        /// <summary>
        /// The code the user must type on the destination device to complete the linking process.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// An HTTP URL for a QR code image which may be displayed to the user, for the purpose of scanning it from the linking device.
        /// </summary>
        public string QrCode { get; }

        public DirectoryUserDeviceLinkData(string code, string qrCode)
        {
            Code = code;
            QrCode = qrCode;
        }
    }
}