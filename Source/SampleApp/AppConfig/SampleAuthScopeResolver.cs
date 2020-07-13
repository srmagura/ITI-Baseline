using Autofac;
using Iti.Baseline.Auth;
using Iti.Baseline.Inversion;
using SampleApp.Auth;

namespace AppConfig
{
    public class SampleAuthScopeResolver : IAuthScopeResolver
    {
        private readonly ILifetimeScope _scope;

        public SampleAuthScopeResolver(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            _scope.TryResolve<IAppAuthContext>(out var parentAuth);
            var childAuth = new InheritedAuthContext(parentAuth);

            return IOC.Container.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(childAuth)
                    .As<IAppAuthContext>()
                    .As<IAuthContext>()
                    .SingleInstance();
            });
        }
    }
}