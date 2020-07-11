namespace Iti.Core.UrlShortener
{
    public class NoOpUrlShortener : IUrlShortener
    {
        public string Shorten(string url)
        {
            // no operation (NoOp)
            return url;
        }
    }
}