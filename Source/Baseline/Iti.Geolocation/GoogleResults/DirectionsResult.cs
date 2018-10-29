using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Iti.Geolocation.GoogleResults
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

    internal class Distance
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public decimal Value { get; set; } // meters
    }

    internal class Duration
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public decimal Value { get; set; } // seconds
    }

    internal class Leg
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }
        [JsonProperty("duration")]
        public Duration Duration { get; set; }
    }

    internal class Route
    {
        [JsonProperty("legs")]
        public List<Leg> Legs { get; set; }
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("warnings")]
        public List<string> Warnings { get; set; }
    }
}