using System;
using System.Collections.Generic;
using System.Linq;
using ITI.DDD.Application;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ITI.Baseline.Audit
{
    public class Auditor : IAuditor
    {
        private readonly ILogger _logger;
        private readonly IAuthContext _auth;

        public Auditor(ILogger logger, IAuthContext auth)
        {
            _logger = logger;
            _auth = auth;
        }

        public void Process(DbContext context)
        {
            
        }
    }
}
