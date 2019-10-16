using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;
using XPike.Extensions.DependencyInjection;
using XPike.IoC.Tests;

namespace XPike.IoC.Microsoft.Tests
{
    [TestClass]
    public class RegistrationTests : RegistrationTestsBase<ServiceLifetime>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            FieldInfo isVerified = typeof(IServiceProviderExtensions)
                .GetField("isVerified", BindingFlags.NonPublic | BindingFlags.Static);

            isVerified.SetValue(typeof(IServiceProviderExtensions), false);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            FieldInfo isVerified = typeof(IServiceProviderExtensions)
                .GetField("isVerified", BindingFlags.NonPublic | BindingFlags.Static);

            isVerified.SetValue(typeof(IServiceProviderExtensions), false);
        }

        protected override IDependencyCollection GetDependencyCollection()
        {
            return new MicrosoftDependencyCollection(new ServiceCollection());
        }

        protected override ServiceLifetime GetLifetime(IDependencyCollection services, LifeTimes lifeTime)
        {
            switch (lifeTime)
            {
                case LifeTimes.Transient: return ServiceLifetime.Transient;
                case LifeTimes.Scoped: return ServiceLifetime.Scoped;
                case LifeTimes.Singleton: return ServiceLifetime.Singleton;
            }
            throw new ArgumentException("Invalid Lifetime");
        }

        protected override ServiceLifetime GetRegisteredLifetime(IDependencyCollection services, Type serviceType)
        {
            return ((MicrosoftDependencyCollection)services).ServiceCollection.First(d => d.ServiceType == serviceType).Lifetime;
        }

        protected override int GetRegistrationCount(IDependencyCollection services, Type serviceType)
        {
            return ((MicrosoftDependencyCollection)services).ServiceCollection.Count(d => d.ServiceType == serviceType);
        }

        protected override ServiceLifetime GetCollectionRegisteredLifetime<T>(IDependencyCollection services, Type serviceType)
        {
            return ((MicrosoftDependencyCollection)services).ServiceCollection.First(d => d.ServiceType == serviceType).Lifetime;
        }

        protected override IDependencyProvider GetScopedProvider(IDependencyProvider provider, IDisposable scope)
        {
            var serviceScope = (IServiceScope)scope;
            return new MicrosoftDependencyProvider(serviceScope.ServiceProvider);
        }
    }
}
