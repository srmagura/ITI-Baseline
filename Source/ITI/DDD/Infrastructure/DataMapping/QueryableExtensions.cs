using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.DDD.Infrastructure.DataMapping
{
    public static class QueryableExtensions
    {
        public static async Task<TDto?> ProjectToDtoAsync<TDbEntity, TDto>(this IQueryable<TDbEntity> q, IMapper mapper)
            where TDbEntity : class
            where TDto : class
        {
            return await mapper.ProjectToDtoAsync<TDbEntity, TDto>(q);
        }

        public static async Task<List<TDto>> ProjectToDtoListAsync<TDbEntity, TDto>(this IQueryable<TDbEntity> q, IMapper mapper)
            where TDbEntity : class
            where TDto : class
        {
            return await mapper.ProjectToDtoListAsync<TDbEntity, TDto>(q);
        }
    }
}
