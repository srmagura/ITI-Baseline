using System.Threading.Tasks;
using TestApp.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Application.Interfaces.RepositoryInterfaces
{
    public interface IFacilityRepository
    {
        Task<Facility?> GetAsync(FacilityId id);

        void Add(Facility facility);
        Task RemoveAsync(FacilityId id);
    }
}
