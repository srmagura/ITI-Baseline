using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ITI.DDD.Application;

public abstract class ApplicationService
{
    protected readonly IUnitOfWorkProvider UnitOfWorkProvider;
    protected readonly ILogger Logger;
    protected readonly IAuthContext Authorize;

    protected ApplicationService(IUnitOfWorkProvider unitOfWorkProvider, ILogger logger, IAuthContext auth)
    {
        UnitOfWorkProvider = unitOfWorkProvider;
        Logger = logger;
        Authorize = auth;
    }

    protected async Task CommandAsync(Func<Task> authorize, Func<Task> exec)
    {
        try
        {
            using var uow = UnitOfWorkProvider.Begin();

            await authorize();
            await exec();
            await uow.CommitAsync();
        }
        catch (Exception exc)
        {
            Handle(exc);
            throw;
        }
    }

    protected async Task<T> CommandAsync<T>(Func<Task> authorize, Func<Task<T>> exec)
    {
        try
        {
            using var uow = UnitOfWorkProvider.Begin();

            await authorize();
            var result = await exec();
            await uow.CommitAsync();

            return result;
        }
        catch (Exception exc)
        {
            Handle(exc);
            throw;
        }
    }

    protected async Task<T> QueryAsync<T>(Func<Task> authorize, Func<Task<T>> exec)
    {
        try
        {
            using (UnitOfWorkProvider.Begin())
            {
                await authorize();

                return await exec();
            }
        }
        catch (Exception exc)
        {
            Handle(exc);
            throw;
        }
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        if(UnitOfWorkProvider.Current == null)
        {
            throw new NotSupportedException(
                $"{nameof(RaiseDomainEvent)} was called outside a unit of work."
            );
        }

        UnitOfWorkProvider.Current.RaiseDomainEvent(domainEvent);
    }

    private void Handle(Exception e)
    {
        if (e is DbUpdateException)
        {
            HandleDbUpdateException(e);
            return;
        }

        if (e is DomainException domainException)
        {
            LogDomainException(domainException);
        }

        // by default, we log it... if we ever need more (like admin alerts) we'll address then
        Logger.Error("Unhandled application exception", e);
    }

    private void LogDomainException(DomainException domainException)
    {
        switch (domainException.AppServiceShouldLog)
        {
            case DomainException.AppServiceLogAs.None:
                break;
            case DomainException.AppServiceLogAs.Info:
                Logger.Info("Unhandled Domain Exception", domainException);
                break;
            case DomainException.AppServiceLogAs.Warning:
                Logger.Warning("Unhandled Domain Exception", domainException);
                break;
            default:
            case DomainException.AppServiceLogAs.Error:
                Logger.Error("Unhandled Domain Exception", domainException);
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

    private static void GetDbUpdateInfo(string message, out string table, out string value)
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
            value = message[(pos + 1)..];
            pos = value.LastIndexOf(')');
            value = value[..pos];
        }
    }
}
