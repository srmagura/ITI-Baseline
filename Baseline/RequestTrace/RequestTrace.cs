using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ITI.Baseline.RequestTracing;

/// <summary>
/// Make sure to call OnModelCreating to create an import database index!
/// </summary>
public class RequestTrace
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

    /// <summary>
    /// For use by Entity Framework.
    /// </summary>
    protected RequestTrace(
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

    public RequestTrace(
        string service,
        RequestTraceDirection direction,
        DateTimeOffset dateBeginUtc,
        DateTimeOffset dateEndUtc,
        string url,
        string request,
        string response,
        Exception? exception
#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete
    {
    }

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<RequestTrace>()
            .HasIndex(t => new { t.Service, t.Direction });
    }
}
