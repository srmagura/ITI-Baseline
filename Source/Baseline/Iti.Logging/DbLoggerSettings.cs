using System.Diagnostics;
using System.Threading;
using Iti.Auth;
using Iti.Inversion;

namespace Iti.Logging
{
    public class DbLoggerSettings
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; } = "LogEntries";
    }
}