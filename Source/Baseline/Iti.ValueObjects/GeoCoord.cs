using Iti.Core.ValueObjects;

namespace Iti.ValueObjects
{
    public class GeoCoord : ValueObject<GeoCoord>
    {
        protected GeoCoord() { }

        public GeoCoord(double? latitude, double? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        //

        public double? Latitude { get; protected set; }
        public double? Longitude { get; protected set; }

        public override bool HasValue()
        {
            return Longitude != null && Latitude != null;
        }
    }
}