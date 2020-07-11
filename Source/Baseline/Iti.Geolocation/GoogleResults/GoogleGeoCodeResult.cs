using System.Collections.Generic;

namespace Iti.Baseline.Geolocation.GoogleResults
{
    internal class GoogleGeoCodeResult
    {
        public List<Result> Results { get; set; }
        public string Status { get; set; }
    }
}