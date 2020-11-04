using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Infrastructure.DataMapping;
using ITI.DDD.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.DataContext;
using TestApp.DataContext.DataModel;
using TestApp.Domain;

namespace TestApp.Repositories
{
    public class EfCustomerRepository : Repository<AppDataContext>
    {
        public EfCustomerRepository(IUnitOfWork uow, IDbEntityMapper dbMapper) 
            : base(uow, dbMapper)
        {
        }

        private IQueryable<DbCustomer> Aggregate => Context.Customers!.AsQueryable();

        public Customer Get(CustomerId id)
        {
            var dbCustomer = Aggregate.FirstOrDefault(c => c.Id == id.Guid);
            return DbMapper.ToEntity<Customer>(dbCustomer);
        }

        public void Add(Customer customer)
        {
            var dbCustomer = DbMapper.ToDb<DbCustomer>(customer);
            Context.Add(dbCustomer);
        }
    }
}
