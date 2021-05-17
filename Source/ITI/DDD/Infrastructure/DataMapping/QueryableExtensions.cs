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

        public static List<TDto> ProjectToDtoList<TDbEntity, TDto>(this IQueryable<TDbEntity> q, IMapper mapper)
            where TDbEntity : class
            where TDto : class
        {
            return mapper.ProjectToDtoList<TDbEntity, TDto>(q);
        }
    }
}
