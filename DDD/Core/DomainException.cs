namespace ITI.DDD.Core;

public class DomainException : Exception
{
    public enum AppServiceLogAs
    {
        None,
        Info,
        Warning,
        Error,
    }

    public DomainException(string message, AppServiceLogAs appServiceShouldLog)
        : base(message)
    {
        AppServiceShouldLog = appServiceShouldLog;
    }

    public DomainException(string message, Exception? innerException, AppServiceLogAs appServiceShouldLog)
        : base(message, innerException)
    {
        AppServiceShouldLog = appServiceShouldLog;
    }

    public AppServiceLogAs AppServiceShouldLog { get; }
}
