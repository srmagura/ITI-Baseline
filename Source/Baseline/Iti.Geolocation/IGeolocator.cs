using System;
using Iti.Baseline.ValueObjects;

namespace Iti.Baseline.Geolocation
{
    public interface IGeolocator
    {
        GeoLocation Geocode(SimpleAddress simpleAddress);
        TimeZoneInfo TimezoneFor(GeoLocation location);
        TimeZoneInfo TimezoneFor(GeoCoord coord);

        double GetDrivingDistance(SimpleAddress from, SimpleAddress to);
    }
}
