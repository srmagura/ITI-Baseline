using System;
using System.Collections.Generic;
using Autofac;
using Iti.Baseline.Core.Audit;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iti.Baseline.Core.UnitOfWorkBase
{
    public class UnitOfWorkImpl : IUnitOfWork
    {
        private readonly ILifetimeScope _scope;
        private readonly DomainEvents _domainEvents;
        private readonly Auditor _auditor;

        public UnitOfWorkImpl(ILifetimeScope scope, DomainEvents domainEvents, Auditor auditor)
        {
            // Console.WriteLine($"DEBUG: CREATE: UnitOfWork: {this.GetHashCode()} -- scope: {scope.GetHashCode()}");

            _scope = scope;
            _domainEvents = domainEvents;
            _auditor = auditor;
        }

        public IUnitOfWorkScope Begin()
        {
            // Console.WriteLine($"DEBUG: BEGIN: UnitOfWork: {this.GetHashCode()} -- scope: {_scope.GetHashCode()}");

            return new UnitOfWorkScope(this);
        }

        private readonly Dictionary<Type, BaseDataContext> _participants = new Dictionary<Type, BaseDataContext>();
        private readonly object _lock = new object();

        public TParticipant Current<TParticipant>()
            where TParticipant : BaseDataContext
        {
            // Console.WriteLine($"DEBUG: CURRENT: UnitOfWork: {this.GetHashCode()} -- scope: {_scope.GetHashCode()}");

            var type = typeof(TParticipant);

            lock (_lock)
            {
                if (_participants.ContainsKey(type))
                {
                    return (TParticipant)_participants[type];
                }

                var inst = _scope.Resolve<TParticipant>();

                var auditor = _scope.Resolve<Auditor>();
                var domainEvents = _scope.Resolve<DomainEvents>();

                inst.Initialize(auditor,domainEvents);

                _participants.Add(type, inst);
                return inst;
            }
        }

        void IUnitOfWork.OnScopeCommit()
        {
            // Console.WriteLine($"DEBUG: COMMIT: UnitOfWork: {this.GetHashCode()} -- scope: {_scope.GetHashCode()}");

            foreach (var db in _participants.Values)
            {
                db?.SaveChanges();
            }

            _domainEvents.HandleAllRaisedEvents(_scope);

            _participants.Clear();
        }

        void IUnitOfWork.OnScopeDispose()
        {
            // Console.WriteLine($"DEBUG: DISPOSE: UnitOfWork: {this.GetHashCode()} -- scope: {_scope.GetHashCode()}");

            _participants.Clear();
        }

        public void Dispose()
        {
        }
    }
}
