namespace ITI.DDD.Logging;

public interface IDbLoggerSettings
{
    string LogConnectionString { get; }
    string LogTableName { get; }
}
