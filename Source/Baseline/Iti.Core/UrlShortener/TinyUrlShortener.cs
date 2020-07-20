using System;
using System.Net;
using Iti.Baseline.Logging;

namespace Iti.Baseline.Core.UrlShortener
{
    public class TinyUrlShortener : IUrlShortener
    {
        private readonly ILogger _logger;

        public TinyUrlShortener(ILogger logger)
        {
            _logger = logger;
        }

        private const string BaseUrl = @"http://tinyurl.com/api-create.php?url=";

        public string Shorten(string urlToShorten)
        {
            try
            {
                var tinyUrl = $"{BaseUrl}{urlToShorten}";

                string result;
                using (var cli = new WebClient())
                {
                    result = cli.DownloadString(tinyUrl);
                }

                return result;
            }
            catch (Exception exc)
            {
                _logger.Error($"Could not shorten url '{urlToShorten}' using TinyUrl", exc);
                return urlToShorten;
            }
        }
    }
}