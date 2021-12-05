using ITI.DDD.Core;
using ITI.DDD.Infrastructure;
using ITI.DDD.Infrastructure.DataMapping;
using Microsoft.EntityFrameworkCore;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.DataContext;
using TestApp.DataContext.DataModel;
using TestApp.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Repositories;

public class EfFacilityRepository : Repository<AppDataContext>, IFacilityRepository
{
    public EfFacilityRepository(IUnitOfWorkProvider uow, IDbEntityMapper dbMapper)
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

        if (dbFacility != null)
            Context.Facilities!.Remove(dbFacility);
    }
}
