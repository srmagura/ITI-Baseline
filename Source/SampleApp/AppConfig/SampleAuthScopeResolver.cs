using Autofac;
using Iti.Baseline.Auth;
using Iti.Baseline.Inversion;
using SampleApp.Auth;

namespace AppConfig
{
    public class SampleAuthScopeResolver : IAuthScopeResolver
    {
        private readonly ILifetimeScope _currentScope;

        public SampleAuthScopeResolver(ILifetimeScope currentScope)
        {
            _currentScope = currentScope;
        }

        public object GetInhertiableAuthContext()
        {
            var auth = _currentScope.Resolve<IAppAuthContext>();
            return new InheritedAuthContext(auth);
        }

        public ILifetimeScope BeginLifetimeScope(object childAuth)
        {
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