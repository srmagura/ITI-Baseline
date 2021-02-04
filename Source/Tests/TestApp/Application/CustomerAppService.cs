using AutoMapper;
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
        private readonly ICustomerQueries _customerQueries;
        private readonly ICustomerRepository _customerRepo;

        public CustomerAppService(
            IUnitOfWork uow,
            ILogger logger,
            IAuthContext auth,
            ICustomerQueries customerQueries,
            ICustomerRepository customerRepo
        ) : base(uow, logger, auth)
        {
            _customerQueries = customerQueries;
            _customerRepo = customerRepo;
        }

        public CustomerDto? Get(CustomerId id)
        {
            return Query(
                () => { },
                () => _customerQueries.Get(id)
            );
        }

        public CustomerId Add(
            string name,
            AddressDto? address = null,
            PersonNameDto? contactName = null,
            PhoneNumberDto? contactPhone = null
        )
        {
            return Command(
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
                    return customer.Id;
                }
            );
        }

        public void Remove(CustomerId id)
        {
            Command(
                () => { },
                () =>
                {
                    _customerRepo.Remove(id);
                }
            );
        }

        public void SetContact(CustomerId id, PersonNameDto? contactName, PhoneNumberDto? contactPhone)
        {
            Command(
                () => { },
                () =>
                {
                    var customer = _customerRepo.Get(id)
                        ?? throw new ValidationException("Customer");

                    customer.SetContact(
                        contactName?.ToValueObject(),
                        contactPhone?.ToValueObject()
                    );
                }
            );
        }

        public void AddLtcPharmacy(CustomerId id, string name)
        {
            Command(
                () => { },
                () =>
                {
                    var customer = _customerRepo.Get(id)
                        ?? throw new ValidationException("Customer");

                    customer.AddLtcPharmacy(name);
                }
            );
        }

        public void RenameLtcPharmacy(CustomerId id, LtcPharmacyId ltcPharmacyId, string name)
        {
            Command(
                () => { },
                () =>
                {
                    var customer = _customerRepo.Get(id)
                        ?? throw new ValidationException("Customer");

                    customer.RenameLtcPharmacy(ltcPharmacyId, name);
                }
           );
        }

        public void RemoveLtcPharmacy(CustomerId id, LtcPharmacyId ltcPharmacyId)
        {
            Command(
                () => { },
                () =>
                {
                    var customer = _customerRepo.Get(id)
                        ?? throw new ValidationException("Customer");

                    customer.RemoveLtcPharmacy(ltcPharmacyId);
                }
           );
        }
    }
}
