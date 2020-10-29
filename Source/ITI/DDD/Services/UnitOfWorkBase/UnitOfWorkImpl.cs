using System;
using System.Collections.Generic;
using Autofac;
using ITI.DDD.Services.DomainEventsBase;

namespace ITI.DDD.Services.UnitOfWorkBase
{
    public class UnitOfWorkImpl : IUnitOfWork
    {
        private readonly ILifetimeScope _scope;
        private readonly DomainEvents _domainEvents;

        public UnitOfWorkImpl(ILifetimeScope scope, DomainEvents domainEvents)
        {
            // Console.WriteLine($"DEBUG: CREATE: UnitOfWork: {this.GetHashCode()} -- scope: {scope.GetHashCode()}");

            _scope = scope;
            _domainEvents = domainEvents;
        }

        public IUnitOfWorkScope Begin()
        {
            // Console.WriteLine($"DEBUG: BEGIN: UnitOfWork: {this.GetHashCode()} -- scope: {_scope.GetHashCode()}");

            return new UnitOfWorkScope(this);
        }

        private readonly Dictionary<Type, IDataContext> _participants = new Dictionary<Type, IDataContext>();
        private readonly object _lock = new object();

        public TParticipant Current<TParticipant>()
            where TParticipant : IDataContext, new()
        {
            // Console.WriteLine($"DEBUG: CURRENT: UnitOfWork: {this.GetHashCode()} -- scope: {_scope.GetHashCode()}");

            var type = typeof(TParticipant);

            lock (_lock)
            {
                if (_participants.ContainsKey(type))
                {
                    return (TParticipant)_participants[type];
                }

                var inst = new TParticipant();

                var auditor = _scope.Resolve<IAuditor>();
                var domainEvents = _scope.Resolve<DomainEvents>();

                inst.Initialize(auditor, domainEvents);

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

            _domainEvents.HandleAllRaisedEvents();

            ClearParticipants();
        }

        void IUnitOfWork.OnScopeDispose()
        {
            // Console.WriteLine($"DEBUG: DISPOSE: UnitOfWork: {this.GetHashCode()} -- scope: {_scope.GetHashCode()}");

            ClearParticipants();
        }

        public void Dispose()
        {
            ClearParticipants();
        }

        private void ClearParticipants()
        {
            try
            {
                foreach (var p in _participants.Values)
                {
                    try
                    {
                        p?.Dispose();
                    }
                    catch (Exception)
                    {
                        // eat it
                    }
                }

                _participants.Clear();
            }
            catch (Exception)
            {
                // eat it
            }
        }
    }
}
