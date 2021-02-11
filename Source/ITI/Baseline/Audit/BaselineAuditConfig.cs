using Autofac;
using ITI.DDD.Application;
using ITI.DDD.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Baseline.Audit
{
    public static class BaselineAuditConfig
    {
        public static void AddRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<Auditor>().As<IAuditor>();
            builder.RegisterType<AuditAppService>().As<IAuditAppService>();
        }
    }
}
