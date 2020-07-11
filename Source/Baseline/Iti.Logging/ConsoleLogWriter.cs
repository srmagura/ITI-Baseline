using System;

namespace Iti.Baseline.Logging
{
    public class ConsoleLogWriter : ILogWriter
    {
        public static void ClearErrors()
        {
            HasErrors = false;
        }

        public static bool HasErrors { get; protected set; } = false;

        public void Write(string level, string userId, string userName, string hostname, string process, string thread, string message,
            Exception exc = null)
        {
            if (exc != null || level.ToUpper() == "ERROR")
                HasErrors = true;

            Console.WriteLine($"{DateTime.Now}: {level}: {userId}|{userName}|{hostname}|{process}|{thread}: {message}");
            if(exc != null)
                Console.WriteLine($"EXCEPTION: {exc}");
        }
    }
}