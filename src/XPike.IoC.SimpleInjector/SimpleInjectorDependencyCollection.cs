using System;
using System.Collections.Generic;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace XPike.IoC.SimpleInjector
{
    public class SimpleInjectorDependencyCollection : IDependencyCollection
    {
        public SimpleInjectorDependencyCollection()
        {
            Container = new Container();
            Container.Options.AllowOverridingRegistrations = true;
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        }

        /// <summary>
        /// Gets the underlying SimpleInjector container instance.
        /// </summary>
        /// <value>The container.</value>
        public Container Container { get; }

        public IDependencyProvider BuildDependencyProvider(bool verifyOnBuild = true)
        {
            // Add IDependencyProvider to the container so it can be injected/resolved for service location when needed.
            Container.Options.AllowOverridingRegistrations = true;

            Container.RegisterSingleton<IDependencyProvider>(() => 
            {
                return new SimpleInjectorDependencyProvider(Container);
            });

            // Verify the SimpleInjector container
            if (verifyOnBuild)
                Container.Verify(VerificationOption.VerifyAndDiagnose);

            return Container.GetInstance<IDependencyProvider>();
        }

        public void AddSingletonToCollection<TService, TImplementation>() where TService : class
        {
            Container.Collection.Append(typeof(TService), typeof(TImplementation), Lifestyle.Singleton);
        }

        public void AddSingletonToCollection<TService, TImplementation>(Func<IDependencyProvider, TService> implementationFactory) where TService : class where TImplementation : class, TService
        {
            Container.Collection.Append<TService>(
                ()=> 
                {
                    return implementationFactory(Container.GetInstance<IDependencyProvider>());
                }, Lifestyle.Singleton);
        }

        public void AddSingletonToCollection(Type genericInterface, Type genericImplementation) =>
            Container.Collection.Append(genericInterface, genericImplementation);

        public void RegisterScoped<TService, TImplementation>() where TService : class where TImplementation : class
        {
            Container.Register(typeof(TService), typeof(TImplementation), Lifestyle.Scoped);
        }

        public void RegisterScoped<TService>(Func<IDependencyProvider, TService> implementationFactory) where TService : class
        {
            Container.Register(typeof(TService), () =>
            {
                return implementationFactory(Container.GetInstance<IDependencyProvider>());
            }, Lifestyle.Scoped);
        }

        public void RegisterSingleton<TService, TImplementation>() where TService : class
        {
            Container.RegisterSingleton(typeof(TService), typeof(TImplementation));
        }

        public void RegisterSingleton<TService>(Func<IDependencyProvider, TService> implementationFactory) where TService : class
        {
            Container.RegisterSingleton(typeof(TService), 
                () => 
                {
                    return implementationFactory(Container.GetInstance<IDependencyProvider>());
                });
        }

        public void RegisterTransient<TService, TImplementation>() where TService : class where TImplementation : class
        {
            Container.Register(typeof(TService), typeof(TImplementation), Lifestyle.Transient);
        }

        public void RegisterTransient<TService>(Func<IDependencyProvider, TService> implementationFactory) where TService : class
        {
            Container.Register(typeof(TService),
                () =>
                {
                    return implementationFactory(Container.GetInstance<IDependencyProvider>());
                }, Lifestyle.Transient);
        }

        public void RegisterSingleton(Type genericInterface, Type genericImplementation) =>
            Container.RegisterSingleton(genericInterface, genericImplementation);

        public void RegisterSingletonFallback(Type genericInterface, Type genericImplementation)
        {
            Container.Options.AllowOverridingRegistrations = false;
            Container.RegisterConditional(genericInterface, genericImplementation, Lifestyle.Singleton, c => !c.Handled);
            Container.Options.AllowOverridingRegistrations = true;
        }

        public void RegisterSingleton<TService>(TService instance) where TService : class =>
            Container.RegisterInstance(typeof(TService), instance);

        public void RegisterTransient(Type genericInterface, Type genericImplementation) =>
            Container.Register(genericInterface, genericImplementation);


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations.", Justification = "Array.Empty is not supported in net452")]
        public void ResetCollection<TService>() =>
            Container.Collection.Register(typeof(TService), new TService[0]);

        public void AddSingletonToCollection<TService>(TService instance) where TService : class
        {
            Container.Collection.AppendInstance(instance);
        }

        public void RegisterScoped(Type genericInterface, Type genericImplementation)
        {
            Container.Register(genericInterface, genericImplementation, Lifestyle.Scoped);
        }
    }
}
