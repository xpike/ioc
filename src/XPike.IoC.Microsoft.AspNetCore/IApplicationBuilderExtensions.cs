using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace XPike.IoC.Microsoft.AspNetCore
{
    /// <summary>
    /// Extension methods for IApplicationBuilder to support xPike IoC in Asp.Net Core
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        public static IDependencyProvider GetXPike(this IApplicationBuilder app) =>
            app.ApplicationServices.GetRequiredService<IDependencyProvider>();

        /// <summary>
        /// Tells the application to use xPike dependency injection for resolution.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        /// <returns>IDependencyProvider.</returns>
        /// <exception cref="System.ArgumentNullException">app - Extension methods require an instance.</exception>
        public static IDependencyProvider UseXPikeDependencyInjection(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app), "Extension methods require an instance.");

            return app.ApplicationServices.GetService<IDependencyProvider>();
        }
    }
}
