using Newtonsoft.Json;

namespace Iti.Geolocation.GoogleResults
{
    internal class Leg
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }
        [JsonProperty("duration")]
        public Duration Duration { get; set; }
    }
}