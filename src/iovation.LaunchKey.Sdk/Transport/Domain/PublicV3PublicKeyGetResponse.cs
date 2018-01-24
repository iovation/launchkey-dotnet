namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class PublicV3PublicKeyGetResponse
	{
		public string PublicKey { get; }
		public string PublicKeyFingerPrint { get; }

		public PublicV3PublicKeyGetResponse(string publicKey, string publicKeyFingerPrint)
		{
			PublicKey = publicKey;
			PublicKeyFingerPrint = publicKeyFingerPrint;
		}
	}
}