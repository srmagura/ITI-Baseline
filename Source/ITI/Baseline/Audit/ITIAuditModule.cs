using Autofac;
using ITI.DDD.Infrastructure.DataContext;

namespace ITI.Baseline.Audit;

public class ITIAuditModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Auditor>().As<IAuditor>();
        builder.RegisterType<AuditAppService>().As<IAuditAppService>();
    }
}
