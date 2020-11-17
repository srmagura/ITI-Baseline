using ITI.DDD.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Identities;

namespace TestApp.Domain.Events
{
    public class CustomerAddedEvent : BaseDomainEvent
    {
        public CustomerId CustomerId { get; set; }
        public string Name { get; set; }

        public CustomerAddedEvent(CustomerId customerId, string name)
        {
            CustomerId = customerId;
            Name = name;
        }
    }
}
