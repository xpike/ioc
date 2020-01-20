using System;
using Microsoft.AspNetCore.Hosting;

namespace XPike.IoC.Microsoft.AspNetCore
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder AddXPikeDependencyInjection(this IWebHostBuilder webHostBuilder,
            Action<IDependencyCollection> setupCollection = null,
            Action<IDependencyProvider> setupProvider = null) =>
            webHostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                var xpikeCollection = serviceCollection.AddXPikeDependencyInjection(setupProvider);
                setupCollection?.Invoke(xpikeCollection);
            });
    }
}