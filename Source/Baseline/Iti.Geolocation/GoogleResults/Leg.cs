using Newtonsoft.Json;

namespace Iti.Baseline.Geolocation.GoogleResults
{
    internal class Leg
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }
        [JsonProperty("duration")]
        public Duration Duration { get; set; }
    }
}