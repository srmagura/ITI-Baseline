namespace FooSampleApp.Auth
{
    public class AppPermissions : IAppPermissions
    {
        private readonly IAppAuthContext _auth;

        public AppPermissions(IAppAuthContext auth)
        {
            _auth = auth;
        }

        public bool CanViewFooSummary => _auth.HasFakeRole1 || _auth.HasFakeRole1;
        public bool CanManageFoos => _auth.HasFakeRole1 || _auth.HasFakeRole2;
    }
}