using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using XPike.Extensions.DependencyInjection;

namespace XPike.IoC.Microsoft
{
    public class MicrosoftDependencyCollection : IDependencyCollection
    {
        public IServiceCollection ServiceCollection { get; }

        public MicrosoftDependencyCollection(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;

            // Add the serviceCollection itself to the container. This is requried for IDependencyProvider.Verify()
            // to work, as it needs the collection to walk the graph, validating scope and ability to resolve.
            ServiceCollection.AddServiceProviderVerification();

            // Add IDependencyProvider to the container so it can be injected/resolved for service location when needed.
            ServiceCollection.TryAddSingleton<IDependencyProvider>(provider => {
                return new MicrosoftDependencyProvider(provider);
            });
        }

        public IDependencyProvider BuildDependencyProvider(bool verifyOnBuild = true)
        {
            // Build the Microsoft container
            IServiceProvider serviceProvider = ServiceCollection.BuildServiceProvider(validateScopes: verifyOnBuild);

            // Get the instance of our wrapper...
            IDependencyProvider dependencyProvider = serviceProvider.GetService<IDependencyProvider>();

            //...and verify it
            if (verifyOnBuild)
                dependencyProvider.Verify();
            
            return dependencyProvider;
        }

        public void AddSingletonToCollection<TService, TImplementation>() where TService : class
        {
            ServiceCollection.TryAddEnumerable(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }

        public void AddSingletonToCollection<TService, TImplementation>(Func<IDependencyProvider, TService> implementationFactory) where TService : class where TImplementation : class, TService
        {
            ServiceCollection.TryAddEnumerable(
                ServiceDescriptor.Singleton<TService, TImplementation>((provider) =>
            {
                return implementationFactory(provider.GetService<IDependencyProvider>()) as TImplementation;
            }));
        }

        public void AddSingletonToCollection(Type genericInterface, Type genericImplementation) 
        {
            ServiceCollection.TryAddEnumerable(new ServiceDescriptor(genericInterface, genericImplementation, ServiceLifetime.Singleton));
        }

        public void RemoveSingleton<TService, TImplementation>()
        {
            ServiceCollection.Remove(new ServiceDescriptor(typeof(TService), typeof(TImplementation)));
        }

        public void RegisterScoped<TService, TImplementation>() where TService : class where TImplementation : class
        {
            ServiceCollection.Replace(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped));
        }

        public void RegisterScoped<TService>(Func<IDependencyProvider, TService> implementationFactory) where TService : class
        {
            ServiceCollection.Replace(new ServiceDescriptor(typeof(TService), provider => {
                return implementationFactory(provider.GetService<IDependencyProvider>());
            }, ServiceLifetime.Scoped));
        }

        public void RegisterSingleton<TService, TImplementation>() where TService : class
        {
            ServiceCollection.Replace(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }

        public void RegisterSingleton<TService>(Func<IDependencyProvider, TService> implementationFactory) where TService : class
        {
            ServiceCollection.Replace(new ServiceDescriptor(typeof(TService), 
                provider => 
                {
                    return implementationFactory(provider.GetService<IDependencyProvider>());
                }, 
                ServiceLifetime.Singleton));
        }

        public void RegisterTransient<TService, TImplementation>() where TService : class where TImplementation : class
        {
            ServiceCollection.Replace(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
        }

        public void RegisterTransient<TService>(Func<IDependencyProvider, TService> implementationFactory) where TService : class
        {
            ServiceCollection.Replace(new ServiceDescriptor(typeof(TService),
            provider =>
            {
                return implementationFactory(provider.GetService<IDependencyProvider>());
            }, ServiceLifetime.Transient));
        }

        public void RegisterSingleton(Type genericInterface, Type genericImplementation) =>
            ServiceCollection.Replace(new ServiceDescriptor(genericInterface, genericImplementation, ServiceLifetime.Singleton));

        public void RegisterSingletonFallback(Type genericInterface, Type genericImplementation) =>
            RegisterSingleton(genericInterface, genericImplementation);

        public void RegisterSingleton<TService>(TService instance) where TService : class =>
            ServiceCollection.Replace(new ServiceDescriptor(typeof(TService), instance));

        public void RegisterTransient(Type genericInterface, Type genericImplementation) =>
            ServiceCollection.Replace(new ServiceDescriptor(genericInterface, genericImplementation, ServiceLifetime.Transient));

        public void ResetCollection<TService>() =>
            ServiceCollection.RemoveAll<TService>();

        public void AddSingletonToCollection<TService>(TService instance) where TService : class
        {
            ServiceCollection.TryAddEnumerable(new ServiceDescriptor(typeof(TService), instance));
        }

        public void RegisterScoped(Type genericInterface, Type genericImplementation)
        {
            ServiceCollection.Replace(new ServiceDescriptor(genericInterface, genericImplementation, ServiceLifetime.Scoped));
        }
    }
}