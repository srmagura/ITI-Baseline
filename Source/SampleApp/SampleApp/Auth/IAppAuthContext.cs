using Iti.Auth;

namespace FooSampleApp.Auth
{
    public interface IAppAuthContext : IAuthContext
    {
        bool IsAuthenticated { get; }

        bool HasFakeRole1 { get; }
        bool HasFakeRole2 { get; }
    }
}