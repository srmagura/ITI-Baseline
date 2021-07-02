using AutoMapper;
using AutoMapper.QueryableExtensions;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure;
using ITI.DDD.Infrastructure.DataMapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<UserDto?> GetAsync(UserId id)
        {
            var q = Context.Users!
                .Where(u => u.Id == id.Guid);

            // Can't use projection because of inheritance
            var user = await q.FirstOrDefaultAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> ListAsync()
        {
            // Can't use projection because of inheritance
            var users = await Context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
