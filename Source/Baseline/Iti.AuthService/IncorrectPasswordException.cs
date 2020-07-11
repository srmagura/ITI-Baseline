using System;
using Iti.Baseline.Exceptions;

namespace Iti.Baseline.AuthService
{
    public class IncorrectPasswordException : DomainException
    {
        public IncorrectPasswordException() : base("Incorrect password", DomainException.AppServiceLogAs.None)
        {
        }

        public IncorrectPasswordException(Exception innerException) : base("Incorrect password", innerException, DomainException.AppServiceLogAs.None)
        {
        }
    }
}