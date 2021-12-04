using ITI.Baseline.Util.Validation;
using ITI.DDD.Application;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Logging;
using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.Domain;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application
{
    public class FacilityAppService : ApplicationService, IFacilityAppService
    {
        private readonly IFacilityQueries _facilityQueries;
        private readonly IFacilityRepository _facilityRepo;

        public FacilityAppService(
            IUnitOfWorkProvider uow, 
            ILogger logger, 
            IAuthContext auth,
            IFacilityQueries facilityQueries,
            IFacilityRepository facilityRepo
        ) : base(uow, logger, auth)
        {
            _facilityQueries = facilityQueries;
            _facilityRepo = facilityRepo;
        }

        public Task<FacilityDto?> GetAsync(FacilityId id)
        {
            return QueryAsync(
                () => Task.CompletedTask,
                () => _facilityQueries.GetAsync(id)
            );
        }

        public Task<FacilityId> AddAsync(
            string name
        )
        {
            return CommandAsync(
                () => Task.CompletedTask,
                () =>
                {
                    var facility = new Facility(
                        name,
                        new FacilityContact(null, null)
                    );                
                    _facilityRepo.Add(facility);
                    return Task.FromResult(facility.Id);
                }
            );
        }
  
        public Task SetContactAsync(FacilityId id, FacilityContactDto contact)
        {
            return CommandAsync(
                () => Task.CompletedTask,
                async () => {
                    var facility = await _facilityRepo.GetAsync(id)
                        ?? throw new ValidationException("Facility");

                    facility.SetContact(contact.ToValueObject());
                }
            );
        }
    }
}
