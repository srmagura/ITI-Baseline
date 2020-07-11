using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Iti.Logging;

namespace Iti.Core.DomainEventsBase
{
    public class DomainEvents
    {
        private readonly ILogger _logger;

        public DomainEvents(ILogger logger)
        {
            _logger = logger;
        }

        public static void Initialize(
            bool waitForDomainEvents = false
        )
        {
            _waitForDomainEvents = waitForDomainEvents;
        }

        private static bool _waitForDomainEvents;

        public static void WaitForDomainEvents(bool shouldWait)
        {
            _waitForDomainEvents = shouldWait;
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

        private List<IDomainEvent> _domainEvents = null;

        public void Raise(IDomainEvent domainEvent)
        {
            if (_domainEvents == null)
                _domainEvents = new List<IDomainEvent>();

            _domainEvents.Add(domainEvent);
        }

        public void RaiseAll(List<IDomainEvent> mappedEntityDomainEvents)
        {
            if (_domainEvents == null)
                _domainEvents = new List<IDomainEvent>();

            _domainEvents.AddRange(mappedEntityDomainEvents);
        }

        public void HandleAllRaisedEvents(ILifetimeScope scope)
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
                        var task = Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                using (var childScope = scope.BeginLifetimeScope())
                                {
                                    var handler = childScope.Resolve(handlerType);
                                    
                                    var handleMethod = handler.GetType()
                                        .GetMethod("Handle", new[] { domainEvent.GetType() });
                                    if(handleMethod == null)
                                        _logger?.Error($"Domain Event: could not resolve handler method for {handlerType.Name}");

                                    handleMethod.Invoke(handler, new object[] { domainEvent });
                                }
                            }
                            catch (Exception exc)
                            {
                                _logger?.Error(
                                    $"Domain Event threw exception: {domainEventType.Name} -> {handlerType.Name}", exc);
                            }
                        });
                        tasks.Add(task);
                    }
                }

                _domainEvents.Clear();

                if (_waitForDomainEvents)
                    Task.WhenAll(tasks).Wait();
            }
            catch (Exception exc)
            {
                _logger?.Error($"Domain Event processing exception", exc);
            }
        }


    }
}