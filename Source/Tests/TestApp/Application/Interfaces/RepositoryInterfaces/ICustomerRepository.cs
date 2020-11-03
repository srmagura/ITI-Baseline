using ITI.DDD.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.DataContext;
using TestApp.Domain;

namespace TestApp.Application.Interfaces.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        void Add(Customer customer);
    }
}
