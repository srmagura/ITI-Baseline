using System;

namespace ITI.DDD.Logging
{
    public class NullLogWriter : ILogWriter
    {
        public void Write(string level, string userId, string userName, string hostname, string process, string thread, string message,
            Exception exc = null)
        {
            // DO NOTHING
        }
    }
}