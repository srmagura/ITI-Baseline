using ITI.DDD.Core;

namespace ITI.DDD.Auth;

public class NotAuthorizedException : DomainException
{
    public NotAuthorizedException(string? message = null, Exception? innerException = null)
        : base(message ?? "You are not authorized to perform this action.", innerException, AppServiceLogAs.None)
    {
    }
}
