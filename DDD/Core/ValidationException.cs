namespace ITI.DDD.Core;

public class ValidationException : DomainException
{
    public ValidationException(string message)
        : base(message, AppServiceLogAs.None)
    {
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException, AppServiceLogAs.None)
    {
    }
}
