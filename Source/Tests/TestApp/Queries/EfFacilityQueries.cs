using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure;
using ITI.DDD.Infrastructure.DataMapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.DataContext;
using TestApp.DataContext.DataModel;
using TestApp.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Queries
{
    public class EfFacilityQueries : Queries<AppDataContext>, IFacilityQueries
    {
        private readonly IMapper _mapper;

        public EfFacilityQueries(IUnitOfWork uow, IMapper mapper) : base(uow)
        {
            _mapper = mapper;
        }

        public Task<FacilityDto?> GetAsync(FacilityId id)
        {
            return Context.Facilities!
                .Where(p => p.Id == id.Guid)
                .ProjectToDtoAsync<DbFacility, FacilityDto>(_mapper);
        }
    }
}
