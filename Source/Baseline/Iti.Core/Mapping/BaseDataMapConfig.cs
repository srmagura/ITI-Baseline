using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Iti.Core.DataContext;
using Iti.Core.Entites;

namespace Iti.Core.Mapping
{
    public class BaseDataMapConfig
    {
        protected static T CreateInstance<T>(T existing)
            where T : class
        {
            if (existing != null)
            {
                Console.WriteLine("DATA:CREATE: Using exising instance");
                return existing;
            }

            Console.WriteLine("DATA:CREATE: Create new instance");

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
                dbValue = CreateInstance<T>(dbValue);
                return dbValue;
            }

            if (dbValue == null)
                dbValue = CreateInstance(dbValue);

            // TODO:JT: is not working???
            Mapper.Map(eValue, dbValue);

            // TODO:JT: remove console writelines

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
            var prop = e.GetType().GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var id = prop?.GetValue(e) as Identity;
            return id?.Guid ?? Guid.Empty;
        }

        protected static void MapIdentity<TIdent>(IMapperConfigurationExpression cfg)
            where TIdent : Identity, new()
        {
            cfg.CreateMap<TIdent, Guid>()
                .ConvertUsing(p => p.Guid)
                ;
            cfg.CreateMap<Guid, TIdent>()
                .ConvertUsing(p => new TIdent().WithId<TIdent>(p))
                ;
        }
    }
}