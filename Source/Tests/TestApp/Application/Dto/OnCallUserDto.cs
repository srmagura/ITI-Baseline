using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public class OnCallUserDto : UserDto
    {
        public OnCallProviderId OnCallProviderId { get; set; }
    }
}
