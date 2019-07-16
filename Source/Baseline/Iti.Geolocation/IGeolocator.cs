using System;
using Iti.ValueObjects;

namespace Iti.Geolocation
{
    public interface IGeolocator
    {
        GeoLocation Geocode(SimpleAddress simpleAddress);
        TimeZoneInfo TimezoneFor(GeoLocation location);
        TimeZoneInfo TimezoneFor(decimal latitude, decimal longitude);

        double GetDrivingDistance(SimpleAddress from, SimpleAddress to);
    }
}
