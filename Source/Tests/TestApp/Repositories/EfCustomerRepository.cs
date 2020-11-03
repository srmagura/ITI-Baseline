using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.DataContext;
using TestApp.Domain;

namespace TestApp.Repositories
{
    public class EfCustomerRepository : Repository<AppDataContext>
    {
        public EfCustomerRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public void Add(Customer customer)
        {
            var dbCustomer = customer;
            Context.Add(dbCustomer);
        }
    }
}
