using ITI.Baseline.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application.Dto;
using TestApp.Domain;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application.Interfaces
{
    public interface ICustomerAppService
    {
        CustomerDto? Get(CustomerId id);

        CustomerId Add(
            string name, 
            AddressDto? address = null,
            PersonNameDto? contactName = null,
            PhoneNumberDto? contactPhone = null
        );
        void Remove(CustomerId id);

        void SetContact(CustomerId id, PersonNameDto? contactName, PhoneNumberDto? contactPhone);

        void AddLtcPharmacy(CustomerId id, string name);
        void RenameLtcPharmacy(CustomerId id, LtcPharmacyId ltcPharmacyId, string name);
        void RemoveLtcPharmacy(CustomerId id, LtcPharmacyId ltcPharmacyId);
    }
}
