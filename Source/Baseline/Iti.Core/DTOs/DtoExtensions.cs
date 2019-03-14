using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iti.Core.Mapping;
using Iti.Core.ValueObjects;

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

            // data.ForEach(p => Mapper.Map(p, p));
            data.ForEach(BaseDataMapConfig.RemoveEmptyValueObjects);

            return data;
        }

        public static TDto ProjectToDto<TDto>(this IQueryable q)
            where TDto : class, IDto
        {
            var inst = q.ProjectTo<TDto>().FirstOrDefault();

            // Console.WriteLine($"ProjectToDto<{typeof(TDto)}>");

            // Mapper.Map(inst, inst);
            BaseDataMapConfig.RemoveEmptyValueObjects(inst);

            return inst;
        }

        public static TDto ToDto<TDto>(this TDto dto)
            where TDto : class, IDto
        {
            if (dto == null)
                return null;

            // Mapper.Map(dto, dto);
            BaseDataMapConfig.RemoveEmptyValueObjects(dto);

            return dto;
        }

        
    }
}