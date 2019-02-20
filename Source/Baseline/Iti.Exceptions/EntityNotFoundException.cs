using System;

namespace Iti.Exceptions
{
    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string message)
            : base(message,false)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException, false)
        {
        }
    }
}