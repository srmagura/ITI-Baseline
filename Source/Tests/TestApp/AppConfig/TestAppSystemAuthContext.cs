using ITI.DDD.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.AppConfig
{
    public class TestAppSystemAuthContext : IAuthContext
    {
        public bool IsAuthenticated => true;
        public string UserId => "f05ddd56-4616-4a4d-8ee5-1cdc343567a3";
        public string UserName => "SYSTEM";
    }
}
