using ITI.Baseline.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application.Dto;

namespace TestApp.Application.Interfaces
{
    public interface IUserAppService
    {
        UserDto? Get(Guid id);
        List<UserDto> List();

        Guid AddCustomerUser(Guid customerId, EmailAddressDto email);
        Guid AddOnCallUser(Guid onCallProviderId, EmailAddressDto email);
    }
}
