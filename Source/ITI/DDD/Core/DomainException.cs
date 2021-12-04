namespace ITI.DDD.Core;

public class DomainException : Exception
{
    public enum AppServiceLogAs
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
    }

    public DomainException(string message, AppServiceLogAs appServiceShouldLog)
        : base(message)
    {
        AppServiceShouldLog = appServiceShouldLog;
    }

    public DomainException(string message, Exception innerException, AppServiceLogAs appServiceShouldLog)
        : base(message, innerException)
    {
        AppServiceShouldLog = appServiceShouldLog;
    }

    public AppServiceLogAs AppServiceShouldLog { get; }
}
