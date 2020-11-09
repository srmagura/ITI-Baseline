using ITI.DDD.Application;
using ITI.DDD.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Baseline.Audit
{
    public static class BaselineAuditConfig
    {
        public static void AddRegistrations(IOC ioc)
        {
            ioc.RegisterType<IAuditor, Auditor>();
            ioc.RegisterType<IAuditAppService, AuditAppService>();
        }
    }
}
