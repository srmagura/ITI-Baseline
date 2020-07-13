using SampleApp.Auth;

namespace AppConfig
{
    public class InheritedAuthContext : IAppAuthContext
    {
        public InheritedAuthContext(IAppAuthContext parentAuth)
        {
            if (parentAuth == null)
                return;

            IsAuthenticated = parentAuth.IsAuthenticated;
            UserId = parentAuth.UserId;
            UserName = parentAuth.UserName;

            HasFakeRole1 = parentAuth.HasFakeRole1;
            HasFakeRole2 = parentAuth.HasFakeRole2;
        }

        public bool IsAuthenticated { get; }
        public string UserId { get; }
        public string UserName { get; }

        public bool HasFakeRole1 { get; }
        public bool HasFakeRole2 { get; }
    }
}