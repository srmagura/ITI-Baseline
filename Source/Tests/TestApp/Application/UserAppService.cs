using AutoMapper;
using ITI.Baseline.Util.Validation;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.Domain;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IMapper _mapper;
        private readonly IUserQueries _userQueries;
        private readonly IUserRepository _userRepo;

        public UserAppService(
            IUnitOfWork uow, 
            ILogger logger, 
            IAuthContext auth,
            IMapper mapper,
            IUserQueries userQueries,
            IUserRepository userRepo
        ) : base(uow, logger, auth)
        {
            _mapper = mapper;
            _userQueries = userQueries;
            _userRepo = userRepo;
        }

        public UserDto? Get(Guid id)
        {
            return Query(
                () => { },
                () => _userQueries.Get(new UserId(id))
            );
        }

        public List<UserDto> List()
        {
            return Query(
                () => { },
                () => _userQueries.List()
            ) ?? new List<UserDto>();
        }

        public Guid AddCustomerUser(Guid customerId, EmailAddressDto email)
        {
            return CommandScalar(
                () => { },
                () =>
                {
                    var customerUser = new CustomerUser(
                        new CustomerId(customerId),
                        _mapper.Map<EmailAddress>(email)
                    );

                    _userRepo.Add(customerUser);
                    return customerUser.Id.Guid;
                }
            );
        }

        public Guid AddOnCallUser(Guid onCallProviderId, EmailAddressDto email)
        {
            return CommandScalar(
                () => { },
                () =>
                {
                    var onCallUser = new OnCallUser(
                        new OnCallProviderId(onCallProviderId),
                        _mapper.Map<EmailAddress>(email)
                    );

                    _userRepo.Add(onCallUser);
                    return onCallUser.Id.Guid;
                }
            );
        }
    }
}
