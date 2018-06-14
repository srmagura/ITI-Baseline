using System;

namespace Iti.Core.DateTime
{
    public interface IDateTimeService
    {
        string DefaultTimeZoneId { get; }

        DateTimeOffset UtcNow();
        DateTimeOffset ToUtc(TimeZone tz, System.DateTime localDateTime);
        DateTimeOffset FromUtc(TimeZone tz, System.DateTime utcDateTime);
    }
}