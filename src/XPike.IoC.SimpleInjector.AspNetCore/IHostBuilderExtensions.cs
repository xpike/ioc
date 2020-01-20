﻿using System;
using Microsoft.Extensions.Hosting;
using SimpleInjector.Integration.ServiceCollection;

namespace XPike.IoC.SimpleInjector.AspNetCore
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder AddXPikeDependencyInjection(this IHostBuilder hostBuilder,
            Action<IDependencyCollection> setupCollection = null,
            Action<SimpleInjectorAddOptions> options = null) =>
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                var xpikeCollection = serviceCollection.AddXPikeDependencyInjection(options);
                setupCollection?.Invoke(xpikeCollection);
            });
    }
}