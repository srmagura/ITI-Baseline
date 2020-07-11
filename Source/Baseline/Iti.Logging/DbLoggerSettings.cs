namespace Iti.Logging
{
    public class DbLoggerSettings
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; } = "LogEntries";
    }
}