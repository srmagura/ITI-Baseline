using ITI.DDD.Core;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Infrastructure.DataMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Infrastructure
{
    public static class DDDInfrastructureConfig
    {
        public static void AddRegistrations(IOC ioc)
        {
            ioc.RegisterType<IDbEntityMapper, DbEntityMapper>();
        }
    }
}
