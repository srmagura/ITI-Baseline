using Iti.Core.Exceptions;

namespace FooSampleApp.Auth
{
    public class Authorize
    {
        public static void AnyUser(IAppAuthContext auth)
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