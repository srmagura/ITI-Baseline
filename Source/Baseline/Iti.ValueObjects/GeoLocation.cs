using System.ComponentModel.DataAnnotations;
using Iti.Baseline.Core.ValueObjects;
using Iti.Baseline.Utilities;
using Newtonsoft.Json;

namespace Iti.Baseline.ValueObjects
{
    public class GeoLocation : ValueObject<GeoLocation>
    {
        protected GeoLocation() { }

        [JsonConstructor]
        protected GeoLocation(string source, double longitude, double latitude, bool isValid, bool isConfident, string status, string locationType, string formattedAddress)
            : this(source, new GeoCoord(latitude, longitude), isValid, isConfident, status, locationType, formattedAddress)
        {
        }

        public GeoLocation(string source, GeoCoord geoCoord, bool isValid, bool isConfident, string status, string locationType, string formattedAddress)
        {
            Source = source.MaxLength(16);

            Latitude = geoCoord?.Latitude;
            Longitude = geoCoord?.Longitude;

            IsValid = isValid;
            IsConfident = isConfident;
            Status = status.MaxLength(64);
            LocationType = locationType.MaxLength(64);
            FormattedAddress = formattedAddress.MaxLength(128);
        }

        [MaxLength(16)]
        public string Source { get; protected set; }

        public double? Longitude { get; protected set; }
        public double? Latitude { get; protected set; }

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