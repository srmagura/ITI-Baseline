using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ITI.DDD.Services.DomainEventsBase
{
    public interface ITaskRunner
    {
        Task Run<TService>(string name, Action<TService> action) where TService : notnull;
    }
}
