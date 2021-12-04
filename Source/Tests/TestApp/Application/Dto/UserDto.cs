using TestApp.Domain.Enums;
using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public abstract class UserDto
    {
        protected UserDto(UserId id, UserType type)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Type = type;
        }

        public UserId Id { get; set; }
        public UserType Type { get; set; }
        public EmailAddressDto? Email { get; set; }
    }
}
