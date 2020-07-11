using System;
using System.Collections.Generic;
using Autofac;
using Iti.Core.Audit;
using Iti.Core.DomainEventsBase;
using Iti.Core.UnitOfWorkBase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.UnitOfWorkBase
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

        private readonly Dictionary<Type, DbContext> _participants = new Dictionary<Type, DbContext>();
        private readonly object _lock = new object();

        public TParticipant Current<TParticipant>()
            where TParticipant : DbContext
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
                _participants.Add(type, inst);
                return inst;
            }
        }

        void IUnitOfWork.OnScopeCommit()
        {
            // Console.WriteLine($"DEBUG: COMMIT: UnitOfWork: {this.GetHashCode()} -- scope: {_scope.GetHashCode()}");

            foreach (var db in _participants.Values)
            {
                var participant = db as IUnitOfWorkParticipant;
                participant?.OnUnitOfWorkCommit(_auditor, _domainEvents);
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
