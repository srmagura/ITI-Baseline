using System.Collections.Generic;

namespace Iti.Geolocation.GoogleResults
{
    internal class GoogleGeoCodeResult
    {
        public List<Result> Results { get; set; }
        public string Status { get; set; }
    }

    internal class TimeZoneLookupResult
    {
        public long DstOffset { get; set; }
        public long RawOffset { get; set; }
        public string Status { get; set; }
        public string TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }
    }
}