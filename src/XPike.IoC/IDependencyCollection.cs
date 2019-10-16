using System;
using System.Collections.Generic;

namespace XPike.IoC
{
    /// <summary>
    /// Interface implememented by all dependency injection providers for registering services.
    /// </summary>
    /// <remarks>
    /// ### Expected Implementation Behavior
    /// 
    /// xPike modules expect that the RegisterXXXX(...) methods will replace an existing registration if on already
    /// exists for the supplied service type. This allows you to override default registration with your own without
    /// having to first remove any existing registrations.
    /// </remarks>
    public interface IDependencyCollection
    {
        /// <summary>
        /// Builds the dependency provider.
        /// </summary>
        /// <param name="verifyOnBuild">if set to <c>true</c>, Verify() will be called implicitly.</param>
        /// <returns>IDependencyProvider.</returns>
        IDependencyProvider BuildDependencyProvider(bool verifyOnBuild = true);

        /// <summary>
        /// Adds a service to the container as a singleton.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation class.</typeparam>
        void RegisterSingleton<TService, TImplementation>() where TService : class;

        /// <summary>
        /// Adds a service to the container as a singleton.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam>
        /// <param name="implementationFactory">A factory delegate that creates and returns and instance of <typeparamref name="TService"/>.</param>
        void RegisterSingleton<TService>(Func<IDependencyProvider, TService> implementationFactory) where TService : class;

        /// <summary>
        /// Adds a service to the container as a singleton.
        /// </summary>
        /// <param name="interfaceType">The interface type.</param>
        /// <param name="implementationType">The implementation type.</param>
        void RegisterSingleton(Type interfaceType, Type implementationType);

        /// <summary>
        /// Adds a service to the container as a singleton.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        void RegisterSingletonFallback(Type interfaceType, Type implementationType);

        /// <summary>
        /// Adds a service to the container as a singleton.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <param name="instance">The instance.</param>
        void RegisterSingleton<TService>(TService instance) where TService : class;

        /// <summary>
        /// Adds a service to the container with a scoped lifetime.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation class.</typeparam>
        void RegisterScoped<TService, TImplementation>() where TService : class where TImplementation : class;

        /// <summary>
        /// Adds a service to the container with a scoped lifetime.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        void RegisterScoped(Type interfaceType, Type implementationType);

        /// <summary>
        /// Registers a service with a scoped lifetime.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam>
        /// <param name="implementationFactory">A factory delegate that creates and returns and instance of <typeparamref name="TService"/>.</param>
        void RegisterScoped<TService>(Func<IDependencyProvider, TService> implementationFactory) where TService : class;

        /// <summary>
        /// Adds a service to the container with a transient lifetime.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation class.</typeparam>
        void RegisterTransient<TService, TImplementation>() where TService : class where TImplementation : class;

        /// <summary>
        /// Adds a service to the container with a transient lifetime.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam>
        /// <param name="implementationFactory">A factory delegate that creates and returns and instance of <typeparamref name="TService"/>.</param>
        void RegisterTransient<TService>(Func<IDependencyProvider, TService> implementationFactory) where TService : class;

        /// <summary>
        /// Adds a service to the container with a transient lifetime.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        void RegisterTransient(Type interfaceType, Type implementationType);

        #region Collections

        /// <summary>
        /// For use when adding an implementation which will be retrieved as part of a collection.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation class.</typeparam>
        void AddSingletonToCollection<TService, TImplementation>() where TService : class;

        /// <summary>
        /// For use when adding an implementation which will be retrieved as part of a collection.
        /// </summary>
        /// <typeparam name="TService">The service type to register.</typeparam>
        /// <typeparam name="TImplementation">The implementation type the delegate will return.</typeparam>
        /// <param name="implementationFactory">A factory delegate that creates and returns and instance of <typeparamref name="TService"/>.</param>
        void AddSingletonToCollection<TService, TImplementation>(Func<IDependencyProvider, TService> implementationFactory) where TService : class where TImplementation : class, TService;

        /// <summary>
        /// For use when adding an implementation which will be retrieved as part of a collection.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        void AddSingletonToCollection(Type interfaceType, Type implementationType);

        /// <summary>
        /// For use when adding an implementation which will be retrieved as part of a collection.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        void AddSingletonToCollection<TService>(TService instance) where TService : class;

        /// <summary>
        /// Clears all registrations for type <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        void ResetCollection<TService>();

        #endregion
    }
}
