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
        private readonly IDomainEvents _domainEvents;
        private readonly IMapper _mapper;
        private readonly IAuditor _auditor;
        private readonly IOC _ioc;

        public UnitOfWorkImpl(
            IDomainEvents domainEvents,
            IMapper mapper,
            IAuditor auditor,
            IOC ioc
        )
        {
            _domainEvents = domainEvents;
            _mapper = mapper;
            _auditor = auditor;
            _ioc = ioc;
        }

        internal static IUnitOfWork? CurrentUnitOfWork { get; private set; }

        public IUnitOfWorkScope Begin()
        {
            CurrentUnitOfWork = this;
            return new UnitOfWorkScope(this);
        }

        private readonly Dictionary<Type, IDataContext> _participants = new Dictionary<Type, IDataContext>();
        private readonly object _lock = new object();

        public TParticipant Current<TParticipant>() where TParticipant : IDataContext
        {
            var type = typeof(TParticipant);

            lock (_lock)
            {
                if (_participants.ContainsKey(type))
                {
                    return (TParticipant)_participants[type];
                }

                var inst = _ioc.Resolve<TParticipant>();

                inst.Initialize(_mapper, _auditor);

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
                
                foreach(var domainEvent in db.GetAllDomainEvents())
                {
                    _domainEvents.Raise(domainEvent);
                }
            }

            // HandleAllRaisedEventsAsync will never throw exceptions
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
