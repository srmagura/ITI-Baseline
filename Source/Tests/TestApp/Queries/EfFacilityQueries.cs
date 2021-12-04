using AutoMapper;
using ITI.DDD.Core;
using ITI.DDD.Infrastructure;
using ITI.DDD.Infrastructure.DataMapping;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.DataContext;
using TestApp.DataContext.DataModel;
using TestApp.Domain.Identities;

namespace TestApp.Queries
{
    public class EfFacilityQueries : Queries<AppDataContext>, IFacilityQueries
    {
        private readonly IMapper _mapper;

        public EfFacilityQueries(IUnitOfWorkProvider uow, IMapper mapper) : base(uow)
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
