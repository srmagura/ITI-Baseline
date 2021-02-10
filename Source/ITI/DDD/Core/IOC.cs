using Autofac;
using Autofac.Features.ResolveAnything;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ITI.DDD.Core
{
    // ReSharper disable once InconsistentNaming
    public class IOC
    {
        private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();
        public ContainerBuilder ContainerBuilder => _containerBuilder;
        
        private IContainer? _container;
        public IContainer Container => _container ??= _containerBuilder.Build();

        public IOC()
        {
            _containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            _containerBuilder.RegisterInstance(this);
        }

        public T Resolve<T>() where T : notnull
        {
            return Container.Resolve<T>();
        }

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder>? configurationAction = null)
        {
            if (configurationAction != null)
            {
                return Container.BeginLifetimeScope(configurationAction);
            }
            else
            {
                return Container.BeginLifetimeScope();
            }
        }

        private void EnsureNotBuilt()
        {
            if (_container != null)
                throw new Exception("Container already built:  Cannot register new types.");
        }

        public void RegisterType<TInterface, TImplementation>()
            where TInterface : notnull
            where TImplementation : notnull
        {
            EnsureNotBuilt();
            _containerBuilder.RegisterType<TImplementation>().As<TInterface>();
        }

        public void RegisterType<T>() where T : notnull
        {
            EnsureNotBuilt();
            _containerBuilder.RegisterType<T>();
        }

        public void RegisterInstance<T>(T instance)
            where T : class
        {
            EnsureNotBuilt();
            _containerBuilder.RegisterInstance(instance);
        }

        public void RegisterInstance<TInt, T>(T instance)
            where T : TInt
            where TInt : class
        {
            EnsureNotBuilt();
            _containerBuilder.RegisterInstance<TInt>(instance);
        }

        public void RegisterLifetimeScope<TInt, TImpl>()
            where TImpl : TInt
            where TInt : class
        {
            EnsureNotBuilt();
            _containerBuilder.RegisterType<TImpl>().As<TInt>().InstancePerLifetimeScope();
        }

        public void RegisterLifetimeScope<TImpl>()
            where TImpl : class
        {
            EnsureNotBuilt();
            _containerBuilder.RegisterType<TImpl>().InstancePerLifetimeScope();
        }
    }
}
