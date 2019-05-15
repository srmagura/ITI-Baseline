using System;
using System.Collections;
using System.Collections.Generic;
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
        protected static T CreateInstance<T>(T existing, bool fillNestedValueObjects)
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

        protected static object CreateInstance(Type t, object existing, bool fillNestedValueObjects)
        {
            if (existing != null)
            {
                return existing;
            }

            var result = Activator.CreateInstance(t,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new object[] { }, null);

            if (fillNestedValueObjects)
            {
                FillDbEntityNullValueObjects(t, result);
            }

            return result;
        }

        private static void FillDbEntityNullValueObjects(Type t, object result)
        {
            foreach (var prop in t.GetProperties())
            {
                if (typeof(IValueObject).IsAssignableFrom(prop.PropertyType))
                {
                    var val = prop.GetValue(result);
                    if (val == null)
                    {
                        val = CreateInstance(prop.PropertyType, null, true);
                        prop.SetValue(result, val);
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
                                var val = MapValueObjectsEntityToDbEntity(dbProp.PropertyType, eVal, dbVal);
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
                /*
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
                                SetValueObjectDbEntityToEntity(e, prop.Name, dbVal as IValueObject);
                            });
                        }
                    }
                }
                */
            });
        }

        /*
        private void SetValueObjectDbEntityToEntity(object obj, string fieldName, IValueObject valObj)
        {
            valObj = valObj.NullIfNoValue();

            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, valObj);
            }

            var prop = obj.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            prop?.SetValue(obj, valObj);

            if (prop != null)
            {
                foreach (var childProp in prop.PropertyType.GetProperties())
                {
                    if (typeof(IValueObject).IsAssignableFrom(childProp.PropertyType))
                    {
                        if (valObj == null)
                        {

                        }
                        else
                        {
                            var dbVal = childProp.GetValue(valObj);

                            if (dbVal != null)
                            {
                                SetValueObjectDbEntityToEntity(prop.GetValue(obj), childProp.Name,
                                    dbVal as IValueObject);
                            }
                        }
                    }
                }
            }
        }
        */

        private object MapValueObjectsEntityToDbEntity(Type t, object eValue, object dbValue)
        {
            if (eValue == null)
            {
                eValue = CreateInstance(t, null, true);
                // return dbValue;
            }

            if (dbValue == null)
            {
                dbValue = CreateInstance(t, null, true);
            }

            Mapper.Map(eValue, dbValue);

            FillDbEntityNullValueObjects(t, dbValue);

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

        protected static void MapIdentity<TIdent>(IMapperConfigurationExpression cfg, Func<Guid?, TIdent> constr)
            where TIdent : Identity, new()
        {
            cfg.CreateMap<TIdent, Guid?>()
                .ConvertUsing(p => p == null ? (Guid?)null : p.Guid);
            cfg.CreateMap<Guid?, TIdent>()
                .ConvertUsing(p => p == null ? null : constr(p.Value));
            cfg.CreateMap<TIdent, Guid>()
                .ConvertUsing(p => p.Guid);
            cfg.CreateMap<Guid, TIdent>()
                .ConvertUsing(p => constr(p));
        }

        public static void RemoveEmptyValueObjects(object obj)
        {
            if (obj == null)
                return;

            if (obj.GetType().IsPrimitive)
                return;
            if (obj is string)
                return;

            foreach (var prop in obj.GetType().GetProperties())
            {
                var val = prop.GetValue(obj);

                if (prop.PropertyType.IsPrimitive)
                    continue;

                if (prop.PropertyType == typeof(string))
                    continue;

                if (val == null)
                    continue;

                if (val is IValueObject valobj)
                {
                    if (!valobj.HasValue())
                        prop.SetValue(obj, null);
                    else
                        RemoveEmptyValueObjects(valobj);
                }
                else if (val is IDto dto)
                {
                    RemoveEmptyValueObjects(dto);
                }
                else if (prop.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                {
                    if (val is IEnumerable list)
                    {
                        foreach (var child in list)
                        {
                            RemoveEmptyValueObjects(child);
                        }
                    }
                }
            }
        }

        public static void FillNullValueObjects(object obj)
        {
            if (obj == null)
                return;

            foreach (var prop in obj.GetType().GetProperties())
            {
                if (typeof(IValueObject).IsAssignableFrom(prop.PropertyType))
                {
                    var val = prop.GetValue(obj);

                    if (val == null)
                    {
                        val = Activator.CreateInstance(prop.PropertyType,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                            null, new object[] { }, null);

                        prop.SetValue(obj, val);
                    }

                    FillNullValueObjects(val);
                }
            }
        }
    }
}