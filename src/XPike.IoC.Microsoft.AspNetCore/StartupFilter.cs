using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace XPike.IoC.Microsoft.AspNetCore
{
    public class StartupFilter
        : IStartupFilter
    {
        private readonly Action<IDependencyProvider> _configureProvider;

        public StartupFilter(Action<IDependencyProvider> configureProvider)
        {
            _configureProvider = configureProvider;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) =>
            app =>
            {
                var provider = app.UseXPikeDependencyInjection();
                _configureProvider?.Invoke(provider);
                next(app);
            };
    }
}