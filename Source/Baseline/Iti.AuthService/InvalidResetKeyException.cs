using System;
using Iti.Exceptions;

namespace Iti.AuthService
{
    public class InvalidResetKeyException : DomainException
    {
        public InvalidResetKeyException() : base("Invalid Password Reset Key", DomainException.AppServiceLogAs.None)
        {
        }

        public InvalidResetKeyException(Exception innerException) : base("Invalid Password Reset Key", innerException, DomainException.AppServiceLogAs.None)
        {
        }
    }
}