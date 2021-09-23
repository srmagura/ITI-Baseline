using AutoMapper;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure;
using ITI.DDD.Infrastructure.DataMapping;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.DataContext;
using TestApp.DataContext.DataModel;
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

        public Task<CustomerDto?> GetAsync(CustomerId id)
        {
            var q = Context.Customers
                .Where(p => p.Id == id.Guid);

            return _mapper.ProjectToDtoAsync<DbCustomer, CustomerDto>(q);
        }
    }
}
