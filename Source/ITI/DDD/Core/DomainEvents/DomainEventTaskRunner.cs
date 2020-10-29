using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ITI.DDD.Logging;

namespace ITI.DDD.Domain.DomainEvents
{
    public class DomainEventTaskRunner
    {
        private readonly ILogger _logger;
        private readonly IAuthScopeResolver _authScopeResolver;

        public DomainEventTaskRunner(ILogger logger, IAuthScopeResolver authScopeResolver)
        {
            _logger = logger;
            _authScopeResolver = authScopeResolver;
        }

        public Task Run<TService>(string name, Action<TService> action) where TService: notnull
        {
            var appAuthContext = _authScopeResolver.GetDomainEventHandlerAuthContext();

            var task = Task.Run(() =>
                {
                    try
                    {
                        using (var innerScope = _authScopeResolver.BeginLifetimeScope(appAuthContext))
                        {
                            var st = innerScope.Resolve<TService>();
                            action(st);
                        }
                    }
                    catch (Exception exc)
                    {
                        _logger.Error($"TASK ERROR: {typeof(TService).Name}:{name}: {exc.Message}", exc);
                    }
                }
            );

            return task;
        }
    }
}
