using Autofac;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.AppConfig;

namespace UnitTests.TestApp
{
    [TestClass]
    public class DataMapConfigTests
    {
        [TestMethod]
        public void AssertConfigurationIsValid()
        {
            var builder = new ContainerBuilder();
            DataMapConfig.RegisterMapper(builder);

            var container = builder.Build();
            var mapper = container.Resolve<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
