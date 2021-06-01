using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ITI.DDD.Infrastructure.DataMapping
{
    public static class IMapperExtensions
    {
        public static TResult? ProjectToDto<T, TResult>(this IMapper mapper, IQueryable<T> queryable) 
            where T : class
            where TResult : class
        {
            return mapper.ProjectTo<TResult>(queryable.AsNoTracking()).FirstOrDefault();
        }

        public static async Task<TResult?> ProjectToDtoAsync<T, TResult>(this IMapper mapper, IQueryable<T> queryable)
            where T : class
            where TResult : class
        {
            return await mapper.ProjectTo<TResult>(queryable.AsNoTracking()).FirstOrDefaultAsync();
        }

        public static List<TResult> ProjectToDtoList<T, TResult>(this IMapper mapper, IQueryable<T> queryable) 
            where T : class
            where TResult : class
        {
            return mapper.ProjectTo<TResult>(queryable.AsNoTracking()).ToList();
        }

        public static async Task<List<TResult>> ProjectToDtoListAsync<T, TResult>(this IMapper mapper, IQueryable<T> queryable)
            where T : class
            where TResult : class
        {
            return await mapper.ProjectTo<TResult>(queryable.AsNoTracking()).ToListAsync();
        }
    }
}
