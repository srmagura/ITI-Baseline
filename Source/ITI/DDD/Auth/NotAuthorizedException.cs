using ITI.DDD.Core;
using System;

namespace ITI.DDD.Auth;

public class NotAuthorizedException : DomainException
{
    public NotAuthorizedException()
        : base("You are not authorized to perform this action.", AppServiceLogAs.None)
    {
    }

    public NotAuthorizedException(Exception innerException)
        : base("You are not authorized to perform this action.", innerException, AppServiceLogAs.None)
    {
    }
}
