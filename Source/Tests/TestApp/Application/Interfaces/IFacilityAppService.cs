using ITI.Baseline.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application.Dto;
using TestApp.Domain;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application.Interfaces
{
    public interface IFacilityAppService
    {
        FacilityDto? Get(Guid id);

        Guid Add(string name);
        void SetContact(Guid id, FacilityContactDto? contact);
    }
}
