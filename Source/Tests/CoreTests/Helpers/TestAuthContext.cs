using System;
using SampleApp.Auth;

namespace CoreTests.Helpers
{
    public class TestAuthContext : IAppAuthContext
    {
        public string UserId => "1234";
        public string UserName => "TestUser";

        public object AuthContextData => new Object();

        public void SetAuthContextData(object data)
        {
            // do nothing
        }

        public bool IsAuthenticated => true;
        public bool HasFakeRole1 => true;
        public bool HasFakeRole2 => true;
    }
}