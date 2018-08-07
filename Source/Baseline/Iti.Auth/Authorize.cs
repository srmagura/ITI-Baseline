namespace Iti.Auth
{
    public static class AuthContextExtensions
    {
        public static void AnyUser(this IAuthContext auth)
        {
            if( auth == null || !auth.IsAuthenticated )
                throw new NotAuthenticatedException();
        }

        public static void Require(this IAuthContext auth, bool b)
        {
            auth.AnyUser();

            if(!b)
                throw new NotAuthorizedException();
        }

        public static void Unauthenticated(this IAuthContext auth)
        {
            // do nothing
        }
    }
}