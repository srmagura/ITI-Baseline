using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.DDD.Infrastructure.DataMapping
{
    public static class QueryableExtensions
    {
        public static TDto ProjectToDto<TDbEntity, TDto>(this IQueryable<TDbEntity> q, IMapper mapper)
            where TDbEntity : class
            where TDto : class
        {
            return mapper.ProjectToDto<TDbEntity, TDto>(q);
        }

        public static async Task<TDto> ProjectToDtoAsync<TDbEntity, TDto>(this IQueryable<TDbEntity> q, IMapper mapper)
            where TDbEntity : class
            where TDto : class
        {
            return await mapper.ProjectToDtoAsync<TDbEntity, TDto>(q);
        }

        public static List<TDto> ProjectToDtoList<TDbEntity, TDto>(this IQueryable<TDbEntity> q, IMapper mapper)
            where TDbEntity : class
            where TDto : class
        {
            return mapper.ProjectToDtoList<TDbEntity, TDto>(q);
        }

        public static async Task<List<TDto>> ProjectToDtoListAsync<TDbEntity, TDto>(this IQueryable<TDbEntity> q, IMapper mapper)
            where TDbEntity : class
            where TDto : class
        {
            return await mapper.ProjectToDtoListAsync<TDbEntity, TDto>(q);
        }
    }
}
