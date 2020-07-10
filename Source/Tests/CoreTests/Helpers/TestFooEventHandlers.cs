using System;
using Domain.Events;
using Iti.Core.DomainEventsBase;

namespace CoreTests.Helpers
{
    public class TestFooEventHandlers :
        IDomainEventHandler<FooCreatedEvent>,
        IDomainEventHandler<FooAddressChangedEvent>,
        IDomainEventHandler<FooBarsChangedEvent>
    {
        public static int FooCreated = 0;
        public static int FooAddressChanged = 0;
        public static int FooBarsChanged = 0;

        public static void Clear()
        {
            FooCreated = 0;
            FooAddressChanged = 0;
            FooBarsChanged = 0;
        }

        public void Handle(FooCreatedEvent ev)
        {
            FooCreated++;
            Console.WriteLine($">>>> EVENT HANDLER: {ev.CreatedUtc}: FooCreatedEvent: {ev.FooId}");
        }

        public void Handle(FooAddressChangedEvent ev)
        {
            FooAddressChanged++;
            Console.WriteLine($">>>> EVENT HANDLER: {ev.CreatedUtc}: FooAddressChangedEvent: {ev.FooId}");
        }

        public void Handle(FooBarsChangedEvent ev)
        {
            FooBarsChanged++;
            Console.WriteLine($">>>> EVENT HANDLER: {ev.CreatedUtc}: FooBarsChangedEvent: {ev.FooId}");
        }
    }
}