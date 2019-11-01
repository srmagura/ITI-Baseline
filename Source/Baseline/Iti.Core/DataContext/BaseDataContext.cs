using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iti.Core.Audit;
using Iti.Core.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.DataContext
{
    public class BaseDataContext : DbContext, IDomainEventContext
    {
        public BaseDataContext() { }
        public BaseDataContext(DbContextOptions options) : base(options) { }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            UpdateEntityMaps();
            HandleAudit();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new Exception("DO NOT USE ASYNC SAVE (domain events will fail)");
        }

        private void UpdateEntityMaps()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is DbEntity dbEntity)
                {
                    if (dbEntity.MappedEntity == null)
                        continue;
                    Mapper.Map(dbEntity.MappedEntity, dbEntity);
                }
            }
        }

        private void HandleAudit()
        {
            if (this is IAuditDataContext dc)
                Auditor.Process(dc, ChangeTracker);
        }

        //
        // DOMAIN EVENTS
        //

        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
        public IReadOnlyCollection<IDomainEvent> Events => _events;
        public void Add(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        public void Clear()
        {
            _events.Clear();
        }
    }
}
