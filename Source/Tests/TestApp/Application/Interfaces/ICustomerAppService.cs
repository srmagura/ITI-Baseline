using ITI.Baseline.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Domain;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application.Interfaces
{
    public interface ICustomerAppService
    {
        Task<CustomerDto?> GetAsync(CustomerId id);

        Task<CustomerId> AddAsync(
            string name, 
            AddressDto? address = null,
            PersonNameDto? contactName = null,
            PhoneNumberDto? contactPhone = null
        );
        Task RemoveAsync(CustomerId id);

        Task SetContactAsync(CustomerId id, PersonNameDto? contactName, PhoneNumberDto? contactPhone);

        Task AddLtcPharmacyAsync(CustomerId id, string name);
        Task RenameLtcPharmacyAsync(CustomerId id, LtcPharmacyId ltcPharmacyId, string name);
        Task RemoveLtcPharmacyAsync(CustomerId id, LtcPharmacyId ltcPharmacyId);
    }
}
