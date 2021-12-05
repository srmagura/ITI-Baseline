using ITI.DDD.Auth;

namespace TestApp.AppConfig;

internal class SystemAuthContext : IAuthContext
{
    public bool IsAuthenticated => true;
    public string? UserIdString => null;
    public string UserName => "SYSTEM";
}
