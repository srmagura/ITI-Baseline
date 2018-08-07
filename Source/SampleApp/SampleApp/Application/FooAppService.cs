using System;
using System.Collections.Generic;
using Domain;
using Domain.DomainServices;
using Iti.Auth;
using Iti.Core.Services;
using Iti.Core.UnitOfWork;
using Iti.Core.UserTracker;
using Iti.Inversion;
using Iti.ValueObjects;
using SampleApp.Application.Dto;
using SampleApp.Application.Interfaces;
using SampleApp.Auth;

namespace SampleApp.Application
{
    public class FooAppService : ApplicationService, IFooAppService, IUserTracking
    {
        private readonly IAppAuthContext _auth;
        private readonly IAppPermissions _perms;
        private readonly IFooRepository _repo;
        private readonly IFooQueries _queries;

        public FooAppService(IAppAuthContext auth, IAppPermissions perms, IFooRepository repo, IFooQueries queries)
            : base(auth)
        {
            _auth = auth;
            _perms = perms;
            _repo = repo;
            _queries = queries;
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

        public FooId CreateFoo(string name, List<Bar> bars)
        {
            Authorize.Require(_perms.CanManageFoos);

            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    var ff = IOC.Resolve<IFooFighter>();
                    var foo = ff.Create(name, bars, new List<int> { 1, 3, 5, 7, 9 });

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

        public void SetAddress(FooId id, Address address)
        {
            Authorize.Require(_perms.CanManageFoos);

            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    var foo = _repo.Get(id);

                    foo.SetAddress(address);

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