using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Iti.Core.DTOs
{
    public static class DtoExtensions
    {
        public static List<TDto> ProjectToDtoList<TDto>(this IQueryable q)
            where TDto : class, IDto
        {
            var data = q
                .ProjectTo<TDto>()
                .ToList();

            data.ForEach(p => Mapper.Map(p, p));

            return data;
        }

        public static TDto ProjectToDto<TDto>(this IQueryable q)
            where TDto : class, IDto
        {
            var inst = q.ProjectTo<TDto>().FirstOrDefault();

            Mapper.Map(inst, inst);

            return inst;
        }

        public static TDto ToDto<TDto>(this TDto dto)
            where TDto : class, IDto
        {
            if (dto == null)
                return null;
            Mapper.Map(dto, dto);
            return dto;
        }
    }
}