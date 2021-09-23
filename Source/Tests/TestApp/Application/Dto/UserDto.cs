using TestApp.Domain.Enums;
using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public abstract class UserDto
    {
        public UserId Id { get; set; }
        public UserType Type { get; set; }
        public EmailAddressDto? Email { get; set; }
    }
}
