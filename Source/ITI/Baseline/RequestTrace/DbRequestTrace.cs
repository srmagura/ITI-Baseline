using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITI.Baseline.RequestTrace
{
    // Make sure to add an index like this:
    //
    //     modelBuilder.Entity<DbRequestTrace>()
    //         .HasIndex(t => new { t.Service, t.Direction })
    //     });
    //
    public class DbRequestTrace
    {
        public long Id { get; set; }

        [MaxLength(32)]
        public string Service { get; set; }

        [MaxLength(16)]
        public string Direction { get; set; }

        public DateTimeOffset DateBeginUtc { get; set; }
        public DateTimeOffset DateEndUtc { get; set; }

        public string Url { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string? Exception { get; set; }

        // For persistence use
        public DbRequestTrace(
            string service,
            string direction,
            DateTimeOffset dateBeginUtc,
            DateTimeOffset dateEndUtc,
            string url,
            string request,
            string response,
            string? exception
        )
        {
            Service = service;
            Direction = direction;
            DateBeginUtc = dateBeginUtc;
            DateEndUtc = dateEndUtc;
            Url = url;
            Request = request;
            Response = response;
            Exception = exception;
        }

        public DbRequestTrace(
            string service,
            RequestTraceDirection direction,
            DateTimeOffset dateBeginUtc,
            DateTimeOffset dateEndUtc,
            string url,
            string request,
            string response,
            Exception? exception
        ) : this(
            service,
            direction.ToString(),
            dateBeginUtc,
            dateEndUtc,
            url,
            request,
            response,
            exception?.ToString()
        )
        {
        }
    }
}
