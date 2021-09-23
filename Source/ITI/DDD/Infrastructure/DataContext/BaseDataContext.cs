using AutoMapper;
using ITI.DDD.Application;
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
        private IMapper? _mapper;
        private IAuditor? _auditor;

        public void Initialize(IMapper mapper, IAuditor auditor)
        {
            _mapper = mapper;
            _auditor = auditor;
        }

        void IDataContext.SaveChanges()
        {
            SaveChanges();
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
                // throw new Exception("SaveChanges called before DataContext initialized.");
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

            return domainEvents
                .OrderBy(e => e.DateCreatedUtc)
                .ToList();
        }
    }
}
