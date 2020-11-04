using AutoMapper;
using ITI.DDD.Core;
using ITI.DDD.Infrastructure.DataMapping;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.DataContext.DataModel;
using TestApp.Domain;

namespace TestApp.AppConfig
{
    public class DataMapConfig : BaseDataMapConfig
    {
        public void RegisterMapper(IOC ioc)
        {
            var config = new MapperConfiguration(cfg => {
                ConfigureCustomer(cfg);
            });

            var mapper = new Mapper(config);
            ioc.RegisterInstance(mapper);
        }

        private static void ConfigureCustomer(IMapperConfigurationExpression cfg)
        {
            MapIdentity(cfg, p => new CustomerId(p));

            cfg.CreateMap<Customer, DbCustomer>()
                .ForMember(p => p.NotInEntity, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
