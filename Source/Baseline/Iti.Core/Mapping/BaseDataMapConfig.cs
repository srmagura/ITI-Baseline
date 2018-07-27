using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Iti.Core.DataContext;
using Iti.Core.DTOs;
using Iti.Core.Entites;
using Iti.Core.ValueObjects;

namespace Iti.Core.Mapping
{
    public class BaseDataMapConfig
    {
        protected static T CreateInstance<T>(T existing)
            where T : class
        {
            if (existing != null)
            {
                return existing;
            }

            return Activator.CreateInstance(typeof(T),
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, new object[] { }, null)
                as T;
        }

        protected static object CreateInstance(Type t, object existing)
        {
            if (existing != null)
                return existing;

            return Activator.CreateInstance(t,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new object[] { }, null);
        }

        protected void ConfigureDtoValueObjects(IMapperConfigurationExpression cfg)
        {
            cfg.ForAllMaps((tm, me) =>
            {
                if (typeof(IDto).IsAssignableFrom(tm.DestinationType))
                {
                    me.AfterMap((x, dto) => DtoCleanup(dto as IDto));
                }
            });
        }

        private void DtoCleanup(IDto dto)
        {
            foreach (var prop in dto.GetType().GetProperties())
            {
                if (typeof(IValueObject).IsAssignableFrom(prop.PropertyType))
                {
                    if (prop.GetValue(dto) is IValueObject val)
                    {
                        if (!val.HasValue())
                            prop.SetValue(dto, null);
                    }
                }
            }
        }

        protected void ConfigureDbEntityValueObjects(IMapperConfigurationExpression cfg)
        {
            cfg.ForAllMaps((tm, me) =>
            {
                // Entity -> DbEntity
                if (typeof(Entity).IsAssignableFrom(tm.SourceType)
                    && typeof(DbEntity).IsAssignableFrom(tm.DestinationType))
                {
                    var eType = tm.SourceType;
                    var dbType = tm.DestinationType;

                    var dbProps = dbType.GetProperties();
                    foreach (var dbProp in dbProps)
                    {
                        if (typeof(IValueObject).IsAssignableFrom(dbProp.PropertyType))
                        {
                            me.ForMember(dbProp.Name, opt => opt.Ignore());
                            me.AfterMap((e, db) =>
                            {
                                var eProp = e.GetType().GetProperty(dbProp.Name);
                                var eVal = eProp.GetValue(e);
                                var dbVal = dbProp.GetValue(db);
                                var val = MapValueObjectRuntime(dbProp.PropertyType, eVal, dbVal);
                                dbProp.SetValue(db, val);
                            });
                        }
                        else if (typeof(DbEntity).IsAssignableFrom(dbProp.PropertyType))
                        {
                            var eProp = eType.GetProperty(dbProp.Name);
                            if (eProp == null)
                            {
                                me.ForMember(dbProp.Name, opt => opt.Ignore());
                            }

                            var idPropName = $"{dbProp.Name}Id";
                            var idProp = tm.DestinationType.GetProperty(idPropName);
                            var eIdProp = eType.GetProperty(idPropName);
                            if (idProp != null && eIdProp == null)
                            {
                                me.ForMember(idPropName, opt => opt.Ignore());
                            }
                        }
                        else // ignore unmapped
                        {
                            if (eType.GetProperty(dbProp.Name) == null)
                            {
                                me.ForMember(dbProp.Name, opt => opt.Ignore());
                            }
                        }
                    }
                }

                // DbEntity -> Entity
                if (typeof(DbEntity).IsAssignableFrom(tm.SourceType)
                    && typeof(Entity).IsAssignableFrom(tm.DestinationType))
                {
                    var props = tm.DestinationType.GetProperties();
                    foreach (var prop in props)
                    {
                        if (typeof(IValueObject).IsAssignableFrom(prop.PropertyType))
                        {
                            me.AfterMap((db, e) =>
                            {
                                var dbProp = db.GetType().GetProperty(prop.Name);
                                var dbVal = dbProp.GetValue(db);
                                SetValueObjectRuntime(prop.PropertyType, e, prop.Name, dbVal as IValueObject);
                            });
                        }
                    }
                }
            });
        }



        private void SetValueObjectRuntime(Type t, object obj, string fieldName, IValueObject valObj)
        {
            valObj = valObj.NullIfNoValue();

            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, valObj);
            }

            var prop = obj.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            prop?.SetValue(obj, valObj);
        }

        private object MapValueObjectRuntime(Type t, object eValue, object dbValue)
        {
            if (eValue == null)
            {
                dbValue = CreateInstance(t, dbValue);
                return dbValue;
            }

            if (dbValue == null)
                dbValue = CreateInstance(t, null);

            Mapper.Map(eValue, dbValue);

            return dbValue;
        }

        protected static List<T> ToList<T>(string s, Func<string, T> convert)
        {
            if (s == null || s.Trim() == "")
                return new List<T>();

            var list = s.Split(',').Select(convert).ToList();
            return list;
        }

        protected static void SetValueObject<T>(object obj, string fieldName, T valObj)
            where T : ValueObject<T>
        {
            valObj = valObj.NullIfNoValue();

            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, valObj);
            }

            var prop = obj.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            prop?.SetValue(obj, valObj);
        }

        protected static void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, value);
            }

            var prop = obj.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            prop?.SetValue(obj, value);
        }

        protected static T MapValueObject<T>(T eValue, T dbValue)
            where T : class
        {
            if (eValue == null)
            {
                dbValue = CreateInstance(dbValue);
                return dbValue;
            }

            if (dbValue == null)
                dbValue = CreateInstance((T)null);

            Mapper.Map(eValue, dbValue);

            return dbValue;
        }

        protected static List<TDb> MapCollection<TEntity, TDb>(IReadOnlyCollection<TEntity> eList, List<TDb> dbList)
            where TEntity : Entity
            where TDb : DbEntity
        {
            if (dbList == null)
                dbList = new List<TDb>();

            // remove
            dbList.RemoveAll(dbx => eList.All(ex => GetDbId(ex) != dbx.Id));

            // update
            foreach (var dbBar in dbList)
            {
                var eBar = eList.FirstOrDefault(p => GetDbId(p) == dbBar.Id);
                if (eBar != null)
                {
                    Mapper.Map(eBar, dbBar);
                }
            }

            // add
            var toAdd = eList.Where(ex => dbList.All(dbx => dbx.Id != GetDbId(ex)))
                .Select(Mapper.Map<TDb>)
                .ToList();
            dbList.AddRange(toAdd);

            return dbList;
        }

        protected static Guid GetDbId(Entity e)
        {
            var prop = e.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var id = prop?.GetValue(e) as Identity;
            return id?.Guid ?? Guid.Empty;
        }

        protected static void MapIdentity<TIdent>(IMapperConfigurationExpression cfg, Func<Guid, TIdent> constr)
            where TIdent : Identity, new()
        {
            cfg.CreateMap<TIdent, Guid?>()
                .ProjectUsing(p => p == null ? (Guid?)null : p.Guid);
            cfg.CreateMap<Guid?, TIdent>()
                .ProjectUsing(p => p == null ? null : constr(p.Value));
            cfg.CreateMap<TIdent, Guid>()
                .ProjectUsing(p => p.Guid);
            cfg.CreateMap<Guid, TIdent>()
                .ProjectUsing(p => constr(p));
        }
    }

    public static class MapperExtensions
    {
        public static IMappingExpression<TSource, TDestination>
            IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var existingMaps = Mapper.Configuration
                .GetAllTypeMaps()
                .First(x => x.SourceType.Equals(sourceType) && x.DestinationType.Equals(destinationType));
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }

        public static IMappingExpression
            IgnoreAllNonExisting(this IMappingExpression expression, Type sourceType, Type destinationType)
        {
            var existingMaps = Mapper.Configuration
                .GetAllTypeMaps()
                .First(x => x.SourceType.Equals(sourceType) && x.DestinationType.Equals(destinationType));
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }
    }
}