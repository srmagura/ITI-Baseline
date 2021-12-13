using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ITI.DDD.Infrastructure.DataMapping;

public static class IMapperExtensions
{
    public static async Task<TDto?> ProjectToDtoAsync<T, TDto>(this IMapper mapper, IQueryable<T> queryable)
        where T : class
        where TDto : class
    {
        return await mapper.ProjectTo<TDto?>(queryable.AsNoTracking()).FirstOrDefaultAsync();
    }

    public static async Task<TDto[]> ProjectToDtoArrayAsync<T, TDto>(this IMapper mapper, IQueryable<T> queryable)
        where T : class
        where TDto : class
    {
        return await mapper.ProjectTo<TDto>(queryable.AsNoTracking()).ToArrayAsync();
    }

    public static async Task<List<TDto>> ProjectToDtoListAsync<T, TDto>(this IMapper mapper, IQueryable<T> queryable)
        where T : class
        where TDto : class
    {
        return await mapper.ProjectTo<TDto>(queryable.AsNoTracking()).ToListAsync();
    }
}
