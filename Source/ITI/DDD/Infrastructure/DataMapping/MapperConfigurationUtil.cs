using AutoMapper;
using ITI.DDD.Domain;

namespace ITI.DDD.Infrastructure.DataMapping;

public static class MapperConfigurationUtil
{
    public static void MapIdentity<TIdentity>(IMapperConfigurationExpression cfg, Func<Guid?, TIdentity> factory)
        where TIdentity : Identity
    {
        cfg.CreateMap<TIdentity, Guid?>()
            .ConvertUsing(id => id == null ? null : id.Guid);

        cfg.CreateMap<Guid?, TIdentity?>()
            .ConvertUsing(guid => guid == null ? null : factory(guid.Value));

        cfg.CreateMap<TIdentity, Guid>()
            .ConvertUsing(id => id.Guid);

        cfg.CreateMap<Guid, TIdentity>()
            .ConvertUsing(guid => factory(guid));
    }
}
