using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace XPike.IoC.SimpleInjector
{
    public class SimpleInjectorDependencyProvider : IDependencyProvider
    {
        private MethodInfo beginScope;

        internal SimpleInjectorDependencyProvider(Container container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));

            beginScope = Container.Options.DefaultScopedLifestyle.GetType()
                .GetMethod("BeginScope", BindingFlags.Static | BindingFlags.Public);
        }

        public Container Container { get; private set; }
        
        public IEnumerable<TService> ResolveDependencies<TService>() where TService : class
        {
            return Container.GetAllInstances<TService>();
        }

        public TService ResolveDependency<TService>() where TService : class
        {
            return Container.GetInstance<TService>();
        }

        public object ResolveDependency(Type serviceType)
        {
            return Container.GetInstance(serviceType);
        }

        public void Verify()
        {
            try
            {
                Container.Verify(VerificationOption.VerifyAndDiagnose);
            }
            catch(Exception ex)
            {
                throw new DependencyVerificationException(new[] { ex });
            }
        }

        public IDisposable BeginScope()
        {
            return (IDisposable)beginScope.Invoke(Container.Options.DefaultScopedLifestyle, new[] { Container });
        }
    }
}
