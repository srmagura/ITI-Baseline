using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Iti.Baseline.Geolocation.GoogleResults
{
    internal class GoogleDirectionsResult
    {
        public double DistanceInMiles
        {
            get
            {
                var route = Routes.FirstOrDefault();
                var distance = route?.Legs.Sum(l => l.Distance.Value / 1609.34m) ?? 0; // convert to miles

                return (double)distance;
            }
        }

        [JsonProperty("routes")]
        public List<Route> Routes { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}