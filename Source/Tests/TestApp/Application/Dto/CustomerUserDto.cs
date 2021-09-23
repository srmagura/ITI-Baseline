using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public class CustomerUserDto : UserDto
    {
        public CustomerId CustomerId { get; set; }
    }
}
