using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPike.Extensions.DependencyInjection;
using XPike.IoC.Tests;

namespace XPike.IoC.Microsoft.AspNetCore.Tests
{
    [TestClass]
    public class TestExtensions
    {
        [TestCleanup]
        public void Cleanup()
        {
            IDependencyCollectionExtensions.ResetPackages();
            typeof(IServiceProviderExtensions).GetField("isVerified", BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, false);
        }

        [TestMethod]
        public void Chained_Resolution()
        {
            IServiceCollection services = new ServiceCollection();

            var xpikeProvider = services.AddXPikeDependencyInjection();
            xpikeProvider.LoadPackage(new TestPackage());
            xpikeProvider.RegisterSingleton<ISpecificInterface, SpecificImplementation>();
            xpikeProvider.RegisterSingleton<IBaseInterface>(provider => provider.ResolveDependency<ISpecificInterface>());

            IServiceProvider provider = services.BuildServiceProvider();

            IApplicationBuilder app = new ApplicationBuilder(provider);

            IDependencyProvider dependencyProvider = app.UseXPikeDependencyInjection();
            dependencyProvider.Verify();

            var config = dependencyProvider.ResolveDependency<IFoo>();

            Assert.IsInstanceOfType(config, typeof(Foo));

            var specific = dependencyProvider.ResolveDependency<IBaseInterface>();
            Assert.IsInstanceOfType(specific, typeof(SpecificImplementation));
        }

        [TestMethod]
        public void Chained_Resolution_Collection()
        {
            IServiceCollection services = new ServiceCollection();

            var xpikeProvider = services.AddXPikeDependencyInjection();
            xpikeProvider.LoadPackage(new TestPackage());
            xpikeProvider.RegisterSingleton<ISpecificInterface, SpecificImplementation>();
            xpikeProvider.AddSingletonToCollection<IBaseInterface, ISpecificInterface>(provider => provider.ResolveDependency<ISpecificInterface>());

            IServiceProvider provider = services.BuildServiceProvider();

            IApplicationBuilder app = new ApplicationBuilder(provider);

            IDependencyProvider dependencyProvider = app.UseXPikeDependencyInjection();
            dependencyProvider.Verify();

            var config = dependencyProvider.ResolveDependency<IFoo>();

            Assert.IsInstanceOfType(config, typeof(Foo));

            var specific = dependencyProvider.ResolveDependencies<IBaseInterface>();
            Assert.IsTrue(specific?.Any() ?? false);

            Assert.IsInstanceOfType(specific.First(), typeof(SpecificImplementation));
        }

        [TestMethod]
        public void Chained_Resolution_Collection_ShimRegistration()
        {
            try
            {
                IServiceCollection services = new ServiceCollection();

                var xpikeProvider = (IDependencyCollection) new MicrosoftDependencyCollection(services, false, false);
                xpikeProvider.RegisterSingleton<ISpecificInterface, SpecificImplementation>();
                xpikeProvider.RegisterSingleton<ISpecificInterface2, SpecificImplementation2>();
                xpikeProvider.AddSingletonToCollection<IBaseInterface, ISpecificInterface>(provider =>
                    provider.ResolveDependency<ISpecificInterface>());
                xpikeProvider.AddSingletonToCollection<IBaseInterface, ISpecificInterface2>(provider =>
                    provider.ResolveDependency<ISpecificInterface2>());

                xpikeProvider = services.AddXPikeDependencyInjection();
                xpikeProvider.LoadPackage(new TestPackage());

                IServiceProvider provider = services.BuildServiceProvider();

                IApplicationBuilder app = new ApplicationBuilder(provider);

                IDependencyProvider dependencyProvider = app.UseXPikeDependencyInjection();
                dependencyProvider.Verify();

                var config = dependencyProvider.ResolveDependency<IFoo>();

                Assert.IsInstanceOfType(config, typeof(Foo));

                var specific = dependencyProvider.ResolveDependency<IEnumerable<IBaseInterface>>().ToList();
                Assert.AreEqual(specific.Count, 2);

                Assert.IsInstanceOfType(specific.First(), typeof(SpecificImplementation));
                Assert.IsInstanceOfType(specific[1], typeof(SpecificImplementation2));

                specific = dependencyProvider.ResolveDependencies<IBaseInterface>().ToList();
                Assert.AreEqual(specific.Count, 2);

                Assert.IsInstanceOfType(specific.First(), typeof(SpecificImplementation));
                Assert.IsInstanceOfType(specific[1], typeof(SpecificImplementation2));
            }
            catch (DependencyVerificationException dve)
            {
                foreach (var ex in dve.Exceptions)
                    if (ex is ServiceProviderVerificationException spve)
                        foreach(var res in spve.Restults)
                            Console.WriteLine($"Verification Result: {res.Message} ({res.Exception.GetType().Name}: {res.Exception.Message})");
                    else
                        Console.WriteLine($"Verification Exception: {ex.Message} ({ex.GetType().Name})");

                throw;
            }
        }
    }
}
