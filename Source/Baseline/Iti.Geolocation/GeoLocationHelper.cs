using System;
using Iti.ValueObjects;

namespace Iti.Geolocation
{
    public static class GeoLocationHelper
    {
        //
        // Note: GPS 1's digit = 111 km
        //
        
        public const double KilometersPerMile = 1.60934;
        public const double MilesPerKilometer = 1.0 / KilometersPerMile;

        public static double MilesToKilometers(double miles)
        {
            return miles * KilometersPerMile;
        }

        public static double KilometersToMiles(double kilometers)
        {
            return kilometers * MilesPerKilometer;
        }

        public static void MinMaxRadiusMiles(GeoLocation geo, double radiusMiles, 
            out double minLat, out double minLng, 
            out double maxLat, out double maxLng)
        {
            var radiusKm = MilesToKilometers(radiusMiles);
            MinMaxRadiusKilometers(geo, radiusKm, out minLat, out minLng, out maxLat, out maxLng);
        }

        public static void MinMaxRadiusKilometers(GeoLocation geo, double radiusKilometers,
            out double minLat, out double minLng,
            out double maxLat, out double maxLng)
        {
            minLat = minLng = maxLat = maxLng = 0;

            if (geo.Latitude == null || geo.Longitude == null)
                return;

            var latDiff = radiusKilometers / 111.0;

            minLat = geo.Latitude.Value - latDiff;
            maxLat = geo.Latitude.Value + latDiff;
            minLng = geo.Longitude.Value - latDiff;
            maxLng = geo.Longitude.Value + latDiff;
        }

        private static double ToRadian(double val) { return val * (Math.PI / 180.0); }
        private static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

        public static double KilometerDistanceBetween(GeoLocation geo1, GeoLocation geo2)
        {
            const double earthRadiusInKilometers = 6367.0;

            var lat1 = geo1.Latitude;
            var lng1 = geo1.Longitude;

            var lat2 = geo2.Latitude;
            var lng2 = geo2.Longitude;

            return (double)(earthRadiusInKilometers * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian((double)lat1, (double)lat2)) / 2.0), 2.0) + Math.Cos(ToRadian((double)lat1)) * Math.Cos(ToRadian((double)lat2)) * Math.Pow(Math.Sin((DiffRadian((double)lng1, (double)lng2)) / 2.0), 2.0))))));
        }

        public static double MilesDistanceBetween(GeoLocation geo1, GeoLocation geo2)
        {
            var kmDist = KilometerDistanceBetween(geo1, geo2);
            return KilometersToMiles(kmDist);
        }
    }
}