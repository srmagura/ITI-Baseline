using AutoMapper;
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
        }

        protected static void MapIdentity<TIdent>(IMapperConfigurationExpression cfg, Func<Guid?, TIdent> constr)
            where TIdent : Identity, new()
        {
            cfg.CreateMap<TIdent?, Guid?>()
                .ConvertUsing(p => p == null ? (Guid?)null : p.Guid);
            cfg.CreateMap<Guid?, TIdent?>()
                .ConvertUsing(p => p == null ? null : constr(p.Value));
            cfg.CreateMap<TIdent, Guid>()
                .ConvertUsing(p => p.Guid);
            cfg.CreateMap<Guid, TIdent>()
                .ConvertUsing(p => constr(p));
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

        protected static List<TDb> MapCollection<TEntity, TDb>(IReadOnlyCollection<TEntity> eList, List<TDb> dbList)
           where TEntity : Entity
           where TDb : DbEntity
        {
            dbList ??= new List<TDb>();

            // remove
            dbList.RemoveAll(dbx => eList.All(ex => GetDbId(ex) != dbx.Id));

            // update
            foreach (var dbBar in dbList)
            {
                var eBar = eList.FirstOrDefault(p => GetDbId(p) == dbBar.Id);
                if (eBar != null)
                {
                    Mapper!.Map(eBar, dbBar);
                }
            }

            // add
            var toAdd = eList.Where(ex => dbList.All(dbx => dbx.Id != GetDbId(ex)))
                .Select(Mapper!.Map<TDb>)
                .ToList();
            dbList.AddRange(toAdd);

            return dbList;
        }
    }
}
