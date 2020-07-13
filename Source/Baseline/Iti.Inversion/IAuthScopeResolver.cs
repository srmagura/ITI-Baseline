using Autofac;

namespace Iti.Baseline.Inversion
{
    public interface IAuthScopeResolver
    {
        ILifetimeScope BeginLifetimeScope();
    }
}