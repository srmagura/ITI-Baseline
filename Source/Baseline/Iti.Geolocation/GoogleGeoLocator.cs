using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Iti.Core.RequestTrace;
using Iti.Geolocation.GoogleResults;
using Iti.Logging;
using Iti.Utilities;
using Iti.ValueObjects;
using Newtonsoft.Json;

namespace Iti.Geolocation
{
    public class GoogleGeoLocator : IGeolocator
    {
        private readonly GoogleGeoLocatorSettings _settings;

        public GoogleGeoLocator(GoogleGeoLocatorSettings settings)
        {
            _settings = settings;
        }

        //
        // Note: GPS 1's digit = 111 km
        //

        public const decimal KilometersPerMile = 1.60934m;
        public const decimal MilesPerKilometer = 1m / KilometersPerMile;

        public GeoLocation Geocode(Address address, IRequestTrace trace = null)
        {
            var requestUrl = "https://maps.googleapis.com/maps/api/geocode/json";

            requestUrl += $"?address={FormatAddressForUrl(address)}&sensor=false&key={_settings.ApiKey}";

            var webClient = new WebClient();
            var responseJson = webClient.DownloadString(requestUrl);

            trace?.WriteTrace(requestUrl, responseJson);

            var result = JsonConvert.DeserializeObject<GoogleGeoCodeResult>(responseJson);

            switch (result.Status)
            {
                case "OK":
                    return HandleGoodResult(result, address);
                default:
                    LogError(address, result.Status);
                    return null;
            }
        }

        private static void LogError(Address address, string message)
        {
            Log.Error($"Geo Location error for [{address}]: {message}");
        }

        public decimal MilesToKilometers(decimal miles)
        {
            return miles * KilometersPerMile;
        }

        public decimal KilometersToMiles(decimal kilometers)
        {
            return kilometers * MilesPerKilometer;
        }

        private static string FormatAddressForUrl(Address info)
        {
            var lines = new List<string> { info.Line1, info.City, info.State, info.Zip }
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Replace(" ", "+"));
            return string.Join(",+", lines);
        }

        private static GeoLocation HandleGoodResult(GoogleGeoCodeResult result, Address address)
        {
            var googleResult = result.Results.FirstOrDefault();
            if (googleResult == null)
            {
                LogError(address, "No result returned");
                return null;
            }

            var geometry = googleResult.Geometry;
            if (geometry == null)
            {
                LogError(address, "No geometry returned");
                return null;
            }

            var location = geometry.Location;
            if (location == null)
            {
                LogError(address, "No location returned");
                return null;
            }

            var isConfident = geometry.LocationType.EqualsIgnoreCase("ROOFTOP");

            return new GeoLocation((decimal)location.Lng, (decimal)location.Lat, isConfident, geometry.LocationType, googleResult.FormattedAddress);
        }
    }
}