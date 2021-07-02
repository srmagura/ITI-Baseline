using ITI.DDD.Application.Exceptions;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleToAttribute("UnitTests")]

// TODO:SAM Give nice DbUpdateExceptions (feature that was in Baseline 1) ??!?
namespace ITI.DDD.Application
{
    public abstract class ApplicationService
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly ILogger Log;
        protected readonly IAuthContext Authorize;

        protected ApplicationService(IUnitOfWork uow, ILogger logger, IAuthContext baseAuth)
        {
            UnitOfWork = uow;
            Log = logger;
            Authorize = baseAuth;
        }

        protected async Task CommandAsync(Func<Task> authorize, Func<Task> exec)
        {
            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    await authorize();
                    await exec();
                    await uow.CommitAsync();
                }
            }
            catch (EntityNotFoundException enfExc)
            {
                Handle(enfExc);
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        protected async Task<T?> CommandAsync<T>(Func<Task> authorize, Func<Task<T?>> exec) 
        {
            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    await authorize();
                    var result = await exec();
                    await uow.CommitAsync();

                    return result;
                }
            }
            catch (EntityNotFoundException enfExc)
            {
                Handle(enfExc);
                return default;
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        protected async Task<T?> QueryAsync<T>(Func<Task> authorize, Func<Task<T?>> exec)
        {
            try
            {
                using (UnitOfWork.Begin())
                {
                    await authorize();

                    return await exec();
                }
            }
            catch (EntityNotFoundException enfExc)
            {
                Handle(enfExc);
                return default;
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        protected void Handle(Exception exc)
        {
            if (exc is DbUpdateException)
            {
                HandleDbUpdateException(exc);
                return;
            }

            if (exc is DomainException dexc)
            {
                if (dexc.AppServiceShouldLog == DomainException.AppServiceLogAs.None)
                    return;

                LogDomainException(dexc);
            }

            // by default, we log it... if we ever need more (like admin alerts) we'll address then
            Log?.Error("Unhandled application exception", exc);
        }

        private void LogDomainException(DomainException dexc)
        {
            switch (dexc.AppServiceShouldLog)
            {
                case DomainException.AppServiceLogAs.None:
                    break;
                case DomainException.AppServiceLogAs.Info:
                    Log?.Info("Unhandled Domain Exception", dexc);
                    break;
                case DomainException.AppServiceLogAs.Warning:
                    Log?.Warning("Unhandled Domain Exception", dexc);
                    break;
                default:
                case DomainException.AppServiceLogAs.Error:
                    Log?.Error("Unhandled Domain Exception", dexc);
                    break;
            }
        }

        private void HandleDbUpdateException(Exception exc, int depth = 0)
        {
            if (exc == null)
                return;

            if (depth > 10)
                return;

            if (exc.Message.StartsWith("Cannot insert duplicate key"))
            {
                GetDbUpdateInfo(exc.Message, out var table, out var value);
                throw new DuplicateKeyException(table, value, exc);
            }
            else if (exc.InnerException != null)
            {
                HandleDbUpdateException(exc.InnerException, depth + 1);
            }
        }

        public static void GetDbUpdateInfo(string message, out string table, out string value)
        {
            table = "";
            value = "";

            if (message == null)
                return;

            // find TABLE

            var cols = message.Split('\'');
            if (cols.Length < 2)
                return;

            table = cols[1]?.Replace("dbo.", "") ?? "";

            // find VALUE
            var pos = message.IndexOf('(');
            if (pos >= 0)
            {
                value = message.Substring(pos + 1);
                pos = value.LastIndexOf(')');
                value = value.Substring(0, pos);
            }
        }
    }
}