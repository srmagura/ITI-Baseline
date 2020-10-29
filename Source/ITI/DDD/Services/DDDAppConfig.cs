using ITI.DDD.Application.UnitOfWorkBase;
using ITI.DDD.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Application
{
    public static class DDDAppConfig
    {
        public static void AddRegistrations(IOC ioc)
        {
            ioc.RegisterType<IDomainEventRaiser, DomainEventRaiser>();
        }
    }
}
