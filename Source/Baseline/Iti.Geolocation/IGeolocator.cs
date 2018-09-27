using System;
using Iti.ValueObjects;

namespace Iti.Geolocation
{
    public interface IGeolocator
    {
        GeoLocation Geocode(Address address);
        TimeZoneInfo TimezoneFor(GeoLocation location);
        TimeZoneInfo TimezoneFor(decimal latitude, decimal longitude);

        double GetDrivingDistance(Address from, Address to);
    }
}
