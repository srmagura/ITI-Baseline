using System.Diagnostics;
using ITI.DDD.Auth;

namespace ITI.DDD.Logging;

public class Logger : ILogger
{
    private readonly ILogWriter _writer;
    private readonly IAuthContext _auth;

    public Logger(ILogWriter writer, IAuthContext auth)
    {
        _writer = writer;
        _auth = auth;
    }

    public void Debug(string message, Exception? exception = null)
    {
#if DEBUG
        Write("Debug", message, exception);
#endif
    }

    public void Info(string message, Exception? exception = null)
    {
        Write("Info", message, exception);
    }

    public void Warning(string message, Exception? exception = null)
    {
        Write("Warning", message, exception);
    }

    public void Error(string message, Exception? exception = null)
    {
        Write("ERROR", message, exception);
    }

    private void Write(string level, string message, Exception? exception)
    {
        if (_writer == null)
            return;

        var userId = _auth?.UserIdString;
        var userName = _auth?.UserName;
        var hostname = Environment.MachineName;
        var process = Process.GetCurrentProcess().ProcessName;

        _writer?.Write(level, userId, userName, hostname, process, message, exception);
    }
}
