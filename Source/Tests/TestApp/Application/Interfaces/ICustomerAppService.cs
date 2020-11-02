using Iti.Baseline.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain;

namespace TestApp.Application.Interfaces
{
    interface ICustomerAppService
    {
        CustomerId Add(
            string name, 
            //List<Bar> bars,
            SimpleAddress address = null,
            SimplePersonName contactName = null,
            PhoneNumber contactPhone = null
        );
    }
}
