using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Application
{
    public static class DDDAppConfig
    {
        public static void AddRegistrations(IOC ioc)
        {
            ioc.RegisterType<ILogger, Logger>();

            ioc.RegisterType<IDomainEventRaiser, DomainEventRaiser>();
            ioc.RegisterLifetimeScope<IDomainEvents, DomainEvents>();
            ioc.RegisterLifetimeScope<IUnitOfWork, UnitOfWorkImpl>();
        }
    }
}
