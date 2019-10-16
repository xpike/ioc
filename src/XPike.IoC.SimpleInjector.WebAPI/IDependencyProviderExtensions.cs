using SimpleInjector;
using System;

namespace XPike.IoC.SimpleInjector
{
    /// <summary>
    /// Extension methods for HttpConfiguration to support configuring SimpleInjector
    /// with xPike in ASP.Net WebAPI.
    /// </summary>
    public static class IDependencyProviderExtensions
    {
        /// <summary>
        /// Gets the underlying SimpleInjector container.
        /// </summary>
        /// <param name="dependencyProvider">The dependency provider.</param>
        /// <returns>Container.</returns>
        public static Container GetContainer(this IDependencyProvider dependencyProvider)
        {
            if (dependencyProvider == null)
                throw new ArgumentNullException(nameof(dependencyProvider), "Extension methods require an instance.");

            return ((SimpleInjectorDependencyProvider)dependencyProvider).Container;
        }
    }
}
