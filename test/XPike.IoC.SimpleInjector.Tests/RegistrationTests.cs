using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using XPike.IoC.Tests;

namespace XPike.IoC.SimpleInjector.Tests
{
    [TestClass]
    public class RegistrationTests : RegistrationTestsBase<Lifestyle>
    {
        protected override IDependencyCollection GetDependencyCollection()
        {
            return new SimpleInjectorDependencyCollection();
        }

        protected override Lifestyle GetLifetime(IDependencyCollection services, LifeTimes lifeTime)
        {
            switch (lifeTime)
            {
                case LifeTimes.Transient: return Lifestyle.Transient;
                case LifeTimes.Scoped: return ((SimpleInjectorDependencyCollection)services).Container.Options.DefaultScopedLifestyle;
                case LifeTimes.Singleton: return Lifestyle.Singleton;
            }
            throw new ArgumentException("Invalid Lifetime");
        }

        protected override Lifestyle GetRegisteredLifetime(IDependencyCollection services, Type serviceType)
        {
            try
            {
                services.BuildDependencyProvider();
            }
            catch { }
            return ((SimpleInjectorDependencyCollection)services).Container.GetCurrentRegistrations().First(p=>p.ServiceType == serviceType).Lifestyle;
        }

        protected override int GetRegistrationCount(IDependencyCollection services, Type serviceType)
        {
            try
            {
                services.BuildDependencyProvider();
            }
            catch { }
            return ((SimpleInjectorDependencyCollection)services).Container.GetAllInstances(serviceType).Count();
        }

        protected override Lifestyle GetCollectionRegisteredLifetime<T>(IDependencyCollection services, Type serviceType)
        {
            Container container = ((SimpleInjectorDependencyCollection)services).Container;
            try
            {
                container.Verify();
            }
            catch { }
            return container.GetCurrentRegistrations().First(r => r.ServiceType == typeof(IEnumerable<T>)).Lifestyle;
        }

        protected override IDependencyProvider GetScopedProvider(IDependencyProvider provider, IDisposable scope)
        {
            return provider;
        }
    }
}
