using System;

namespace ITI.DDD.Logging
{
    public interface ILogger
    {
        void Info(string message, Exception exc = null);
        void Debug(string message, Exception exc = null);
        void Warning(string message, Exception exc = null);
        void Error(string message, Exception exc = null);
    }
}