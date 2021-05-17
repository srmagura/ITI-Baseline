using AutoMapper;
using AutoMapper.EquivalencyExpression;
using ITI.DDD.Domain.Entities;
using ITI.DDD.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ITI.DDD.Infrastructure.DataMapping
{
    public abstract class BaseDataMapConfig
    {
        protected static IMapper? Mapper;
        protected static IDbEntityMapper? DbEntityMapper;

        protected static void SetStaticMapper(IMapper mapper)
        {
            Mapper = mapper;
            DbEntityMapper = new DbEntityMapper(mapper);
        }

        protected static void BaseConfig(IMapperConfigurationExpression cfg)
        {
            cfg.DisableConstructorMapping();
            cfg.AddCollectionMappers();
        }

        protected static void MapIdentity<TIdent>(IMapperConfigurationExpression cfg, Func<Guid?, TIdent> constr)
            where TIdent : Identity, new()
        {
            cfg.CreateMap<TIdent, Guid?>()
                .ConvertUsing(id => id == null ? null : id.Guid)
                ;
            cfg.CreateMap<Guid?, TIdent>()
                .ConvertUsing(guid => guid == null ? null : constr(guid.Value))
                ;

            cfg.CreateMap<TIdent, Guid>()
                .ConvertUsing(id => id.Guid)
                ;
            cfg.CreateMap<Guid, TIdent>()
                .ConvertUsing(guid => constr(guid))
                ;
        }

        protected static void SetPrivateField(object obj, string fieldName, object? value)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, value);
            }

            var prop = obj.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            prop?.SetValue(obj, value);
        }
    }
}
