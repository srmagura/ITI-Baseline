using Autofac;
using Autofac.Features.ResolveAnything;
using System;

namespace ITI.DDD.Core
{
    // ReSharper disable once InconsistentNaming
    public class IOC
    {
        private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();

        private IContainer? _container;
        public IContainer Container => _container ??= _containerBuilder.Build();

        private static IOC? Static;

        public IOC()
        {
            Static = this;
            _containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
        }

        public T ResolveForTest<T>() where T : notnull
        {
            return Container.Resolve<T>();
        }

        public static T ResolveStaticUseSparingly<T>() where T : notnull
        {
            if (Static == null)
                throw new Exception("Static IOC instance has not been initialized.");

            return Static.Container.Resolve<T>();
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return Container.BeginLifetimeScope();
        }

        public void RegisterType<TInterface, TImplementation>()
            where TInterface : notnull
            where TImplementation : notnull
        {
            _containerBuilder.RegisterType<TImplementation>().As<TInterface>();
        }

        public void RegisterType<T>() where T : notnull
        {
            _containerBuilder.RegisterType<T>();
        }

        public void RegisterInstance<T>(T instance)
            where T : class
        {
            _containerBuilder.RegisterInstance(instance);
        }

        public void RegisterInstance<TInt, T>(T instance)
            where T : TInt
            where TInt : class
        {
            _containerBuilder.RegisterInstance<TInt>(instance);
        }

        public void RegisterLifetimeScope<TInt, TImpl>()
            where TImpl : TInt
            where TInt : class
        {
            _containerBuilder.RegisterType<TImpl>().As<TInt>().InstancePerLifetimeScope();
        }

        public void RegisterLifetimeScope<TImpl>()
            where TImpl : class
        {
            _containerBuilder.RegisterType<TImpl>().InstancePerLifetimeScope();
        }
    }
}
