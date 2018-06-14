using System;

namespace Iti.Logging
{
    public class NullWriter : ILogWriter
    {
        public void Write(string level, string userId, string userName, string hostname, string process, string thread, string message,
            Exception exc = null)
        {
            // DO NOTHING
        }
    }
}