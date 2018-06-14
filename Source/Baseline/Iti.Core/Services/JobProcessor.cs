using System;

namespace Iti.Core.Services
{
    public abstract class JobProcessor
    {
        public abstract void Run();

        protected void Output(string msg, Exception exc = null)
        {
            // For job processors, we write output to console log so that it shows up
            // in the Azure web job, if used.   We could just inject a different log implementation,
            // but then we would not be able to log errors/warnings, etc. to the db log... or we'd have
            // to write a "split" log implementation (probably the better option, but this is simpler for now).
            Console.WriteLine($">>> {msg}");

            if (exc != null)
                Console.WriteLine(exc);
        }
    }
}