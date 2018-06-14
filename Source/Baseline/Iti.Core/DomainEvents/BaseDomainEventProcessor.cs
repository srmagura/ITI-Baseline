using System;
using System.Collections.Generic;
using Iti.Logging;

namespace Iti.Core.DomainEvents
{
    public abstract class BaseDomainEventProcessor : IDomainEventProcessor
    {
        public abstract void HandleEvents(Dictionary<Type, List<Type>> handlers, List<IDomainEvent> events);

        protected void HandleEvent(Dictionary<Type, List<Type>> handlers, IDomainEvent ev)
        {
            try
            {
                var eventType = ev.GetType();

                if (!handlers.ContainsKey(eventType))
                    return;

                var handlerTypes = handlers[eventType];
                foreach (var handlerType in handlerTypes)
                {
                    CallHandler(ev, handlerType);
                }

            }
            catch (Exception exc)
            {
                Log.Error($"Error processing domain event: {ev?.GetType().Name}", exc);
            }
        }

        protected void CallHandler(IDomainEvent ev, Type handlerType)
        {
            try
            {
                var handler = Activator.CreateInstance(handlerType);

                var handleMethod = handler.GetType().GetMethod("Handle", new[] { ev.GetType() });
                if (handleMethod != null)
                {
                    handleMethod.Invoke(handler, new object[] { ev });
                }
            }
            catch (Exception exc)
            {
                Log.Error($"Error handling domain event {ev?.GetType().Name} with handler {handlerType?.Name}", exc);
            }
        }
    }
}