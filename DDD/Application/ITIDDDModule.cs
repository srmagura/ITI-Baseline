using Autofac;
using ITI.DDD.Core;
using ITI.DDD.Infrastructure.DataMapping;
using ITI.DDD.Logging;

namespace ITI.DDD.Application;

public class ITIDDDModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DbEntityMapper>().As<IDbEntityMapper>();
        builder.RegisterType<Logger>().As<ILogger>();

        builder.RegisterType<UnitOfWorkProvider>()
            .As<IUnitOfWorkProvider>()
            .InstancePerLifetimeScope();
    }
}
