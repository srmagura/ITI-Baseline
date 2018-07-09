using System;
using System.Diagnostics;
using System.Threading;
using Iti.Auth;
using Iti.Inversion;

namespace Iti.Logging
{
    public static class Log
    {
        public static bool DebugEnabled = false;

        public static void Info(string message)
        {
            Write("Info", message);
        }

        public static void Debug(string message)
        {
            if (!DebugEnabled)
                return;

            Write("Debug", message);
        }

        public static void Warning(string message, Exception exc = null)
        {
            Write("Warning", message, exc);
        }

        public static void Error(string message, Exception exc = null)
        {
            Write("ERROR", message, exc);
        }

        //

        private static void Write(string level, string message, Exception exc = null)
        {
            ResolveHandlers();
            if (_writer == null)
                return;

            string userId = null;
            string userName = null;
            if (_auth != null)
            {
                userId = _auth?.UserId;
                userName = _auth?.UserName;
            }

            var hostname = Environment.MachineName;
            var process = Process.GetCurrentProcess().ProcessName;
            var thread = Thread.CurrentThread.Name;

            _writer?.Write(level, userId, userName, hostname, process, thread, message, exc);
        }

        private static ILogWriter _writer;
        private static IAuthContext _auth;
        private static readonly object LockObject = new object();

        public static void RefreshHandlers()
        {
            _auth = null;
            _writer = null;
        }

        private static void ResolveHandlers()
        {
            if (_auth != null && _writer != null)
                return;

            lock (LockObject)
            {
                if (_auth == null)
                {
                    IOC.TryResolve(out _auth);
                }

                if (_writer == null)
                {
                    IOC.TryResolve(out _writer);
                }
            }
        }
    }
}
