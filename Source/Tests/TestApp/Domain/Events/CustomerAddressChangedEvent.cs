﻿using ITI.DDD.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Identities;

namespace TestApp.Domain.Events
{
    public class CustomerAddressChangedEvent : BaseDomainEvent
    {
        public CustomerId Id { get; set; }

        public CustomerAddressChangedEvent(CustomerId id)
        {
            Id = id;
        }
    }
}
