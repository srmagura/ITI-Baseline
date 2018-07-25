using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Iti.Core.DataContext;
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

        /*
        protected static void ConfigureValueObjects(IMapperConfigurationExpression cfg)
        {
            var vobjTypeList = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(IValueObject).IsAssignableFrom(assemblyType)
                select assemblyType).ToArray();

            foreach (var vobjType in vobjTypeList)
            {
                if (vobjType.Name == "IValueObject" || vobjType.Name == "ValueObject`1")
                    continue;

                Console.WriteLine($"CONFIG VALUE OBJECT: {vobjType.Name}");

                cfg.CreateMap(vobjType, vobjType);
            }
        }
        */
    }
}