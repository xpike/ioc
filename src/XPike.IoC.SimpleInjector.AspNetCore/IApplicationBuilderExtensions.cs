using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using System;

namespace XPike.IoC.SimpleInjector.AspNetCore
{
    public static class IApplicationBuilderExtensions
    {
        public static IDependencyProvider UseXPikeDependencyInjection(this IApplicationBuilder app, Action<SimpleInjectorUseOptions> options = null)
        {
            IDependencyCollection dependencyCollection = app.ApplicationServices.GetService<IDependencyCollection>();
            
            if(options == null)
            {
                options = (o) =>
                {
                    o.UseLogging();
                };
            }

            app.UseSimpleInjector(((SimpleInjectorDependencyCollection)dependencyCollection).Container, options);

            return dependencyCollection.BuildDependencyProvider();
        }
    }
}
