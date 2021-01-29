using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Infrastructure.DataMapping;
using ITI.DDD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.DataContext;
using TestApp.DataContext.DataModel;
using TestApp.Domain;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using TestApp.Domain.Identities;

namespace TestApp.Repositories
{
    public class EfCustomerRepository : Repository<AppDataContext>, ICustomerRepository
    {
        public EfCustomerRepository(IUnitOfWork uow, IDbEntityMapper dbMapper) 
            : base(uow, dbMapper)
        {
        }

        private IQueryable<DbCustomer> Aggregate => Context.Customers!
            .Include(c => c.LtcPharmacies)
            .AsQueryable();

        public Customer Get(CustomerId id)
        {
            var dbCustomer = Aggregate.FirstOrDefault(c => c.Id == id.Guid);
            return DbMapper.ToEntity<Customer>(dbCustomer);
        }

        public void Add(Customer customer)
        {
            var dbCustomer = DbMapper.ToDb<DbCustomer>(customer);
            Context.Customers!.Add(dbCustomer);
        }

        public void Remove(CustomerId id)
        {
            var dbCustomer = Aggregate
                .Include(c => c.LtcPharmacies)
                .FirstOrDefault(c => c.Id == id.Guid);

            if (dbCustomer != null)
            {
                Context.LtcPharmacies!.RemoveRange(dbCustomer.LtcPharmacies);
                Context.Customers!.Remove(dbCustomer);
            }
        }
    }
}
