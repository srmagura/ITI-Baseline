using System;
using Domain.Events;
using Iti.Core.DomainEventsBase;
using Iti.Core.UnitOfWorkBase.Interfaces;
using Iti.Logging;

namespace CoreTests.Helpers
{
    public class TestFooEventHandlers :
        IDomainEventHandler<FooCreatedEvent>,
        IDomainEventHandler<FooAddressChangedEvent>,
        IDomainEventHandler<FooBarsChangedEvent>
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly ILogger _log;
        public static int FooCreated = 0;
        public static int FooAddressChanged = 0;
        public static int FooBarsChanged = 0;

        public TestFooEventHandlers(IUnitOfWork uow, ILogger log)
        {
            UnitOfWork = uow;
            _log = log;
        }

        public static void Clear()
        {
            FooCreated = 0;
            FooAddressChanged = 0;
            FooBarsChanged = 0;
        }

        public void Handle(FooCreatedEvent ev)
        {
            using (var uow = UnitOfWork.Begin())
            {
                FooCreated++;
                Console.WriteLine($">>>> EVENT HANDLER: {ev.CreatedUtc}: FooCreatedEvent: {ev.FooId}");

                _log.Info(">>> FOO CREATE EVENT HANDLER: Test Message to Log");

                uow.Commit();
            }
        }

        public void Handle(FooAddressChangedEvent ev)
        {
            using (var uow = UnitOfWork.Begin())
            {
                FooAddressChanged++;
                Console.WriteLine($">>>> EVENT HANDLER: {ev.CreatedUtc}: FooAddressChangedEvent: {ev.FooId}");

                _log.Info(">>> FOO ADDR CHANGED EVENT HANDLER: Test Message to Log");

                uow.Commit();
            }
        }

        public void Handle(FooBarsChangedEvent ev)
        {
            using (var uow = UnitOfWork.Begin())
            {
                FooBarsChanged++;
                Console.WriteLine($">>>> EVENT HANDLER: {ev.CreatedUtc}: FooBarsChangedEvent: {ev.FooId}");

                _log.Info(">>> FOO BARS CHANGED EVENT HANDLER: Test Message to Log");

                uow.Commit();
            }
        }
    }
}