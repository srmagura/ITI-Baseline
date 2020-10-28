using ITI.DDD.Core.Exceptions;
using System;

namespace ITI.DDD.Services.Exceptions
{
    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string message)
            : base(message, AppServiceLogAs.None)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException, AppServiceLogAs.None)
        {
        }
    }
}