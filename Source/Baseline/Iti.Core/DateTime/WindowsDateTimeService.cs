using System;

namespace Iti.Core.DateTime
{
    public class WindowsDateTimeService : IDateTimeService
    {
        public string DefaultTimeZoneId => "Eastern Standard Time";

        public TimeZoneInfo Lookup(string timeZoneId)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow;
        }

        public DateTimeOffset ToUtc(string timeZoneId, DateTimeOffset localDateTime)
        {
            // assumes timeZoneId is windows TimeZoneInfo.Id string
            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            return TimeZoneInfo.ConvertTimeToUtc(localDateTime.DateTime, tzInfo);
        }

        public DateTimeOffset FromUtc(string timeZoneId, DateTimeOffset utcDateTime)
        {
            // assumes timeZoneId is windows TimeZoneInfo.Id string
            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.DateTime, tzInfo);
        }
    }
}