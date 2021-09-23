using Autofac;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Logging;

namespace ITI.DDD.Application
{
    public static class DDDAppConfig
    {
        public static void AddRegistrations(ContainerBuilder builder)
        {
            builder.RegisterType<Logger>().As<ILogger>();

            builder.RegisterType<DomainEvents>().As<IDomainEvents>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWorkImpl>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}
