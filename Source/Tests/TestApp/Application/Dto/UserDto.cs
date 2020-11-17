using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Enums;

namespace TestApp.Application.Dto
{
    public abstract class UserDto
    {
        public Guid Id { get; set; }
        public UserType Type { get; set; }
        public EmailAddressDto? Email { get; set; }
    }
}
