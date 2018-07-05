using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Iti.Core.RequestTrace;
using Iti.ValueObjects;

namespace Iti.Geolocation
{
    public interface IGeolocator
    {
        GeoLocation Geocode(Address address, IRequestTrace trace = null);
    }
}
