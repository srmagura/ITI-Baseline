using TestApp.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Application.Interfaces.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetAsync(CustomerId id);

        void Add(Customer customer);
        Task RemoveAsync(CustomerId id);
    }
}
