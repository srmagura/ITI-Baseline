using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Application.Interfaces.QueryInterfaces
{
    public interface IFacilityQueries
    {
        Task<FacilityDto?> GetAsync(FacilityId id);
    }
}
