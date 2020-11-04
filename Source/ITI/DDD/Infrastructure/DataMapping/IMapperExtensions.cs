using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITI.DDD.Infrastructure.DataMapping
{
    public static class IMapperExtensions
    {
        public static T? ProjectToDto<T>(this IMapper mapper, IQueryable queryable) where T : class
        {
            return mapper.ProjectTo<T>(queryable).FirstOrDefault();
        }
    }
}
