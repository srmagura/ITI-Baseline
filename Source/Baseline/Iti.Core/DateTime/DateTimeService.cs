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

        public static DateTimeOffset ToUtc(string timeZoneId, DateTimeOffset localDateTime) => Handler.ToUtc(timeZoneId, localDateTime);

        public static DateTimeOffset FromUtc(string timeZoneId, DateTimeOffset utcDateTime) => Handler.FromUtc(timeZoneId, utcDateTime);

        public static TimeZoneInfo Lookup(string timeZoneId) => Handler.Lookup(timeZoneId);
    }
}
