namespace ITI.DDD.Auth;

public static class AuthContextExtensions
{
    // Parameterless extension methods should return Task.CompletedTask so that you
    // don't have to do this:
    //
    // return QueryAsync(
    //     () =>
    //     {
    //         Authorize.AnyUser();
    //         return Task.CompletedTask;
    //     },
    //     () => { /* ... */ }
    // );

    public static Task AnyUser(this IAuthContext auth)
    {
        if (!auth.IsAuthenticated)
            throw new NotAuthorizedException();

        return Task.CompletedTask;
    }

    public static void Require(this IAuthContext auth, bool b)
    {
        if (!auth.IsAuthenticated)
            throw new NotAuthorizedException();

        if (!b) throw new NotAuthorizedException();
    }

    public static Task Unauthenticated(this IAuthContext _)
    {
        // do nothing
        return Task.CompletedTask;
    }

    // This is to indicate that authorization is performed in the application service method body
    // (usually done for performance & keeping IAppPermissions simple)
    public static Task AuthorizedBelow(this IAuthContext _)
    {
        return Task.CompletedTask;
    }
}
