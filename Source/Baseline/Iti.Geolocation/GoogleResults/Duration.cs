using Newtonsoft.Json;

namespace Iti.Geolocation.GoogleResults
{
    internal class Duration
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public decimal Value { get; set; } // seconds
    }
}