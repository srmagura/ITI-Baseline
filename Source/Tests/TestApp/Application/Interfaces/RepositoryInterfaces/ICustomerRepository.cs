using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.DataContext;
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
