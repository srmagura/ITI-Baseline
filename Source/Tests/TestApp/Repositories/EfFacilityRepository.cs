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
using System.Threading.Tasks;

namespace TestApp.Repositories
{
    public class EfFacilityRepository : Repository<AppDataContext>, IFacilityRepository
    {
        public EfFacilityRepository(IUnitOfWork uow, IDbEntityMapper dbMapper) 
            : base(uow, dbMapper)
        {
        }

        private IQueryable<DbFacility> Aggregate => Context.Facilities!
            .AsQueryable();

        public async Task<Facility?> GetAsync(FacilityId id)
        {
            var dbFacility = await Aggregate.FirstOrDefaultAsync(c => c.Id == id.Guid);
            
            return dbFacility != null
                ? DbMapper.ToEntity<Facility>(dbFacility)
                : null;
        }

        public void Add(Facility facility)
        {
            var dbFacility = DbMapper.ToDb<DbFacility>(facility);
            Context.Facilities!.Add(dbFacility);
        }

        public async Task RemoveAsync(FacilityId id)
        {
            var dbFacility = await Aggregate
                .FirstOrDefaultAsync(c => c.Id == id.Guid);

            if(dbFacility != null)
                Context.Facilities!.Remove(dbFacility);
        }
    }
}
