using System;
using System.Collections.Generic;
using Iti.Inversion;

namespace Iti.Core.DomainEvents
{
    public class DomainEvents
    {
        private static readonly Dictionary<Type, List<Type>> Handlers = new Dictionary<Type, List<Type>>();

        private static readonly object LockObject = new object();
        private static IDomainEventProcessor _processor;

        private static IDomainEventProcessor GetProcessor()
        {
            if (_processor == null)
            {
                lock (LockObject)
                {
                    if (_processor == null)
                    {
                        _processor = IOC.TryResolve<IDomainEventProcessor>()
                                     ?? new SingleTaskDomainEventProcessor();
                    }
                }
            }

            return _processor;
        }

        public static void Raise(IDomainEvent domainEvent)
        {
            var dec = DomainEventContext;
            dec?.Add(domainEvent);
        }

        public static Func<IDomainEventContext> ContextLocator = null;
        public static IDomainEventContext DomainEventContext => ContextLocator?.Invoke();

        public static void Register<TEvent, THandler>()
            where TEvent : IDomainEvent
            where THandler : IDomainEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            if (!Handlers.ContainsKey(eventType))
            {
                Handlers.Add(eventType, new List<Type>());
            }

            Handlers[eventType].Add(typeof(THandler));
        }

        internal static void HandleEvents(List<IDomainEvent> events)
        {
            var proc = GetProcessor();

            proc?.HandleEvents(Handlers,events);
        }

        public static void ClearRegistrations()
        {
            // Used for testing only (as far as I know)
            Handlers.Clear();
        }
    }
}