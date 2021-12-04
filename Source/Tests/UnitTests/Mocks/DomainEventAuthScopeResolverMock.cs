using Autofac;

namespace UnitTests.Mocks
{
    internal class DomainEventAuthScopeResolverMock // TODO:SAM : IDomainEventAuthScopeResolver
    {
        private readonly ILifetimeScope _scope;

        public DomainEventAuthScopeResolverMock(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return _scope.BeginLifetimeScope();
        }
    }

}
