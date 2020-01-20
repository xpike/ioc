using System;
using Microsoft.AspNetCore.Hosting;

namespace XPike.IoC.Microsoft.AspNetCore
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder AddXPikeDependencyInjection(this IWebHostBuilder webHostBuilder,
            Action<IDependencyCollection> setupCollection = null) =>
            webHostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                var xpikeCollection = serviceCollection.AddXPikeDependencyInjection();
                setupCollection?.Invoke(xpikeCollection);
            });
    }
}