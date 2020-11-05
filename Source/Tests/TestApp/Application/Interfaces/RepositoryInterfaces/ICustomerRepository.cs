using System;
using System.Collections.Generic;
using System.Text;
using TestApp.DataContext;
using TestApp.Domain;

namespace TestApp.Application.Interfaces.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        Customer Get(CustomerId id);

        void Add(Customer customer);
        void Remove(CustomerId id);
    }
}
