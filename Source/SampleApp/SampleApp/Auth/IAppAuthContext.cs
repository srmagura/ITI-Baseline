using Iti.Auth;

namespace SampleApp.Auth
{
    public interface IAppAuthContext : IAuthContext
    {
        bool HasFakeRole1 { get; }
        bool HasFakeRole2 { get; }
    }
}