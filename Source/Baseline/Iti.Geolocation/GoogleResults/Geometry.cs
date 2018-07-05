using Newtonsoft.Json;

namespace Iti.Geolocation.GoogleResults
{
    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }
    }
}