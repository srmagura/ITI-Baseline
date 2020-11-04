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

        private static Dictionary<Type, List<Type>> _handlerTypes = new Dictionary<Type, List<Type>>();

        public static void ClearRegistrations()
        {
            _handlerTypes = new Dictionary<Type, List<Type>>();
        }

        public static void Register<TEvent, THandler>()
            where TEvent : class, IDomainEvent
            where THandler : class, IDomainEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            var handlerType = typeof(THandler);

            List<Type> handlerTypeList;
            if (_handlerTypes.ContainsKey(eventType))
            {
                handlerTypeList = _handlerTypes[eventType];
            }
            else
            {
                handlerTypeList = new List<Type>();
                _handlerTypes.Add(eventType, handlerTypeList);
            }

            // don't allow duplicate registrations for handlers
            if (handlerTypeList.All(p => p != handlerType))
            {
                handlerTypeList.Add(handlerType);
            }
        }

        private List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

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

                var tasks = new List<Task>();

                foreach (var domainEvent in _domainEvents)
                {
                    var domainEventType = domainEvent.GetType();

                    if (!_handlerTypes.ContainsKey(domainEventType))
                        continue;

                    var handlerTypes = _handlerTypes[domainEventType];
                    if (handlerTypes == null)
                        continue;

                    foreach (var handlerType in handlerTypes)
                    {
                        tasks.Add(ExecuteHandler(handlerType, domainEvent));
                    }
                }

                _domainEvents.Clear();

                await Task.WhenAll(tasks);
            }
            catch (Exception exc)
            {
                _logger?.Error($"Domain Event processing exception", exc);
            }
        }

        private Task ExecuteHandler(Type handlerType, IDomainEvent domainEvent)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var scope = _domainEventAuthScopeResolver.BeginLifetimeScope())
                    {
                        var handler = scope.Resolve(handlerType);

                        var handleMethod = handler.GetType()
                            .GetMethod("Handle", new[] { domainEvent.GetType() });
                        if (handleMethod == null)
                        {
                            _logger?.Error($"Domain Event: could not resolve handler method for {handlerType.Name}");
                            return;
                        }

                        handleMethod.Invoke(handler, new object[] { domainEvent });
                    }
                }
                catch (Exception exc)
                {
                    _logger.Error($"DomainEvent:{handlerType.Name}: {exc.Message}", exc);
                }
            }
            );
        }
    }
}