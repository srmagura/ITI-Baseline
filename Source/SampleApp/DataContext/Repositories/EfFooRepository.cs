using System.Linq;
using Domain;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.Repositories;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Microsoft.EntityFrameworkCore;
using SampleApp.Application.Interfaces;

namespace DataContext.Repositories
{
    public class EfFooRepository : Repository<SampleDataContext>, IFooRepository
    {
        public EfFooRepository(IUnitOfWork uow) : base(uow)
        {
        }

        private IQueryable<DbFoo> Aggregate => Context.Foos.Include(p => p.Bars).AsQueryable();

        public void Add(Foo foo)
        {
            var dbFoo = DbEntity.ToDb<DbFoo>(foo);
            Context.Foos.Add(dbFoo);
        }

        public Foo Get(FooId id)
        {
            var dbfoo = Aggregate
                .FirstOrDefault(p => p.Id == id.Guid);

            return dbfoo?.ToEntity<Foo>();
        }

        public void Remove(FooId id)
        {
            var dbfoo = Context.Foos.FirstOrDefault(p => p.Id == id.Guid);
            if (dbfoo != null)
                Context.Remove(dbfoo);
        }
    }
}