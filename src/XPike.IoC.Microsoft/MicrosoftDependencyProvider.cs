using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using XPike.Extensions.DependencyInjection;

namespace XPike.IoC.Microsoft
{
    public class MicrosoftDependencyProvider : IDependencyProvider
    {
        public IServiceProvider ServiceProvider { get; }

        public MicrosoftDependencyProvider(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public IEnumerable<TService> ResolveDependencies<TService>() where TService : class
        {
            return ServiceProvider.GetServices<TService>();
        }

        public TService ResolveDependency<TService>() where TService : class
        {
            return ServiceProvider.GetService<TService>();
        }

        public object ResolveDependency(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }

        /// <summary>
        /// Verifies the dependency graph for completeness. 
        /// </summary>
        /// <exception cref="DependencyVerificationException">
        /// When verification fails.
        /// </exception>
        public void Verify()
        {
            try
            {
                ServiceProvider.Verify();
            }
            catch(Exception ex)
            {
                throw new DependencyVerificationException(new[] { ex });
            }
        }

        public IDisposable BeginScope()
        {
            return ServiceProvider.CreateScope();
        }
    }
}
