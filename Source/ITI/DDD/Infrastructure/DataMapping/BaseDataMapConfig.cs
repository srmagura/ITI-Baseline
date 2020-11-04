using AutoMapper;
using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ITI.DDD.Infrastructure.DataMapping
{
    public abstract class BaseDataMapConfig
    {
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
    }
}
