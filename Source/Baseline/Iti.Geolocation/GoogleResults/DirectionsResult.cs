using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Iti.Geolocation.GoogleResults
{
    [DataContract]
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

        [DataMember(Name = "routes")]
        public List<Route> Routes { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }
    }

    [DataContract]
    internal class Distance
    {
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "value")]
        public decimal Value { get; set; } // meters
    }

    [DataContract]
    internal class Duration
    {
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "value")]
        public decimal Value { get; set; } // seconds
    }

    internal class Leg
    {
        [DataMember(Name = "distance")]
        public Distance Distance { get; set; }
        [DataMember(Name = "duration")]
        public Duration Duration { get; set; }
    }

    internal class Route
    {
        [DataMember(Name = "legs")]
        public List<Leg> Legs { get; set; }
        [DataMember(Name = "summary")]
        public string Summary { get; set; }
        [DataMember(Name = "warnings")]
        public List<string> Warnings { get; set; }
    }
}