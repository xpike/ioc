using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPike.IoC.SimpleInjector.AspNetCore
{
    /// <summary>
    /// Extension methods for IServiceCollection to support XPike dependency injection abscrations.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the SimpleInjector implementation of XPike dependency injection.
        /// </summary>
        /// <param name="services">This instance of IServiceCollection.</param>
        /// <param name="options">SimpleInjector options. If omitted, configures SimpleInjector for AspNetCore and controller activation.</param>
        /// <returns>An insance of <see cref="IDependencyCollection"/>.</returns>
        /// <remarks>
        /// See https://simpleinjector.readthedocs.io/en/latest/aspnetintegration.html 
        /// </remarks>
        public static IDependencyCollection AddXPikeDependencyInjection(this IServiceCollection services,
            Action<SimpleInjectorAddOptions> options = null)
        {
            var dependencyCollection = new SimpleInjectorDependencyCollection();
            var provider = new SimpleInjectorDependencyProvider(dependencyCollection.Container);

            services.AddSingleton<Container>(dependencyCollection.Container);
            services.AddSingleton<IDependencyCollection>(dependencyCollection);
            services.AddSingleton<IDependencyProvider>(provider);
            services.AddSingleton<IServiceCollection>(services);

            dependencyCollection.Container.Options.AllowOverridingRegistrations = false;

            if (options == null)
            {
                options = (o) =>
                {
                    o.AddAspNetCore()
                        .AddControllerActivation();
                };
            }

            services.AddSimpleInjector(dependencyCollection.Container, options);
            
            dependencyCollection.Container.Options.AllowOverridingRegistrations = true;

            dependencyCollection.RegisterSingleton<IDependencyCollection>(dependencyCollection);
            dependencyCollection.RegisterSingleton<IDependencyProvider>(provider);

            return dependencyCollection;
        }
    }
}