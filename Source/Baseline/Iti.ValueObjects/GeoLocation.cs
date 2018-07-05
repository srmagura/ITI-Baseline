using System.ComponentModel.DataAnnotations;
using Iti.Core.ValueObjects;
using Iti.Utilities;

namespace Iti.ValueObjects
{
    public class GeoLocation : ValueObject<GeoLocation>
    {
        protected GeoLocation() { }

        public GeoLocation(decimal longitude, decimal latitude, bool isConfident, string locationType, string formattedAddress)
        {
            Longitude = longitude;
            Latitude = latitude;
            IsConfident = isConfident;
            LocationType = locationType.MaxLength(64);
            FormattedAddress = formattedAddress.MaxLength(128);
        }

        public decimal? Longitude { get; protected set; }
        public decimal? Latitude { get; protected set; }

        public bool IsConfident { get; protected set; }

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