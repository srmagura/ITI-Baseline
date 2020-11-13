using System;
using System.Collections.Generic;
using System.Text;

namespace RequestTrace
{
    public class DbRequestTrace
    {
        public long Id { get; set; }

        public DateTimeOffset DateBeginUtc { get; set; }
        public DateTimeOffset DateEndUtc { get; set; }

        public string Url { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Exception { get; set; }

        [Obsolete("Serialization use only")]
        protected DbRequestTrace() { }

        public DbRequestTrace(DateTimeOffset dateBeginUtc, DateTimeOffset dateEndUtc,
            string url, string request, string response, Exception exc)
        {
            DateBeginUtc = dateBeginUtc;
            DateEndUtc = dateEndUtc;
            Url = url;
            Request = request;
            Response = response;
            Exception = exc?.ToString();
        }
    }
}
