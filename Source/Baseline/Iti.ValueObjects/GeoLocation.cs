using System.ComponentModel.DataAnnotations;
using Iti.Core.ValueObjects;
using Iti.Utilities;

namespace Iti.ValueObjects
{
    public class GeoLocation : ValueObject<GeoLocation>
    {
        protected GeoLocation() { }

        public GeoLocation(string source, decimal longitude, decimal latitude, bool isValid, bool isConfident, string status, string locationType, string formattedAddress)
        {
            Source = source.MaxLength(16);
            Longitude = longitude;
            Latitude = latitude;
            IsValid = isValid;
            IsConfident = isConfident;
            Status = status.MaxLength(64);
            LocationType = locationType.MaxLength(64);
            FormattedAddress = formattedAddress.MaxLength(128);
        }

        [MaxLength(16)]
        public string Source { get; protected set; }

        public decimal? Longitude { get; protected set; }
        public decimal? Latitude { get; protected set; }

        public bool IsValid { get; protected set; }
        public bool IsConfident { get; protected set; }

        [MaxLength(64)]
        public string Status { get; protected set; }

        [MaxLength(64)]
        public string LocationType { get; protected set; }

        [MaxLength(128)]
        public string FormattedAddress { get; protected set; }

        public override bool HasValue()
        {
            return Longitude != null && Latitude != null;
        }

        public override string ToString()
        {
            return $"(Lng:{Longitude},Lat:{Latitude})";
        }
    }
}