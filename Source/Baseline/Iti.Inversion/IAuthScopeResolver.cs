using Autofac;

namespace Iti.Baseline.Inversion
{
    public interface IAuthScopeResolver
    {
        object GetInhertiableAuthContext();
        ILifetimeScope BeginLifetimeScope(object parentAuthInstance);
    }
}