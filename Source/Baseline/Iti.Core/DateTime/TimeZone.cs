using System;
using System.ComponentModel.DataAnnotations;
using Iti.Core.ValueObjects;
using Iti.Utilities;

namespace Iti.Core.DateTime
{
    public class TimeZone : ValueObject<TimeZone>
    {
        protected TimeZone() { }

        public TimeZone(string value)
        {
            Id = value.MaxLength(FieldLengths.TimeZone.Id);
        }

        //

        [MaxLength(FieldLengths.TimeZone.Id)]
        public string Id { get; protected set; }

        //

        public static TimeZone Default => new TimeZone(DateTimeService.DefaultTimeZoneId);

        //

        public override string ToString()
        {
            return Id;
        }

        public override bool HasValue()
        {
            return Id.HasValue();
        }

        public DateTimeOffset FromUtc(DateTimeOffset utcDateTime)
        {
            return DateTimeService.FromUtc(this, utcDateTime.DateTime);
        }

        public DateTimeOffset FromUtc(System.DateTime utcDateTime)
        {
            return DateTimeService.FromUtc(this, utcDateTime);
        }
    }
}