using System;

namespace iovation.LaunchKey.Sdk.Time
{
    public class UnixTimeConverter : IUnixTimeConverter
    {
        private readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public long GetUnixTimestamp(DateTime time)
        {
            if (time.Kind != DateTimeKind.Utc)
                time = time.ToUniversalTime();

            var elapsed = time.Subtract(_epoch);
            return (long)elapsed.TotalSeconds;
        }

        public DateTime GetDateTime(long unixTimestamp)
        {
            return _epoch.AddSeconds(unixTimestamp);
        }
    }
}