using ITI.DDD.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Domain.Events
{
    public class CustomerAddedEvent : BaseDomainEvent
    {
        public CustomerId CustomerId { get; set; }

        public CustomerAddedEvent(CustomerId customerId)
        {
            CustomerId = customerId;
        }
    }
}
