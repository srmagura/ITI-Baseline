using System;
using Autofac;
using Autofac.Features.ResolveAnything;

namespace Iti.Inversion
{
    // ReSharper disable once InconsistentNaming
    public static class IOC
    {
        public static ContainerBuilder ContainerBuilder = new ContainerBuilder();

        private static IContainer _container;

        public static IContainer Container => _container ?? (_container = ContainerBuilder.Build());

        public static void Initialize()
        {
            ContainerBuilder = new ContainerBuilder();
            ContainerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            _container = null;
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static object TryResolveByType(Type type)
        {
            Container.TryResolve(type, out var inst);
            return inst;
        }

        public static bool TryResolve<T>(out T result)
        {
            return Container.TryResolve(out result);
        }

        public static T TryResolve<T>()
            where T : class
        {
            return Container.TryResolve<T>(out var result) ? result : null;
        }

        public static bool IsRegistered<T>()
        {
            return Container?.IsRegistered<T>() ?? false;
        }

        public static void RegisterType<TInterface, TImplementation>()
        {
            if (_container != null)
                throw new Exception("Container has already been built. You shouldn't register any more types.");

            ContainerBuilder.RegisterType<TImplementation>().As<TInterface>();
        }

        public static void RegisterType<T>()
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

    }
}
