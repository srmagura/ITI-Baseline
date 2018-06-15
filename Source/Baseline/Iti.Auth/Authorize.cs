namespace Iti.Auth
{
    public class Authorize
    {
        public static void AnyUser(IAuthContext auth)
        {
            if (auth == null || !auth.IsAuthenticated)
                throw new NotAuthenticationException();
        }

        public static void Require(bool b)
        {
            if (!b)
                throw new NotAuthorizedException();
        }

        public static void Unauthenticated()
        {
            // do nothing... everyone is authenticated
        }
    }
}