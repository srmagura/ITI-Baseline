using System;
using Iti.Baseline.Exceptions;

namespace Iti.Baseline.AuthService
{
    public class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException() : base("Invalid password", DomainException.AppServiceLogAs.None)
        {
        }

        public InvalidPasswordException(Exception innerException) : base("Invalid password", innerException, DomainException.AppServiceLogAs.None)
        {
        }
    }
}