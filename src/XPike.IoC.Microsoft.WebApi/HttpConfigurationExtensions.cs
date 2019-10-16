using System;
using System.Web.Http;
using XPike.Extensions.DependencyInjection.WebApi;

namespace XPike.IoC.Microsoft.WebApi
{
    /// <summary>
    /// Extension methods for HttpConfiguration to support configuring SimpleInjector
    /// with xPike in ASP.Net WebAPI.
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Uses the xPike dependency injection for resolving dependencies.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="dependencyProvider">The dependency provider.</param>
        /// <returns>HttpConfiguration</returns>
        public static HttpConfiguration UseXPikeDependencyInjection(this HttpConfiguration config, IDependencyProvider dependencyProvider)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config), "Extension methods require an instance.");

            if (dependencyProvider == null)
                throw new ArgumentNullException(nameof(dependencyProvider));

            config.DependencyResolver = new MicrosoftDependencyResolver(dependencyProvider.GetContainer());
            return config;
        }
    }
}
