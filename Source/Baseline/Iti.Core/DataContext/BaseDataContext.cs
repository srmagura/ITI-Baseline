using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iti.Baseline.Core.Audit;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Iti.Baseline.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Iti.Baseline.Core.DataContext
{
    public abstract class BaseDataContext : DbContext
    {
        private Auditor _auditor;
        private DomainEvents _domainEvents;

        protected BaseDataContext() { }

        protected BaseDataContext(DbContextOptions options) : base(options) { }

        protected BaseDataContext(Auditor auditor, DomainEvents domainEvents)
        {
            Initialize(auditor, domainEvents);
        }

        public void Initialize(Auditor auditor, DomainEvents domainEvents)
        {
            _auditor = auditor;
            _domainEvents = domainEvents;
        }

        public override int SaveChanges()
        {
            UpdateEntityMaps();
            HandleAudit(_auditor);
            HandleDomainEvents(_domainEvents);

            return base.SaveChanges();
        }

        private void HandleDomainEvents(DomainEvents domainEvents)
        {
            if (_domainEvents == null)
                return;

            foreach (var entry in ChangeTracker.Entries())
            {
                var dbe = entry.Entity as DbEntity;
                if (dbe?.MappedEntity == null)
                    continue;

                if (dbe.MappedEntity?.DomainEvents?.HasItems() ?? false)
                {
                    domainEvents.RaiseAll(dbe.MappedEntity?.DomainEvents);
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new Exception("DO NOT USE ASYNC SAVE (domain events will fail)");
        }

        internal void UpdateEntityMaps()
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

        internal void HandleAudit(Auditor audit)
        {
            if (audit == null)
                return;

            if (this is IAuditDataContext dc)
            {
                audit?.Process(dc, ChangeTracker);
            }
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
