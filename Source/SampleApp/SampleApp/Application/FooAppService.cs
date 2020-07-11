using System;
using System.Collections.Generic;
using Domain;
using Domain.DomainServices;
using Iti.Baseline.Auth;
using Iti.Baseline.Core.Services;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Iti.Baseline.Logging;
using Iti.Baseline.Utilities;
using Iti.Baseline.ValueObjects;
using SampleApp.Application.Dto;
using SampleApp.Application.Interfaces;
using SampleApp.Auth;

namespace SampleApp.Application
{
    public class FooAppService : ApplicationService, IFooAppService
    {
        private readonly IAppAuthContext _auth;
        private readonly IAppPermissions _perms;
        private readonly IFooRepository _repo;
        private readonly IFooQueries _queries;
        private readonly IFooFighter _fooFighter;

        public FooAppService(IUnitOfWork uow, ILogger logger, IAppAuthContext auth, IAppPermissions perms, IFooRepository repo, IFooQueries queries, IFooFighter fooFighter)
            : base(uow, logger, auth)
        {
            _auth = auth;
            _perms = perms;
            _repo = repo;
            _queries = queries;
            _fooFighter = fooFighter;
        }

        //
        // QUERIES
        //

        public FooReferenceDto ReferenceFor(FooId id)
        {
            Authorize.AnyUser();

            try
            {
                return _queries.ReferenceFor(id);
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public FooSummaryDto SummaryFor(FooId id)
        {
            Authorize.Require(_perms.CanViewFooSummary);

            try
            {
                return _queries.SummaryFor(id);
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public FooJunkDto JunkFor(FooId id)
        {
            Authorize.Unauthenticated();

            try
            {
                return _queries.JunkFor(id);
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public FooDto Get(FooId id)
        {
            Authorize.AnyUser();

            try
            {
                return _queries.Get(id);
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public List<FooDto> GetList()
        {
            Authorize.AnyUser();

            try
            {
                return _queries.GetList();
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        //
        // OPERATIONS
        //

        public FooId CreateFoo(string name, List<Bar> bars,
            SimpleAddress simpleAddress = null,
            SimplePersonName simplePersonName = null,
            PhoneNumber phoneNumber = null)
        {
            Authorize.Require(_perms.CanManageFoos);

            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    var foo = _fooFighter.Create(name, bars, new List<int> { 1, 3, 5, 7, 9 });

                    foo.SimpleAddress = simpleAddress;
                    foo.SimplePersonName = simplePersonName;
                    foo.PhoneNumber = phoneNumber;

                    foo.ConsoleDump();

                    _repo.Add(foo);
                    uow.Commit();

                    return foo.Id;
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public void Remove(FooId id)
        {
            Authorize.Require(_perms.CanManageFoos);

            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    _repo.Remove(id);

                    uow.Commit();
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public void SetName(FooId id, string newName)
        {
            Authorize.Require(_perms.CanManageFoos);

            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    var foo = _repo.Get(id);

                    foo.SetName(newName);

                    uow.Commit();
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public void RemoveBar(FooId id, string name)
        {
            Authorize.Require(_perms.CanManageFoos);

            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    var foo = _repo.Get(id);

                    foo.RemoveBar(name);

                    uow.Commit();
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public void AddBar(FooId id, string name)
        {
            Authorize.Require(_perms.CanManageFoos);

            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    var foo = _repo.Get(id);

                    foo.AddBar(name);

                    uow.Commit();
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public void SetAllBarNames(FooId id, string name)
        {
            Authorize.Require(_perms.CanManageFoos);

            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    var foo = _repo.Get(id);

                    foo.SetAllBarNames(name);

                    uow.Commit();
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public void SetAddress(FooId id, SimpleAddress simpleAddress)
        {
            Authorize.Require(_perms.CanManageFoos);

            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    var foo = _repo.Get(id);

                    foo.SetAddress(simpleAddress);

                    uow.Commit();
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }
    }
}