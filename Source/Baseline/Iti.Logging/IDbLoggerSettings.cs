namespace Iti.Baseline.Logging
{
    public interface IDbLoggerSettings
    {
        string LogConnectionString { get; }
        string LogTableName { get; }
    }
}