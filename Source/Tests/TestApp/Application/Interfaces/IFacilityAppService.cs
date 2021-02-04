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
    public interface IFacilityAppService
    {
        FacilityDto? Get(FacilityId id);

        FacilityId Add(string name);
        void SetContact(FacilityId id, FacilityContactDto? contact);
    }
}
