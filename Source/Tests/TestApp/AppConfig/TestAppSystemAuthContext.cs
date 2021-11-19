using ITI.DDD.Auth;

namespace TestApp.AppConfig
{
    public class TestAppSystemAuthContext : IAuthContext
    {
        public bool IsAuthenticated => true;
        public string UserIdString => "f05ddd56-4616-4a4d-8ee5-1cdc343567a3";
        public string UserName => "SYSTEM";
    }
}
