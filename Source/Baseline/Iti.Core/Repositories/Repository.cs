using Iti.Core.UnitOfWorkBase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.Repositories
{
    public abstract class Repository<TDbContext> 
        where TDbContext : DbContext, IUnitOfWorkParticipant
    {
        private readonly IUnitOfWork _uow;

        protected Repository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected TDbContext Context => _uow.Current<TDbContext>();
    }
}
