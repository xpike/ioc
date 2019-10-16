using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Web.Http;

namespace XPike.IoC.SimpleInjector
{
    /// <summary>
    /// Extension methods for IDependencyCollection to support configuring SimpleInjector
    /// with xPike in ASP.Net WebAPI.
    /// </summary>
    public static class IDependencyCollectionExtensions
    {
        /// <summary>
        /// Gets the underlying SimpleInjector container.
        /// </summary>
        /// <param name="dependencyCollection">The dependency collection.</param>
        /// <returns>Container</returns>
        public static Container GetContainer(this IDependencyCollection dependencyCollection)
        {
            if (dependencyCollection == null)
                throw new ArgumentNullException(nameof(dependencyCollection), "Extension methods require an instance.");

            return ((SimpleInjectorDependencyCollection)dependencyCollection).Container;
        }

        /// <summary>
        /// Configures the underlying SimpleInjector container for use with ASP.Net WebAPI.
        /// </summary>
        /// <param name="dependencyCollection">The dependency collection.</param>
        /// <param name="httpConfiguration">The HTTP configuration.</param>
        /// <returns>IDependencyCollection.</returns>
        public static IDependencyCollection Configure(this IDependencyCollection dependencyCollection, HttpConfiguration httpConfiguration)
        {
            if (dependencyCollection == null)
                throw new ArgumentNullException(nameof(dependencyCollection), "Extension methods require an instance.");

            if (httpConfiguration == null)
                throw new ArgumentNullException(nameof(httpConfiguration));

            Container container = dependencyCollection.GetContainer();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            container.RegisterWebApiControllers(httpConfiguration);

            return dependencyCollection;
        }
    }
}
