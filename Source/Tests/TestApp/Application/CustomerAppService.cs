﻿using AutoMapper;
using ITI.Baseline.Util.Validation;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.AppConfig;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.Domain;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerQueries _customerQueries;
        private readonly ICustomerRepository _customerRepo;

        public CustomerAppService(
            IUnitOfWork uow,
            ILogger logger,
            IAuthContext auth,
            IMapper mapper,
            ICustomerQueries customerQueries,
            ICustomerRepository customerRepo
        ) : base(uow, logger, auth)
        {
            _mapper = mapper;
            _customerQueries = customerQueries;
            _customerRepo = customerRepo;
        }

        public CustomerDto? Get(Guid id)
        {
            return Query(
                () => { },
                () => _customerQueries.Get(new CustomerId(id))
            );
        }

        public Guid Add(
            string name,
            AddressDto? address = null,
            PersonNameDto? contactName = null,
            PhoneNumberDto? contactPhone = null
        )
        {
            return CommandScalar(
                () => { },
                () =>
                {
                    var customer = new Customer(
                        name,
                        new List<LtcPharmacy> {
                            new LtcPharmacy("Pruitt"),
                            new LtcPharmacy("Alixa")
                        },
                        new List<int> { 1, 2 },
                        99
                    );
                    customer.SetAddress(address?.ToValueObject());
                    customer.SetContact(
                        contactName?.ToValueObject(),
                        contactPhone?.ToValueObject()
                    );

                    _customerRepo.Add(customer);
                    return customer.Id.Guid;
                }
            );
        }

        public void Remove(Guid id)
        {
            Command(
                () => { },
                () =>
                {
                    _customerRepo.Remove(new CustomerId(id));
                }
            );
        }

        public void SetContact(Guid id, PersonNameDto? contactName, PhoneNumberDto? contactPhone)
        {
            Command(
                () => { },
                () =>
                {
                    var customer = _customerRepo.Get(new CustomerId(id))
                        ?? throw new ValidationException("Customer");

                    customer.SetContact(
                        contactName?.ToValueObject(),
                        contactPhone?.ToValueObject()
                    );
                }
            );
        }

        public void AddLtcPharmacy(Guid id, string name)
        {
            Command(
                () => { },
                () =>
                {
                    var customer = _customerRepo.Get(new CustomerId(id))
                        ?? throw new ValidationException("Customer");

                    customer.AddLtcPharmacy(name);
                }
            );
        }

        public void RenameLtcPharmacy(Guid id, Guid ltcPharmacyId, string name)
        {
            Command(
                () => { },
                () =>
                {
                    var customer = _customerRepo.Get(new CustomerId(id))
                        ?? throw new ValidationException("Customer");

                    customer.RenameLtcPharmacy(new LtcPharmacyId(ltcPharmacyId), name);
                }
           );
        }

        public void RemoveLtcPharmacy(Guid id, Guid ltcPharmacyId)
        {
            Command(
                () => { },
                () =>
                {
                    var customer = _customerRepo.Get(new CustomerId(id))
                        ?? throw new ValidationException("Customer");

                    customer.RemoveLtcPharmacy(new LtcPharmacyId(ltcPharmacyId));
                }
           );
        }
    }
}
