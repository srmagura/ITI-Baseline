using System;

namespace Iti.Core.DateTime
{
    public class WindowsDateTimeService : IDateTimeService
    {
        public string DefaultTimeZoneId => "Eastern Standard Time";

        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow;
        }

        public DateTimeOffset ToUtc(TimeZone tz, System.DateTime localDateTime)
        {
            // assumes TimeZone.Id is windows TimeZoneInfo.Id string
            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(tz.Id);

            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, tzInfo);
        }

        public DateTimeOffset FromUtc(TimeZone tz, System.DateTime utcDateTime)
        {
            // assumes TimeZone.Id is windows TimeZoneInfo.Id string
            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(tz.Id);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tzInfo);
        }
    }
}