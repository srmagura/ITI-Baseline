namespace ITI.DDD.Auth
{
    public static class AuthContextExtensions
    {
        public static void AnyUser(this IAuthContext auth)
        {
            if (!auth.IsAuthenticated)
                throw new NotAuthorizedException();
        }

        public static void Require(this IAuthContext auth, bool b)
        {
            auth.AnyUser();

            if (!b) throw new NotAuthorizedException();
        }

        public static void Unauthenticated(this IAuthContext _)
        {
            // do nothing
        }
    }
}