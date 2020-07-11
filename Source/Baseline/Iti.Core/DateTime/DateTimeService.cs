using System;
using Autofac;
using Iti.Inversion;

namespace Iti.Core.DateTime
{
    public static class DateTimeService
    {
        private static IDateTimeService _handler;
        private static readonly object LockObject = new object();

        private static IDateTimeService Handler
        {
            get
            {
                if (_handler == null)
                {
                    lock (LockObject)
                    {
                        if (_handler == null)
                        {
                            _handler = IOC.TryResolveStatic<IDateTimeService>(() => new WindowsDateTimeService());
                        }
                    }
                }

                return _handler;
            }
        }

        //

        public static DateTimeOffset UtcNow => Handler.UtcNow();

        //

        public static string DefaultTimeZoneId => Handler.DefaultTimeZoneId;
        public static TimeZoneInfo DefaultTimeZone => Lookup(DefaultTimeZoneId);

        public static DateTimeOffset ToUtc(string timeZoneId, DateTimeOffset localDateTime) => Handler.ToUtc(timeZoneId, localDateTime);

        public static DateTimeOffset FromUtc(string timeZoneId, DateTimeOffset utcDateTime) => Handler.FromUtc(timeZoneId, utcDateTime);

        public static TimeZoneInfo Lookup(string timeZoneId) => Handler.Lookup(timeZoneId);
    }
}
