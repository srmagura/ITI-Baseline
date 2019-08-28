using System;
using Iti.ValueObjects;

namespace Iti.Geolocation
{
    public interface IGeolocator
    {
        GeoLocation Geocode(SimpleAddress simpleAddress);
        TimeZoneInfo TimezoneFor(GeoLocation location);
        TimeZoneInfo TimezoneFor(GeoCoord coord);

        double GetDrivingDistance(SimpleAddress from, SimpleAddress to);
    }
}
