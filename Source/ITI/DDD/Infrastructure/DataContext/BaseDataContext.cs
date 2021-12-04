using AutoMapper;
using ITI.DDD.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITI.DDD.Infrastructure.DataContext
{
    public abstract class BaseDataContext : DbContext, IDataContext
    {
        private readonly IMapper? _mapper;
        private readonly IAuditor? _auditor;

        protected BaseDataContext()
        {
        }

        protected BaseDataContext(IMapper mapper, IAuditor auditor)
        {
            _mapper = mapper;
            _auditor = auditor;
        }

        public override int SaveChanges()
        {
            UpdateEntityMaps();
            _auditor?.Process(this);

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateEntityMaps();
            _auditor?.Process(this);

            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateEntityMaps()
        {
            if(_mapper == null)
                return;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is DbEntity dbEntity)
                {
                    if (dbEntity.MappedEntity != null)
                    {
                        _mapper.Map(dbEntity.MappedEntity, dbEntity);
                    }
                }
            }

            ChangeTracker.DetectChanges();
        }

        public List<IDomainEvent> GetAllDomainEvents()
        {
            var domainEvents = new List<IDomainEvent>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is DbEntity dbEntity)
                {
                    domainEvents.AddRange(dbEntity.DomainEvents);
                }
            }

            return domainEvents;
        }
    }
}
