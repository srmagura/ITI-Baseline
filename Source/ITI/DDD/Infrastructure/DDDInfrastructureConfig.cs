using Autofac;
using ITI.DDD.Infrastructure.DataMapping;

namespace ITI.DDD.Infrastructure
{
    public static class DDDInfrastructureConfig
    {
        public static void AddRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<DbEntityMapper>().As<IDbEntityMapper>();
        }
    }
}
