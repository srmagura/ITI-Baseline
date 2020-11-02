using System;
using System.Collections.Generic;
using Autofac;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;

namespace ITI.DDD.Application.UnitOfWorkBase
{
    public class UnitOfWorkImpl : IUnitOfWork
    {
        private readonly ILifetimeScope _scope;
        private readonly DomainEvents _domainEvents;

        public UnitOfWorkImpl(ILifetimeScope scope, DomainEvents domainEvents)
        {
            _scope = scope;
            _domainEvents = domainEvents;
        }

        internal static IUnitOfWork? CurrentUnitOfWork { get; private set; }

        public IUnitOfWorkScope Begin()
        {
            CurrentUnitOfWork = this;
            return new UnitOfWorkScope(this);
        }

        private readonly Dictionary<Type, IDataContext> _participants = new Dictionary<Type, IDataContext>();
        private readonly object _lock = new object();

        public TParticipant Current<TParticipant>()
            where TParticipant : IDataContext, new()
        {
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

        public void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Raise(domainEvent);
        }

        void IUnitOfWork.OnScopeCommit()
        {
            foreach (var db in _participants.Values)
            {
                db?.SaveChanges();
            }

            _domainEvents.HandleAllRaisedEventsAsync();

            ClearParticipants();
        }

        void IUnitOfWork.OnScopeDispose()
        {
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
