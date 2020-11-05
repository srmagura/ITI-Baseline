using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;

namespace ITI.DDD.Application.UnitOfWork
{
    public class UnitOfWorkImpl : IUnitOfWork
    {
        private readonly ILifetimeScope _scope;
        private readonly IDomainEvents _domainEvents;
        private readonly IMapper _mapper;

        public UnitOfWorkImpl(ILifetimeScope scope, IDomainEvents domainEvents, IMapper mapper)
        {
            _scope = scope;
            _domainEvents = domainEvents;
            _mapper = mapper;
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
                //var auditor = _scope.Resolve<IAuditor>();
                // TODO:SAM AUDITING
                //inst.Initialize(auditor);
                inst.Initialize(_mapper, null);

                _participants.Add(type, inst);
                return inst;
            }
        }

        public void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Raise(domainEvent);
        }

        private static bool WaitForDomainEvents = false;

        public static void ShouldWaitForDomainEvents(bool waitForDomainEvents)
        {
            WaitForDomainEvents = waitForDomainEvents;
        }

        void IUnitOfWork.OnScopeCommit()
        {
            foreach (var db in _participants.Values)
            {
                db.SaveChanges();
            }

            // HandleAllRaisedEventsAsync Will never throw exceptions
            var domainEventTask = _domainEvents.HandleAllRaisedEventsAsync();

            if (WaitForDomainEvents)
                domainEventTask.Wait();

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
            CurrentUnitOfWork = null;

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
