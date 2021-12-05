using ITI.DDD.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ITI.DDD.Logging;

/// <summary>
/// Make sure to call OnModelCreating to create an important index!
/// </summary>
public class LogEntry
{
    /// <summary>
    /// For use by Entity Framework.
    /// </summary>
    protected LogEntry(
        string level,
        string? userId,
        string? userName,
        string hostname,
        string process,
        string message,
        string? exception
    )
    {
        Level = level.MaxLength(16);
        UserId = userId?.MaxLength(128);
        UserName = userName?.MaxLength(128);
        Hostname = hostname.MaxLength(128);
        Process = process.MaxLength(128);
        Message = message;
        Exception = exception;
    }

    public LogEntry(
        string level,
        string? userId,
        string? userName,
        string hostname,
        string process,
        string message,
        Exception? exception
    ) : this(
        level,
        userId,
        userName,
        hostname,
        process,
        message,
        exception?.ToString()
    )
    { }

    public long Id { get; protected set; }
    public DateTimeOffset WhenUtc { get; protected set; } = DateTimeOffset.UtcNow;

    [MaxLength(16)]
    public string? Level { get; protected set; }

    [MaxLength(128)]
    public string? UserId { get; protected set; }

    [MaxLength(128)]
    public string? UserName { get; protected set; }

    [MaxLength(128)]
    public string? Hostname { get; protected set; }

    [MaxLength(128)]
    public string? Process { get; protected set; }

    public string? Message { get; protected set; }
    public string? Exception { get; protected set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<LogEntry>().HasIndex(p => p.WhenUtc);
    }
}
