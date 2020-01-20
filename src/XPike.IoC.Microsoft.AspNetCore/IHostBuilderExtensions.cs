using System;
using Microsoft.Extensions.Hosting;

namespace XPike.IoC.Microsoft.AspNetCore
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder AddXPikeDependencyInjection(this IHostBuilder hostBuilder,
            Action<IDependencyCollection> setupCollection = null,
            Action<IDependencyProvider> setupProvider = null) =>
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                var xpikeCollection = serviceCollection.AddXPikeDependencyInjection(setupProvider);
                setupCollection?.Invoke(xpikeCollection);
            });
    }
}