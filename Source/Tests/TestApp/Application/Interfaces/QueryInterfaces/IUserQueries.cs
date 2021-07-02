using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Application.Interfaces.QueryInterfaces
{
    public interface IUserQueries
    {
        Task<UserDto?> GetAsync(UserId id);
        Task<List<UserDto>> ListAsync();
    }
}
