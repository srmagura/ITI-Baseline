using System;
using System.Net;
using Iti.Logging;

namespace Iti.Core.UrlShortener
{
    public class TinyUrlShortener : IUrlShortener
    {
        private const string BaseUrl = @"http://tinyurl.com/api-create.php?url=";

        public string Shorten(string urlToShorten)
        {
            try
            {
                var tinyUrl = $"{BaseUrl}{urlToShorten}";

                var cli = new WebClient();
                var result = cli.DownloadString(tinyUrl);

                return result;
            }
            catch (Exception exc)
            {
                Log.Error($"Could not shorten url '{urlToShorten}' using TinyUrl", exc);
                return urlToShorten;
            }
        }
    }
}