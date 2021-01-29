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
using TestApp.DataContext.DataModel;
using TestApp.Domain;
using TestApp.Domain.Identities;

namespace TestApp.Queries
{
    public class EfCustomerQueries : Queries<AppDataContext>, ICustomerQueries
    {
        private readonly IMapper _mapper;

        public EfCustomerQueries(IUnitOfWork uow, IMapper mapper) : base(uow)
        {
            _mapper = mapper;
        }

        public CustomerDto? Get(CustomerId id)
        {
            var q = Context.Customers
                .Where(p => p.Id == id.Guid);

            return _mapper.ProjectToDto<DbCustomer, CustomerDto>(q);
        }
    }
}
