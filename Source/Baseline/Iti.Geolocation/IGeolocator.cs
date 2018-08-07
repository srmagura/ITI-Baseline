using Iti.ValueObjects;

namespace Iti.Geolocation
{
    public interface IGeolocator
    {
        GeoLocation Geocode(Address address);
    }
}
