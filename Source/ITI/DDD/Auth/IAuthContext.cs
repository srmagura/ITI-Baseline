namespace ITI.DDD.Auth;

public interface IAuthContext
{
    bool IsAuthenticated { get; }
    string? UserIdString { get; }
    string? UserName { get; }
}
