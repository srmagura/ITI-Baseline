using ITI.DDD.Auth;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Logging;
using System.Threading.Tasks;
using TestApp.Domain.Events;

namespace TestApp.Infrastructure
{
    public class CustomerAddedDomainEventHandler : IDomainEventHandler<CustomerAddedEvent>
    {
        public static void Register()
        {
            DomainEvents.Register<CustomerAddedEvent, CustomerAddedDomainEventHandler>();
        }

        private readonly ILogger _logger;
        private readonly IAuthContext _authContext;

        public CustomerAddedDomainEventHandler(ILogger logger, IAuthContext authContext)
        {
            _logger = logger;
            _authContext = authContext;
        }

        public Task HandleAsync(CustomerAddedEvent domainEvent)
        {
            _logger.Info($"Customer added: {domainEvent.Name} (by {_authContext.UserName})");
            return Task.CompletedTask;
        }
    }
}
