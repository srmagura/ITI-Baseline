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
    public class EfFacilityRepository : Repository<AppDataContext>, IFacilityRepository
    {
        public EfFacilityRepository(IUnitOfWork uow, IDbEntityMapper dbMapper) 
            : base(uow, dbMapper)
        {
        }

        private IQueryable<DbFacility> Aggregate => Context.Facilities!
            .AsQueryable();

        public Facility Get(FacilityId id)
        {
            var dbFacility = Aggregate.FirstOrDefault(c => c.Id == id.Guid);
            return DbMapper.ToEntity<Facility>(dbFacility);
        }

        public void Add(Facility facility)
        {
            var dbFacility = DbMapper.ToDb<DbFacility>(facility);
            Context.Facilities!.Add(dbFacility);
        }

        public void Remove(FacilityId id)
        {
            var dbFacility = Aggregate
                .FirstOrDefault(c => c.Id == id.Guid);

            Context.Facilities!.Remove(dbFacility);
        }
    }
}
