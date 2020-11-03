using ITI.Baseline.ValueObjects;
using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application.Interfaces;
using TestApp.Domain;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        public CustomerAppService(IUnitOfWork uow, ILogger logger, IAuthContext auth)
            : base(uow, logger, auth)
        {

        }

        public CustomerId? Add(
            string name, 
            SimpleAddress? address = null, 
            SimplePersonName? contactName = null, 
            PhoneNumber? contactPhone = null
        )
        {
            return Command(
                () => { },
                () =>
                {
                    var customer = new Customer(name, new List<int>(), 99);
                    customer.SetAddress(address);
                    customer.SetContact(contactName, contactPhone);



                    return customer.Id;
                }
            );
        }
    }
}
