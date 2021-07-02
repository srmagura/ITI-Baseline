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
    public interface IFacilityAppService
    {
        Task<FacilityDto?> GetAsync(FacilityId id);

        Task<FacilityId> AddAsync(string name);
        Task SetContactAsync(FacilityId id, FacilityContactDto? contact);
    }
}
