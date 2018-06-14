using System.Linq;
using Domain;
using FooSampleApp.Application.Interfaces;
using Iti.Core.DataContext;
using Iti.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Repositories
{
    public class EfFooRepository : Repository<SampleDataContext>, IFooRepository
    {
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
    }
}