using System;

namespace Iti.Logging
{
    public class ConsoleLogWriter : ILogWriter
    {
        public void Write(string level, string userId, string userName, string hostname, string process, string thread, string message,
            Exception exc = null)
        {
            Console.WriteLine($"{DateTime.Now}: {level}: {userId}|{userName}|{hostname}|{process}|{thread}: {message}");
            if(exc != null)
                Console.WriteLine($"EXCEPTION: {exc}");
        }
    }
}