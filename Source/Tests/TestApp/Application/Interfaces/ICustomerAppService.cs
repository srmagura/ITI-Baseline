using ITI.Baseline.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application.Dto;
using TestApp.Domain;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application.Interfaces
{
    public interface ICustomerAppService
    {
        CustomerDto? Get(Guid id);

        Guid Add(
            string name, 
            AddressDto? address = null,
            PersonNameDto? contactName = null,
            PhoneNumberDto? contactPhone = null
        );
        void Remove(Guid id);

        void SetContact(Guid id, PersonNameDto? contactName, PhoneNumberDto? contactPhone);

        void AddLtcPharmacy(Guid id, string name);
        void RenameLtcPharmacy(Guid ltcPharmacyId, string name);
        void RemoveLtcPharmacy(Guid ltcPharmacyId);
    }
}
