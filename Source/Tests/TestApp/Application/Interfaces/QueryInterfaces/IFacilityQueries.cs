using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application.Dto;
using TestApp.Domain;

namespace TestApp.Application.Interfaces.QueryInterfaces
{
    public interface IFacilityQueries
    {
        FacilityDto? Get(FacilityId id);
    }
}
