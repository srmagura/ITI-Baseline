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
        private readonly IRequestTrace _trace;

        private GeoLocation InvalidGoogleResult(string status) => new GeoLocation(Source, 0, 0, false, false, status, "ERROR", "");

        public GoogleGeoLocator(GoogleGeoLocatorSettings settings, IRequestTrace trace)
        {
            _settings = settings;
            _trace = trace;
        }

        public GeoLocation Geocode(Address address)
        {
            var requestUrl = "";
            var responseJson = "";
            var begin = DateTimeService.UtcNow;

            try
            {
                requestUrl = "https://maps.googleapis.com/maps/api/geocode/json";

                requestUrl += $"?address={FormatAddressForUrl(address)}&sensor=false&key={_settings.ApiKey}";

                var webClient = new WebClient();
                responseJson = webClient.DownloadString(requestUrl);

                var googleResult = JsonConvert.DeserializeObject<GoogleGeoCodeResult>(responseJson);

                if (googleResult.Status != "OK")
                {
                    LogError(address, googleResult.Status);
                    _trace?.WriteTrace(begin, requestUrl, "", responseJson);
                    return InvalidGoogleResult(googleResult.Status);
                }

                _trace?.WriteTrace(begin, requestUrl, "", responseJson);
                return HandleGoodResult(googleResult, address);
            }
            catch (Exception exc)
            {
                Log.Error("Error google geo encoding", exc);
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

        public TimeZoneInfo TimezoneFor(decimal latitude, decimal longitude)
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
                Log.Error("Error google geo encoding", exc);
                _trace?.WriteTrace(begin, requestUrl, "", responseJson, exc);
                return null;
            }
        }

        public double GetDrivingDistance(Address from, Address to)
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
                Log.Error("Error google distance", exc);
                _trace?.WriteTrace(begin, requestUrl, "", responseJson, exc);
                return 0;
            }
        }

        private TimeZoneInfo ConvertTimeZone(TimeZoneLookupResult result)
        {
            var winId = TZConvert.IanaToWindows(result.TimeZoneId);
            return DateTimeService.Lookup(winId);
        }

        private void LogError(Address address, string message)
        {
            Log.Error($"Geo Location error for [{address}]: {message}");
        }

        private void LogError(Address from, Address to, string message)
        {
            Log.Error($"Geo Location error for [{from}] to [{to}]: {message}");
        }

        private void LogError(decimal lat, decimal lng, string message)
        {
            Log.Error($"TimeZone lookup error for [{lat},{lng}]: {message}");
        }

        public string FormatAddressForUrl(Address info)
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