using ITI.Baseline.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Domain.Identities;

namespace TestApp.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<UserDto?> GetAsync(UserId id);
        Task<List<UserDto>> ListAsync();

        Task<UserId> AddCustomerUserAsync(CustomerId customerId, EmailAddressDto email);
        Task<UserId> AddOnCallUserAsync(OnCallProviderId onCallProviderId, EmailAddressDto email);
    }
}
