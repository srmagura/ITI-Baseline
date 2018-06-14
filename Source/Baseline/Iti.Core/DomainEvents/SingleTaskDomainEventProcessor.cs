using System;
using System.Collections.Generic;
using Iti.Logging;

namespace Iti.Core.DomainEvents
{
    public class SingleTaskDomainEventProcessor : BaseDomainEventProcessor
    {
        public override void HandleEvents(Dictionary<Type, List<Type>> handlers, List<IDomainEvent> events)
        {
            try
            {
                foreach (var ev in events)
                {
                    HandleEvent(handlers, ev);
                }

            }
            catch (Exception exc)
            {
                Log.Error("Error processing domain events", exc);
            }
        }
    }
}