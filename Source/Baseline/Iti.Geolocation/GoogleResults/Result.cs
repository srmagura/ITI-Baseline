using System.Collections.Generic;
using Newtonsoft.Json;

namespace Iti.Baseline.Geolocation.GoogleResults
{
    internal class Result
    {
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("partial_match")]
        public bool PartialMatch { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }
}