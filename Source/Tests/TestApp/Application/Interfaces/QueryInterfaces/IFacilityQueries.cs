using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Domain.Identities;

namespace TestApp.Application.Interfaces.QueryInterfaces
{
    public interface IFacilityQueries
    {
        Task<FacilityDto?> GetAsync(FacilityId id);
    }
}
