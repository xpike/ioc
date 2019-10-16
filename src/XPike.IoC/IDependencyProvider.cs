using System;
using System.Collections.Generic;

namespace XPike.IoC
{
    /// <summary>
    /// Interface implememented by all dependency injection providers for resolving services and their dependencies.
    /// </summary>
    public interface IDependencyProvider
    {
        /// <summary>
        /// Creates a new scope for managing the lifetime of scoped instance registrations.
        /// </summary>
        /// <returns>IDisposable</returns>
        /// <example>
        /// <code>
        /// using(var scope = dependencyProvider.BeginScope())
        /// {
        ///     var foo = dependencyProvider.ResolveDependecy(typeof(IFoo));
        /// }
        /// </code>
        /// </example>
        IDisposable BeginScope();

        /// <summary>
        /// Resolves a dependency for the provided type.
        /// </summary>
        /// <typeparam name="TService">The service type to resolve.</typeparam>
        /// <returns>An instance of <typeparamref name="TService"/>.</returns>
        TService ResolveDependency<TService>() where TService : class;

        /// <summary>
        /// Resolves a dependency for the provided type.
        /// </summary>
        /// <param name="serviceType">The service type to resolve.</param>
        /// <returns>An instance of <paramref name="serviceType"/>.</returns>
        object ResolveDependency(Type serviceType);

        /// <summary>
        /// Resolves an enumerable of implementations for <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The service type to resolve.</typeparam>
        /// <returns>An enumerable of instances of <typeparamref name="TService"/>.</returns>
        IEnumerable<TService> ResolveDependencies<TService>() where TService : class;

        /// <summary>
        /// Verifies the dependency graph for completeness. 
        /// </summary>
        /// <exception cref="DependencyVerificationException">
        /// When verification fails.
        /// </exception>
        void Verify();
    }
}