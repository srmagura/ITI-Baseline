using System;
using Autofac;
using Autofac.Features.ResolveAnything;

namespace Iti.Baseline.Inversion
{
    // ReSharper disable once InconsistentNaming
    public static class IOC
    {
        public static ContainerBuilder ContainerBuilder = new ContainerBuilder();

        private static IContainer? _container;

        public static IContainer Container => _container ??= ContainerBuilder.Build();

        public static void Initialize()
        {
            ContainerBuilder = new ContainerBuilder();

            ContainerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            _container = null;
        }

        public static T ResolveForTest<T>() where T : notnull
        {
            return Container.Resolve<T>();
        }
      
        public static ILifetimeScope BeginLifetimeScope()
        {
            return Container.BeginLifetimeScope();
        }

        //
        //

        public static void RegisterType<TInterface, TImplementation>() 
            where TInterface: notnull
            where TImplementation : notnull
        {
            if (_container != null)
                throw new Exception("Container has already been built. You shouldn't register any more types.");

            ContainerBuilder.RegisterType<TImplementation>().As<TInterface>();
        }

        public static void RegisterType<T>() where T: notnull
        {
            if (_container != null)
                throw new Exception("Container has already been built. You shouldn't register any more types.");

            ContainerBuilder.RegisterType<T>();
        }

        public static void RegisterInstance<T>(T instance)
            where T : class
        {
            if (_container != null)
                throw new Exception("Container has already been built. You shouldn't register any more types.");

            ContainerBuilder.RegisterInstance(instance);
        }

        public static void RegisterInstance<TInt, T>(T instance)
            where T : TInt
            where TInt : class
        {
            if (_container != null)
                throw new Exception("Container has already been built. You shouldn't register any more types.");

            ContainerBuilder.RegisterInstance<TInt>(instance);
        }

        public static void RegisterLifetimeScope<TInt, TImpl>()
            where TImpl : TInt
            where TInt : class
        {
            ContainerBuilder.RegisterType<TImpl>().As<TInt>().InstancePerLifetimeScope();
        }

        public static void RegisterLifetimeScope<TImpl>()
            where TImpl : class
        {
            ContainerBuilder.RegisterType<TImpl>().InstancePerLifetimeScope();
        }
    }
}
