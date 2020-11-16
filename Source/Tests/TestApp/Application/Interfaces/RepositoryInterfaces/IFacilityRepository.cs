using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain;

namespace TestApp.Application.Interfaces.RepositoryInterfaces
{
    public interface IFacilityRepository
    {
        Facility Get(FacilityId id);

        void Add(Facility facility);
        void Remove(FacilityId id);
    }
}
