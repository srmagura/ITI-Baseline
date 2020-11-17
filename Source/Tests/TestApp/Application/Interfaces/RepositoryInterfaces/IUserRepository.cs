using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain;

namespace TestApp.Application.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        void Add(User user);
    }
}
