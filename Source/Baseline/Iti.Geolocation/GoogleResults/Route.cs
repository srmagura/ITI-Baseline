using System.Collections.Generic;
using Newtonsoft.Json;

namespace Iti.Geolocation.GoogleResults
{
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