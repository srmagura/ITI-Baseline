using TestApp.Application.Dto;
using TestApp.Domain.Identities;

namespace TestApp.Application.Interfaces
{
    public interface IFacilityAppService
    {
        Task<FacilityDto?> GetAsync(FacilityId id);

        Task<FacilityId> AddAsync(string name);
        Task SetContactAsync(FacilityId id, FacilityContactDto contact);
    }
}
