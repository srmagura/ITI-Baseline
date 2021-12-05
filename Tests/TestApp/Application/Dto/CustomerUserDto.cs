using TestApp.Domain.Enums;
using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public class CustomerUserDto : UserDto
    {
        public CustomerUserDto(UserId id, CustomerId customerId) : base(id, UserType.Customer)
        {
            CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        }

        public CustomerId CustomerId { get; set; }
    }
}
