using DataContext;
using Iti.Baseline.Logging;

namespace AppConfig
{
    public class DbLoggerSettings : IDbLoggerSettings
    {
        public string LogConnectionString => SampleDataContext.GetConnectionString();
        public string LogTableName => "LogEntries";
    }
}