using System;

namespace Iti.Baseline.Core.DateTime
{
    public interface IDateTimeService
    {
        string DefaultTimeZoneId { get; }
        TimeZoneInfo Lookup(string timeZoneId);

        DateTimeOffset UtcNow();
        DateTimeOffset ToUtc(string timeZoneId, DateTimeOffset localDateTime);
        DateTimeOffset FromUtc(string timeZoneId, DateTimeOffset utcDateTime);
    }
}