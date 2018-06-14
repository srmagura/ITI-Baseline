using System;
using Iti.Logging;

namespace Iti.Core.Services
{
    public abstract class ApplicationService
    {
        protected void Handle(Exception exc)
        {
            // by default, we log it... if we ever need more (like admin alerts) we'll address then
            Log.Error("Unhandled application exception", exc);
        }
    }
}