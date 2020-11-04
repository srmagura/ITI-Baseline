using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Application.Dto
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressDto? Address { get; set; }
        public PersonNameDto? ContactName { get; set; }
        public PhoneNumberDto? ContactPhone { get; set; }
    }
}
