using Autofac;
using ITI.Baseline.RequestTracing;
using ITI.DDD.Application.DomainEvents.Direct;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.AppConfig;
using TestApp.DataContext;

namespace IntegrationTests.Harness;

public abstract class IntegrationTest
{
#nullable disable // To avoid unnecessary null checks in every test
    protected IContainer Container { get; set; }
#nullable enable

    protected static void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterModule<AppModule>();

        builder.RegisterType<ConsoleLogWriter>().As<ILogWriter>();

        var connectionStrings = new ConnectionStrings();
        builder.RegisterInstance(connectionStrings);
        builder.RegisterInstance<IDbLoggerSettings>(connectionStrings);
        builder.RegisterInstance<IDbRequestTraceSettings>(connectionStrings);
    }

    [TestInitialize]
    public async Task TestInitialize()
    {
        ConsoleLogWriter.ClearErrors();
        DirectDomainEventPublisher.ShouldWaitForHandlersToComplete(true);

        var builder = new ContainerBuilder();
        RegisterServices(builder);
        Container = builder.Build();

        await DeleteFromTablesUtil.DeleteFromTablesAsync(new ConnectionStrings().AppDataContext);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        Container.Dispose();
        Assert.IsFalse(ConsoleLogWriter.HasErrors);
    }
}
