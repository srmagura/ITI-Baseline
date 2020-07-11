namespace Iti.Baseline.Logging
{
    public class DbLoggerSettings
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; } = "LogEntries";
    }
}