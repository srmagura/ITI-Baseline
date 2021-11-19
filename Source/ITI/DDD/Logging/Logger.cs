using System;
using System.Diagnostics;
using System.Threading;
using ITI.DDD.Auth;

namespace ITI.DDD.Logging
{
    public class Logger : ILogger
    {
        public Logger(ILogWriter writer, IAuthContext auth)
        {
            _writer = writer;
            _auth = auth;
        }

        private readonly ILogWriter _writer;
        private readonly IAuthContext _auth;

        public void Info(string message, Exception? exc = null)
        {
            Write("Info", message, exc);
        }

        public void Debug(string message, Exception? exc = null)
        {
#if DEBUG
            Write("Debug", message, exc);
#endif
        }

        public void Warning(string message, Exception? exc = null)
        {
            Write("Warning", message, exc);
        }

        public void Error(string message, Exception? exc = null)
        {
            Write("ERROR", message, exc);
        }

        //

        private void Write(string level, string message, Exception? exc = null)
        {
            if (_writer == null)
                return;

            string? userId = null;
            string? userName = null;
            if (_auth != null)
            {
                userId = _auth.UserIdString;
                userName = _auth.UserName;
            }

            var hostname = Environment.MachineName;
            var process = Process.GetCurrentProcess().ProcessName;
            var thread = Thread.CurrentThread.Name;

            _writer?.Write(level, userId, userName, hostname, process, thread, message, exc);
        }
    }
}
