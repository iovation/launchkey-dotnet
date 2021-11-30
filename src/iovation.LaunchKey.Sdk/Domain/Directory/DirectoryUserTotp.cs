namespace iovation.LaunchKey.Sdk.Domain.Directory
{
    /// <summary>
    /// An API response containing the data required for a TOTP request 
    /// </summary>
    public class DirectoryUserTotp
    {
        /// <summary>
        /// A base32 encoded string containing the shared secret.
        /// </summary>
        public string Secret;
        
        /// <summary>
        /// The hashing algorithm that will be used to calculate TOTP values.
        /// </summary>
        public string Algorithm;
        
        /// <summary>
        /// The period in seconds for which a TOTP code will be valid for.
        /// </summary>
        public int Period;
        
        /// <summary>
        /// The length in digits that a TOTP will be presented to the user.
        /// </summary>
        public int Digits;
        
        public DirectoryUserTotp(string secret, string algorithm, int period, int digits)
        {
            Secret = secret;
            Algorithm = algorithm;
            Period = period;
            Digits = digits;
        }

        public override string ToString()
        {
            return base.ToString() + "{" +
                   "secret = '" + Secret + "'" +
                   "algorithm = '" + Algorithm + "'" +
                   "period = '" + Period + "'" +
                   "digits = '" + Digits + "'" +
                   "}"
                ;
        }

        protected bool Equals(DirectoryUserTotp other)
        {
            return Secret == other.Secret && Algorithm == other.Algorithm && Period == other.Period && Digits == other.Digits;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DirectoryUserTotp) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Secret.GetHashCode();
                hashCode = (hashCode * 397) ^ Algorithm.GetHashCode();
                hashCode = (hashCode * 397) ^ Period;
                hashCode = (hashCode * 397) ^ Digits;
                return hashCode;
            }
        }
    }
}