namespace ITI.DDD.Logging
{
    public interface ILogger
    {
        void Debug(string message, Exception? exc = null);
        void Info(string message, Exception? exc = null);
        void Warning(string message, Exception? exc = null);
        void Error(string message, Exception? exc = null);
    }
}