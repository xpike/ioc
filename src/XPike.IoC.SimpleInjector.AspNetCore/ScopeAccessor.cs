using System;
using Microsoft.Extensions.DependencyInjection;

namespace XPike.IoC.SimpleInjector.AspNetCore
{
    public class ScopeAccessor
    {
        public ScopeAccessor(IServiceProvider serviceProvider)
        {
            IServiceScopeFactory serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
            ServiceScope = serviceScopeFactory.CreateScope();
        }

        public IServiceScope ServiceScope { get; }
    }
}