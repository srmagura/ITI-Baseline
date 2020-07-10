using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Iti.Core.DateTime;
using Iti.Core.RequestTrace;
using Iti.Geolocation.GoogleResults;
using Iti.Logging;
using Iti.Utilities;
using Iti.ValueObjects;
using Newtonsoft.Json;
using TimeZoneConverter;

namespace Iti.Geolocation
{
    public class GoogleGeoLocator : IGeolocator
    {
        public const string Source = "Google";

        private readonly GoogleGeoLocatorSettings _settings;
        private readonly ILogger _logger;
        private readonly IRequestTrace _trace;

        private GeoLocation InvalidGoogleResult(string status) => new GeoLocation(Source, null, false, false, status, "ERROR", "");

        public GoogleGeoLocator(GoogleGeoLocatorSettings settings, ILogger logger, IRequestTrace trace)
        {
            _settings = settings;
            _logger = logger;
            _trace = trace;
        }

        public GeoLocation Geocode(SimpleAddress simpleAddress)
        {
            var requestUrl = "";
            var responseJson = "";
            var begin = DateTimeService.UtcNow;

            try
            {
                requestUrl = "https://maps.googleapis.com/maps/api/geocode/json";

                requestUrl += $"?address={FormatAddressForUrl(simpleAddress)}&sensor=false&key={_settings.ApiKey}";

                var webClient = new WebClient();
                responseJson = webClient.DownloadString(requestUrl);

                var googleResult = JsonConvert.DeserializeObject<GoogleGeoCodeResult>(responseJson);

                if (googleResult.Status != "OK")
                {
                    LogError(simpleAddress, googleResult.Status);
                    _trace?.WriteTrace(begin, requestUrl, "", responseJson);
                    return InvalidGoogleResult(googleResult.Status);
                }

                _trace?.WriteTrace(begin, requestUrl, "", responseJson);
                return HandleGoodResult(googleResult, simpleAddress);
            }
            catch (Exception exc)
            {
                _logger.Error("Error google geo encoding", exc);
                _trace?.WriteTrace(begin, requestUrl, "", responseJson, exc);
                return InvalidGoogleResult("ERROR");
            }
        }

        public TimeZoneInfo TimezoneFor(GeoLocation location)
        {
            if (location.Latitude == null || location.Longitude == null)
                return null;

            return TimezoneFor(location.Latitude.Value, location.Longitude.Value);
        }

        public TimeZoneInfo TimezoneFor(GeoCoord coord)
        {
            if (coord == null || coord.Latitude == null || coord.Longitude == null)
                return null;

            return TimezoneFor(coord.Latitude.Value, coord.Longitude.Value);
        }

        private TimeZoneInfo TimezoneFor(double latitude, double longitude)
        {
            var requestUrl = "";
            var responseJson = "";
            var begin = DateTimeService.UtcNow;

            try
            {
                requestUrl = "https://maps.googleapis.com/maps/api/timezone/json";

                var seconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                requestUrl += $"?location={latitude},{longitude}&timestamp={seconds}&key={_settings.ApiKey}";

                var webClient = new WebClient();
                responseJson = webClient.DownloadString(requestUrl);

                var googleResult = JsonConvert.DeserializeObject<TimeZoneLookupResult>(responseJson);

                if (googleResult.Status != "OK")
                {
                    LogError(latitude, longitude, googleResult.Status);
                    _trace?.WriteTrace(begin, requestUrl, "", responseJson);
                    return null;
                }

                _trace?.WriteTrace(begin, requestUrl, "", responseJson);
                return ConvertTimeZone(googleResult);
            }
            catch (Exception exc)
            {
                _logger.Error("Error google geo encoding", exc);
                _trace?.WriteTrace(begin, requestUrl, "", responseJson, exc);
                return null;
            }
        }

        public double GetDrivingDistance(SimpleAddress from, SimpleAddress to)
        {
            // USES: GOOGLE DIRECTIONS API

            var requestUrl = "";
            var responseJson = "";
            var begin = DateTimeService.UtcNow;

            try
            {
                requestUrl = "https://maps.googleapis.com/maps/api/directions/json";

                requestUrl += $"?origin={FormatAddressForUrl(from)}&destination={FormatAddressForUrl(to)}&key={_settings.ApiKey}";

                var webClient = new WebClient();
                responseJson = webClient.DownloadString(requestUrl);

                var googleResult = JsonConvert.DeserializeObject<GoogleDirectionsResult>(responseJson);

                if (googleResult.Status != "OK")
                {
                    LogError(from, to, googleResult.Status);
                    _trace?.WriteTrace(begin, requestUrl, "", responseJson);
                    return 0;
                }

                _trace?.WriteTrace(begin, requestUrl, "", responseJson);
                return googleResult.DistanceInMiles;
            }
            catch (Exception exc)
            {
                _logger.Error("Error google distance", exc);
                _trace?.WriteTrace(begin, requestUrl, "", responseJson, exc);
                return 0;
            }
        }

        private TimeZoneInfo ConvertTimeZone(TimeZoneLookupResult result)
        {
            var winId = TZConvert.IanaToWindows(result.TimeZoneId);
            return DateTimeService.Lookup(winId);
        }

        private void LogError(SimpleAddress simpleAddress, string message)
        {
            _logger.Error($"Geo Location error for [{simpleAddress}]: {message}");
        }

        private void LogError(SimpleAddress from, SimpleAddress to, string message)
        {
            _logger.Error($"Geo Location error for [{from}] to [{to}]: {message}");
        }

        private void LogError(double? lat, double? lng, string message)
        {
            _logger.Error($"TimeZone lookup error for [{lat},{lng}]: {message}");
        }

        public string FormatAddressForUrl(SimpleAddress info)
        {
            var zip = info.Zip?.Trim() ?? "";
            if (zip.Length > 5)
                zip = zip.Substring(0, 5);

            var lines = new List<string> { info.Line1, info.City, info.State, zip }
                .Where(s => !string.IsNullOrEmpty(s))
                ;
            var joined = string.Join(", ", lines);

            var urlS = HttpUtility.UrlEncode(joined);
            return urlS;
        }

        private GeoLocation HandleGoodResult(GoogleGeoCodeResult result, SimpleAddress simpleAddress)
        {
            var googleResult = result.Results.FirstOrDefault();
            if (googleResult == null)
            {
                LogError(simpleAddress, "No result returned");
                return InvalidGoogleResult("ERROR:NO_RESULT");
            }

            var geometry = googleResult.Geometry;
            if (geometry == null)
            {
                LogError(simpleAddress, "No geometry returned");
                return InvalidGoogleResult("ERROR:NO_GEOMETRY");
            }

            var location = geometry.Location;
            if (location == null)
            {
                LogError(simpleAddress, "No location returned");
                return InvalidGoogleResult("ERROR:NO_LOCATION");
            }

            var isConfident = geometry.LocationType.EqualsIgnoreCase("ROOFTOP");

            var geoCoord = new GeoCoord(location.Lat, location.Lng);
            return new GeoLocation(Source, geoCoord, true, isConfident,
                result.Status, geometry.LocationType, googleResult.FormattedAddress);
        }
    }
}