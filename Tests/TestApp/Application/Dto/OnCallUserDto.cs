using TestApp.Domain.Enums;
using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public class OnCallUserDto : UserDto
    {
        public OnCallUserDto(UserId id, OnCallProviderId onCallProviderId) : base(id, UserType.OnCall)
        {
            OnCallProviderId = onCallProviderId;
        }

        public OnCallProviderId OnCallProviderId { get; set; }
    }
}
