using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iti.Core.Audit;
using Iti.Core.DomainEventsBase;
using Iti.Core.UnitOfWorkBase.Interfaces;
using Iti.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.DataContext
{
    public abstract class BaseDataContext : DbContext, IUnitOfWorkParticipant
    {
        protected BaseDataContext() { }

        protected BaseDataContext(DbContextOptions options) : base(options) { }

        public void OnUnitOfWorkCommit(Auditor auditor, DomainEvents domainEvents)
        {
            SaveChanges(auditor, domainEvents);
        }

        public void SaveChanges(Auditor auditor, DomainEvents domainEvents)
        {
            UpdateEntityMaps();
            HandleAudit(auditor);
            HandleDomainEvents(domainEvents);

            base.SaveChanges();
        }

        private void HandleDomainEvents(DomainEvents domainEvents)
        {
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

        [Obsolete("Do not call SaveChanges directly!", true)]
        public override int SaveChanges()
        {
            UpdateEntityMaps();

            return base.SaveChanges();
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
