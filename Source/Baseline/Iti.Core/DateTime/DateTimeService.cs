using System;
using Iti.Inversion;

namespace Iti.Core.DateTime
{
    public static class DateTimeService
    {
        private static IDateTimeService _handler;

        private static IDateTimeService Handler => _handler ?? (_handler = IOC.TryResolve<IDateTimeService>() ?? new WindowsDateTimeService());

        //

        public static DateTimeOffset UtcNow => Handler.UtcNow();

        //

        public static string DefaultTimeZoneId => Handler.DefaultTimeZoneId;
        public static TimeZone DefaultTimeZone => TimeZone.Default;

        public static DateTimeOffset ToUtc(TimeZone tz, System.DateTime localDateTime) => Handler.ToUtc(tz, localDateTime);

        public static DateTimeOffset FromUtc(TimeZone tz, System.DateTime utcDateTime) => Handler.FromUtc(tz, utcDateTime);
    }
}
