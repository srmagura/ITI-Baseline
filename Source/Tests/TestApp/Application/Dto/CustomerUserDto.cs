using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Application.Dto
{
    public class CustomerUserDto : UserDto
    {
        public Guid CustomerId { get; set; }
    }
}
