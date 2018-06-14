using System;

namespace Iti.Logging
{
    public interface ILogWriter
    {
        void Write(
            string level, 
            string userId, string userName, 
            string hostname, string process, string thread, 
            string message,
            Exception exc = null);
    }
}