using ITI.DDD.Domain.ValueObjects;
using System.Collections.Generic;

namespace ITI.Baseline.ValueObjects
{
    public class GeoCoord : ValueObject
    {
        protected GeoCoord() { }

        public GeoCoord(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        //

        public double Latitude { get; protected set; }
        public double Longitude { get; protected set; }

        public override string ToString()
        {
            return $"({Latitude},{Longitude})";
        }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}