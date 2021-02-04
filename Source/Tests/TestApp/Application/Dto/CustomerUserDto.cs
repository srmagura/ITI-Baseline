using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public class CustomerUserDto : UserDto
    {
        public CustomerId CustomerId { get; set; }
    }
}
