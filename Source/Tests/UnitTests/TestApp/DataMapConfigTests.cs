using Autofac;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.AppConfig;

namespace UnitTests.TestApp;

[TestClass]
public class DataMapConfigTests
{
    [TestMethod]
    public void AssertConfigurationIsValid()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<MapperModule>();

        var container = builder.Build();
        var mapper = container.Resolve<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
