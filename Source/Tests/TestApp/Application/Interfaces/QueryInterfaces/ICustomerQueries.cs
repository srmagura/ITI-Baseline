using TestApp.Application.Dto;
using TestApp.Domain.Identities;

namespace TestApp.Application.Interfaces.QueryInterfaces
{
    public interface ICustomerQueries
    {
        Task<CustomerDto?> GetAsync(CustomerId id);
    }
}
