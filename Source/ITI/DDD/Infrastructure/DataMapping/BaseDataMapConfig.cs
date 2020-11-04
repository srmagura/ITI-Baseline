using AutoMapper;
using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
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
    }
}
