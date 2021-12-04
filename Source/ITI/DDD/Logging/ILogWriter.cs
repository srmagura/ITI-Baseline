namespace ITI.DDD.Logging;

public interface ILogWriter
{
    void Write(
        string level,
        string? userId,
        string? userName,
        string hostname,
        string process,
        string message,
        Exception? exc
    );
}
