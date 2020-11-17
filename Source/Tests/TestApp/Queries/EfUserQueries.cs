using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure;
using ITI.DDD.Infrastructure.DataMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.DataContext;
using TestApp.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Queries
{
    public class EfUserQueries : Queries<AppDataContext>, IUserQueries
    {
        private readonly IMapper _mapper;

        public EfUserQueries(IUnitOfWork uow, IMapper mapper) : base(uow)
        {
            _mapper = mapper;
        }

        public UserDto? Get(UserId id)
        {
            var q = Context.Users!
                .Where(u => u.Id == id.Guid);

            // Can't use projection because of inheritance
            var user = q.FirstOrDefault();
            return _mapper.Map<UserDto>(user);
        }

        public List<UserDto> List()
        {
            // Can't use projection because of inheritance
            var users = Context.Users.ToList();
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
