using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITI.DDD.Core.Util;
using ITI.DDD.Domain.ValueObjects;

namespace Iti.Baseline.ValueObjects
{
    public class GeoLocation : ValueObject
    {
        protected GeoLocation() { }

        public GeoLocation(
            string source,
            GeoCoord? geoCoord,
            bool isValid,
            bool isConfident,
            string status,
            string locationType,
            string formattedAddress
        )
        {
            Source = source.MaxLength(16);
            GeoCoord = geoCoord;
            IsValid = isValid;
            IsConfident = isConfident;
            Status = status.MaxLength(64);
            LocationType = locationType.MaxLength(64);
            FormattedAddress = formattedAddress.MaxLength(128);
        }

        [MaxLength(FieldLengths.GeoLocation.Source)]
        public string Source { get; protected set; }

        public GeoCoord? GeoCoord { get; protected set; }
        public bool IsValid { get; protected set; }
        public bool IsConfident { get; protected set; }

        [MaxLength(FieldLengths.GeoLocation.Status)]
        public string Status { get; protected set; }

        [MaxLength(FieldLengths.GeoLocation.LocationType)]
        public string LocationType { get; protected set; }

        [MaxLength(FieldLengths.GeoLocation.FormattedAddress)]
        public string FormattedAddress { get; protected set; }

        public override string ToString()
        {
            return GeoCoord?.ToString() ?? "Invalid GeoLocation";
        }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Source;
            yield return GeoCoord;
            yield return IsValid;
            yield return IsConfident;
            yield return Status;
            yield return LocationType;
            yield return FormattedAddress;
        }
    }
}