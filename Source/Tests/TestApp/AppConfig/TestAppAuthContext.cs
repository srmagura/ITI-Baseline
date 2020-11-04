using ITI.DDD.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.AppConfig
{
    public class TestAppAuthContext : IAuthContext
    {
        public bool IsAuthenticated => true;
        public string UserId => "e05ddd56-4616-4a4d-8ee5-1cdc343567a3";
        public string UserName => "Automation User";
    }
}
