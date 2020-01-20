using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using SimpleInjector.Lifestyles;

namespace XPike.IoC.SimpleInjector.AspNetCore
{
    public static class IApplicationBuilderExtensions
    {
        public static IDependencyProvider GetXPike(this IApplicationBuilder app) =>
            app.ApplicationServices.GetRequiredService<IDependencyProvider>();

        public static IDependencyProvider UseXPikeDependencyInjection(this IApplicationBuilder app,
            Action<SimpleInjectorUseOptions> options = null)
        {
            var serviceCollection = app.ApplicationServices.GetService<IServiceCollection>();
            var dict = new Dictionary<Type, List<ServiceDescriptor>>();
            
            foreach (var descriptor in serviceCollection)
            {
                var type = descriptor.ServiceType;

                if (!dict.ContainsKey(type))
                    dict[type] = new List<ServiceDescriptor>();

                dict[type].Add(descriptor);
            }

            if (options == null)
            {
                options = (o) => { o.UseLogging(); };
            }

            var container = app.ApplicationServices.GetRequiredService<Container>();
            container.RegisterInstance<IServiceProvider>(app.ApplicationServices);

            //IServiceProvider serviceProvider = serviceCollection.EnableSimpleInjectorCrossWiring(container);
            container.Register<ScopeAccessor>(Lifestyle.Scoped);

            foreach (var item in dict)
            {
                if (item.Value.Count > 1 && item.Value.Any(x => x.Lifetime == ServiceLifetime.Singleton))
                {
                    var services = app.ApplicationServices.GetServices(item.Key);

                    foreach (var instance in services)
                    {
                        container.Collection.AppendInstance(item.Key, instance);
                    }
                }
            }

            app.ApplicationServices.UseSimpleInjector(container, options);
            
            var provider = app.ApplicationServices.GetRequiredService<IDependencyProvider>();
            provider.Verify();

            app.Use(async (context, next) =>
            {
                using (AsyncScopedLifestyle.BeginScope(container))
                using (container.GetInstance<ScopeAccessor>().ServiceScope)
                {
                    await next();
                }
            });

            return provider;
        }
    }
}