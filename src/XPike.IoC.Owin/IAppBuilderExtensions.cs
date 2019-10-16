using Owin;
using System;

namespace XPike.IoC.Owin
{
    /// <summary>
    /// Extension methods for IAppBuilder so support xPike dependency injection.
    /// </summary>
    public static class IAppBuilderExtensions
    {
        /// <summary>
        /// Uses xPike dependency injection.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="dependencyProviderFactoryDelegate">The dependency provider factory delegate.</param>
        /// <returns>IAppBuilder.</returns>
        public static IAppBuilder UseXPikeDependencyInjection(this IAppBuilder app, Func<IDependencyProvider> dependencyProviderFactoryDelegate)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app), "Extension methods require an instance.");

            if (dependencyProviderFactoryDelegate == null)
                throw new ArgumentNullException(nameof(dependencyProviderFactoryDelegate));

            return app.UseXPikeDependencyInjection(dependencyProviderFactoryDelegate());
        }

        /// <summary>
        /// Uses xPike dependency injection.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="dependencyProvider">The dependency provider.</param>
        /// <returns>IAppBuilder.</returns>
        public static IAppBuilder UseXPikeDependencyInjection(this IAppBuilder app, IDependencyProvider dependencyProvider)
        {
            return app.Use<XPikeDependencyInjectionMiddleware>(dependencyProvider);
        }

        /// <summary>
        /// Uses xPike dependency injection.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="dependencyProvider">The dependency provider.</param>
        /// <param name="applicationPackage">The application package.</param>
        /// <returns>IAppBuilder.</returns>
        public static IAppBuilder UseXPikeDependencyInjection(this IAppBuilder app, IDependencyProvider dependencyProvider, IDependencyPackage applicationPackage)
        {
            return app.Use<XPikeDependencyInjectionMiddleware>(dependencyProvider, applicationPackage);
        }
    }
}
