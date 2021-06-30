using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ITI.DDD.Core;
using ITI.DDD.Logging;

namespace ITI.DDD.Domain.DomainEvents
{
    public class DomainEvents : IDomainEvents
    {
        private readonly ILogger _logger;
        private readonly IDomainEventAuthScopeResolver _domainEventAuthScopeResolver;

        public DomainEvents(ILogger logger, IDomainEventAuthScopeResolver domainEventAuthScopeResolver)
        {
            _logger = logger;
            _domainEventAuthScopeResolver = domainEventAuthScopeResolver;
        }

        private static Dictionary<Type, List<Type>> HandlerTypes = new();

        public static void ClearRegistrations()
        {
            HandlerTypes = new();
        }

        public static void Register<TEvent, THandler>()
            where TEvent : class, IDomainEvent
            where THandler : class, IDomainEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            var handlerType = typeof(THandler);

            List<Type> handlerTypeList;
            if (HandlerTypes.ContainsKey(eventType))
            {
                handlerTypeList = HandlerTypes[eventType];
            }
            else
            {
                handlerTypeList = new List<Type>();
                HandlerTypes.Add(eventType, handlerTypeList);
            }

            // don't allow duplicate registrations for handlers
            if (handlerTypeList.All(p => p != handlerType))
            {
                handlerTypeList.Add(handlerType);
            }
        }

        private readonly List<IDomainEvent> _domainEvents = new();

        public void Raise(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public async Task HandleAllRaisedEventsAsync()
        {
            try
            {
                if (_domainEvents == null)
                    return;

                foreach (var domainEvent in _domainEvents)
                {
                    var domainEventType = domainEvent.GetType();

                    if (!HandlerTypes.ContainsKey(domainEventType))
                        continue;

                    var handlerTypes = HandlerTypes[domainEventType];
                    if (handlerTypes == null)
                        continue;

                    foreach (var handlerType in handlerTypes)
                    {
                        // Don't execute domain events in parallel, in case event 2 
                        // depends on the result of event 1 
                        await ExecuteHandlerAsync(handlerType, domainEvent);
                    }
                }

                _domainEvents.Clear();
            }
            catch (Exception exc)
            {
                _logger?.Error($"Domain Event processing exception", exc);
            }
        }

        private async Task ExecuteHandlerAsync(Type handlerType, IDomainEvent domainEvent)
        {
            try
            {
                using var scope = _domainEventAuthScopeResolver.BeginLifetimeScope();
                var handler = scope.Resolve(handlerType);

                var handleMethod = handler.GetType()
                    .GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync), new[] { domainEvent.GetType() });
                if (handleMethod == null)
                {
                    _logger?.Error($"Domain Event: could not resolve handler method for {handlerType.Name}");
                    return;
                }

                var returnValue = handleMethod.Invoke(handler, new object[] { domainEvent });
                if (returnValue is Task t)
                    await t;
            }
            catch (Exception exc)
            {
                _logger.Error($"DomainEvent:{handlerType.Name}: {exc.Message}", exc);
            }
        }
    }
}