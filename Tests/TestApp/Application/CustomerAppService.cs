using ITI.DDD.Application;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Logging;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Application;

public class CustomerAppService : ApplicationService, ICustomerAppService
{
    private readonly ICustomerQueries _customerQueries;
    private readonly ICustomerRepository _customerRepo;

    public CustomerAppService(
        IUnitOfWorkProvider uow,
        ILogger logger,
        IAuthContext auth,
        ICustomerQueries customerQueries,
        ICustomerRepository customerRepo
    ) : base(uow, logger, auth)
    {
        _customerQueries = customerQueries;
        _customerRepo = customerRepo;
    }

    public Task<CustomerDto?> GetAsync(CustomerId id)
    {
        return QueryAsync(
            () => Task.CompletedTask,
            () => _customerQueries.GetAsync(id)
        );
    }

    public Task<CustomerId> AddAsync(
        string name,
        AddressDto? address = null,
        PersonNameDto? contactName = null,
        PhoneNumberDto? contactPhone = null
    )
    {
        return CommandAsync(
            () => Task.CompletedTask,
            () =>
            {
                var customer = new Customer(
                    name,
                    new List<LtcPharmacy>
                    {
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
                return Task.FromResult(customer.Id);
            }
        );
    }

    public Task RemoveAsync(CustomerId id)
    {
        return CommandAsync(
            () => Task.CompletedTask,
            () => _customerRepo.RemoveAsync(id)
        );
    }

    public Task SetContactAsync(CustomerId id, PersonNameDto? contactName, PhoneNumberDto? contactPhone)
    {
        return CommandAsync(
            () => Task.CompletedTask,
            async () =>
            {
                var customer = await _customerRepo.GetAsync(id)
                    ?? throw new ValidationException("Customer");

                customer.SetContact(
                    contactName?.ToValueObject(),
                    contactPhone?.ToValueObject()
                );
            }
        );
    }

    public Task AddLtcPharmacyAsync(CustomerId id, string name)
    {
        return CommandAsync(
            () => Task.CompletedTask,
            async () =>
            {
                var customer = await _customerRepo.GetAsync(id)
                    ?? throw new ValidationException("Customer");

                customer.AddLtcPharmacy(name);
            }
        );
    }

    public Task RenameLtcPharmacyAsync(CustomerId id, LtcPharmacyId ltcPharmacyId, string name)
    {
        return CommandAsync(
            () => Task.CompletedTask,
            async () =>
            {
                var customer = await _customerRepo.GetAsync(id)
                    ?? throw new ValidationException("Customer");

                customer.RenameLtcPharmacy(ltcPharmacyId, name);
            }
       );
    }

    public Task RemoveLtcPharmacyAsync(CustomerId id, LtcPharmacyId ltcPharmacyId)
    {
        return CommandAsync(
            () => Task.CompletedTask,
            async () =>
            {
                var customer = await _customerRepo.GetAsync(id)
                    ?? throw new ValidationException("Customer");

                customer.RemoveLtcPharmacy(ltcPharmacyId);
            }
       );
    }
}
