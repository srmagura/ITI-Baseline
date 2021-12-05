namespace ITI.DDD.Logging;

public interface ILogger
{
    void Debug(string message, Exception? exception = null);
    void Info(string message, Exception? exception = null);
    void Warning(string message, Exception? exception = null);
    void Error(string message, Exception? exception = null);
}
