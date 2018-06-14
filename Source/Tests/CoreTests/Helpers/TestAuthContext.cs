using FooSampleApp.Auth;

namespace BasicTests.Helpers
{
    public class TestAuthContext : IAppAuthContext
    {
        public string UserId => "1234";
        public string UserName => "TestUser";
        public bool IsAuthenticated => true;
        public bool HasFakeRole1 => true;
        public bool HasFakeRole2 => true;
    }
}