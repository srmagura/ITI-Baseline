using System;

namespace Iti.Exceptions
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