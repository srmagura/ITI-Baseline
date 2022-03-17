using Dapper;
using Microsoft.Data.SqlClient;

namespace ITI.DDD.Logging;

public class DbLogWriter : ILogWriter
{
    private readonly IDbLoggerSettings _settings;

    public DbLogWriter(IDbLoggerSettings settings)
    {
        _settings = settings;
    }

    public void Write(
        string level,
        string? userId,
        string? userName,
        string hostname,
        string process,
        string message,
        Exception? exception
    )
    {
        try
        {
            using var conn = new SqlConnection(_settings.LogConnectionString);
            var sql = $@"INSERT INTO {_settings.LogTableName} 
                                    (WhenUtc,Level,UserId,UserName,Hostname,Process,Message,Exception) 
                                    VALUES (@WhenUtc,@Level,@UserId,@UserName,@Hostname,@Process,@Message,@Exception)";

            var logEntry = new LogEntry(level, userId, userName, hostname, process, message, exception);

            conn.Execute(sql, logEntry);
        }
        catch (Exception logException)
        {
            // eat it... nothing we can do, and don't want to fail anything because of logging issues
            // ... so we send the message screaming into the void...

            // last ditch attempt...
            Console.WriteLine($"DBLOGWRITER INTERNAL EXCEPTION: {logException}");
        }
    }
}
