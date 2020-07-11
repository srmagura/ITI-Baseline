﻿using Newtonsoft.Json;

namespace Iti.Baseline.Geolocation.GoogleResults
{
    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}