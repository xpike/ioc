using System;

namespace XPike.IoC.Microsoft.WebApi
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
        public static IServiceProvider GetContainer(this IDependencyProvider dependencyProvider)
        {
            if (dependencyProvider == null)
                throw new ArgumentNullException(nameof(dependencyProvider), "Extension methods require an instance.");

            return ((MicrosoftDependencyProvider)dependencyProvider).ServiceProvider;
        }
    }
}
