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
        public const string Source = "Google";

        private readonly GoogleGeoLocatorSettings _settings;

        private GeoLocation InvalidGoogleResult(string status) => new GeoLocation(Source, 0, 0, false, false, status, "ERROR", "");

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

            var googleResult = JsonConvert.DeserializeObject<GoogleGeoCodeResult>(responseJson);

            if (googleResult.Status != "OK")
            {
                LogError(address, googleResult.Status);
                return InvalidGoogleResult(googleResult.Status);
            }

            return HandleGoodResult(googleResult, address);
        }

        private void LogError(Address address, string message)
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

        private string FormatAddressForUrl(Address info)
        {
            var lines = new List<string> { info.Line1, info.City, info.State, info.Zip }
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Replace(" ", "+"));
            return string.Join(",+", lines);
        }

        private GeoLocation HandleGoodResult(GoogleGeoCodeResult result, Address address)
        {
            var googleResult = result.Results.FirstOrDefault();
            if (googleResult == null)
            {
                LogError(address, "No result returned");
                return InvalidGoogleResult("ERROR:NO_RESULT");
            }

            var geometry = googleResult.Geometry;
            if (geometry == null)
            {
                LogError(address, "No geometry returned");
                return InvalidGoogleResult("ERROR:NO_GEOMETRY");
            }

            var location = geometry.Location;
            if (location == null)
            {
                LogError(address, "No location returned");
                return InvalidGoogleResult("ERROR:NO_LOCATION");
            }

            var isConfident = geometry.LocationType.EqualsIgnoreCase("ROOFTOP");

            return new GeoLocation(Source, (decimal)location.Lng, (decimal)location.Lat, true, isConfident, result.Status, geometry.LocationType, googleResult.FormattedAddress);
        }
    }
}