using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Application.Dto
{
    public class OnCallUserDto : UserDto
    {
        public Guid OnCallProviderId { get; set; }
    }
}
