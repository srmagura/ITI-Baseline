using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ITI.DDD.Infrastructure.DataMapping;

public static class IMapperExtensions
{
    public static async Task<TDto?> ProjectToDtoAsync<T, TDto>(this IMapper mapper, IQueryable<T> queryable)
        where T : class
        where TDto : class
    {
        return await mapper.ProjectTo<TDto?>(queryable.AsNoTracking()).FirstOrDefaultAsync();
    }

    public static async Task<List<TDto>> ProjectToDtoListAsync<T, TDto>(this IMapper mapper, IQueryable<T> queryable)
        where T : class
        where TDto : class
    {
        return await mapper.ProjectTo<TDto>(queryable.AsNoTracking()).ToListAsync();
    }
}
