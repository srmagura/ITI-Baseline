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

        protected static void MapIdentity<TIdentity>(IMapperConfigurationExpression cfg)
            where TIdentity : Identity, new()
        {
            cfg.CreateMap<TIdentity, Guid>()
                .ConstructUsing(p => p.Guid)
                .ReverseMap()
                .ConstructUsing(p => new TIdentity() { Guid = p });
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

        protected static Guid GetDbId(Entity e)
        {
            var prop = e.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var id = prop?.GetValue(e) as Identity;
            return id?.Guid ?? Guid.Empty;
        }
    }
}
