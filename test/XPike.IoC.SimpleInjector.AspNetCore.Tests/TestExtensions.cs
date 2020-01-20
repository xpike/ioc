using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using XPike.IoC.Microsoft;
using XPike.IoC.Tests;

namespace XPike.IoC.SimpleInjector.AspNetCore.Tests
{
    [TestClass]
    public class TestExtensions
    {
        [TestCleanup]
        public void Cleanup()
        {
            IDependencyCollectionExtensions.ResetPackages();
        }

        [TestMethod]
        public void Basic_wireup()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMvcCore();

            services.AddXPikeDependencyInjection((options) => { });

            IServiceProvider provider = services.BuildServiceProvider();

            IApplicationBuilder app = new ApplicationBuilder(provider);

            app.UseXPikeDependencyInjection((options) => { }).Verify();
        }

        [TestMethod]
        public void Basic_Resolution()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMvcCore();

            services.AddXPikeDependencyInjection((options) => { })
                .LoadPackage(new TestPackage());

            IServiceProvider provider = services.BuildServiceProvider();

            IApplicationBuilder app = new ApplicationBuilder(provider);

            IDependencyProvider dependencyProvider = app.UseXPikeDependencyInjection((options) => { });
            dependencyProvider.Verify();

            var config = dependencyProvider.ResolveDependency<IFoo>();

            Assert.IsInstanceOfType(config, typeof(Foo));
        }

        [TestMethod]
        public void Chained_Resolution()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMvcCore();

            var xpikeProvider = services.AddXPikeDependencyInjection((options) => { });
            xpikeProvider.LoadPackage(new TestPackage());
            xpikeProvider.RegisterSingleton<ISpecificInterface, SpecificImplementation>();
            xpikeProvider.RegisterSingleton<IBaseInterface>(provider => provider.ResolveDependency<ISpecificInterface>());

            IServiceProvider provider = services.BuildServiceProvider();

            IApplicationBuilder app = new ApplicationBuilder(provider);

            IDependencyProvider dependencyProvider = app.UseXPikeDependencyInjection((options) => { });
            dependencyProvider.Verify();

            var config = dependencyProvider.ResolveDependency<IFoo>();

            Assert.IsInstanceOfType(config, typeof(Foo));

            var specific = dependencyProvider.ResolveDependency<IBaseInterface>();
            Assert.IsInstanceOfType(specific, typeof(SpecificImplementation));
        }

        [TestMethod]
        public void Chained_Resolution_Collection()
        {
            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddMvcCore();

                var xpikeProvider = services.AddXPikeDependencyInjection((options) => { });
                xpikeProvider.LoadPackage(new TestPackage());
                xpikeProvider.RegisterSingleton<ISpecificInterface, SpecificImplementation>();
                xpikeProvider.RegisterSingleton<ISpecificInterface2, SpecificImplementation2>();
                xpikeProvider.AddSingletonToCollection<IBaseInterface, ISpecificInterface>(provider =>
                    provider.ResolveDependency<ISpecificInterface>());
                xpikeProvider.AddSingletonToCollection<IBaseInterface, ISpecificInterface2>(provider =>
                    provider.ResolveDependency<ISpecificInterface2>());

                IServiceProvider provider = services.BuildServiceProvider();

                IApplicationBuilder app = new ApplicationBuilder(provider);

                IDependencyProvider dependencyProvider = app.UseXPikeDependencyInjection((options) => { });
                dependencyProvider.Verify();

                var config = dependencyProvider.ResolveDependency<IFoo>();

                Assert.IsInstanceOfType(config, typeof(Foo));

                var specific = dependencyProvider.ResolveDependencies<IBaseInterface>().ToList();
                Assert.AreEqual(specific.Count, 2);

                Assert.IsInstanceOfType(specific.First(), typeof(SpecificImplementation));
                Assert.IsInstanceOfType(specific[1], typeof(SpecificImplementation2));

                specific = dependencyProvider.ResolveDependency<IEnumerable<IBaseInterface>>().ToList();
                Assert.AreEqual(specific.Count, 2);

                Assert.IsInstanceOfType(specific.First(), typeof(SpecificImplementation));
                Assert.IsInstanceOfType(specific[1], typeof(SpecificImplementation2));
            }
            catch (DependencyVerificationException dve)
            {
                Console.WriteLine($"{dve.Message}: {dve.GetType()}");

                foreach (var ex in dve.Exceptions)
                    Console.WriteLine($"{ex.Message}: {ex.GetType()}");

                throw;
            }
        }

        private void AssertInstance<TService, TImplementation>(IServiceProvider provider)
        {
            var instance = provider.GetService<TService>();
            Assert.IsNotNull(instance);
            Console.WriteLine($"{typeof(TService).Name}: {instance.GetType()}");
            Assert.IsInstanceOfType(instance, typeof(TImplementation));
        }

        [TestMethod]
        public void Chained_Resolution_Collection_ShimRegistration()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMvcCore();
            services.AddLogging();

            var xpikeProvider = (IDependencyCollection)new MicrosoftDependencyCollection(services, false, false);
            xpikeProvider.RegisterSingleton<ISpecificInterface, SpecificImplementation>();
            xpikeProvider.RegisterSingleton<ISpecificInterface2, SpecificImplementation2>();
            xpikeProvider.AddSingletonToCollection<IBaseInterface, ISpecificInterface>(provider =>
                provider.ResolveDependency<ISpecificInterface>());
            xpikeProvider.AddSingletonToCollection<IBaseInterface, ISpecificInterface2>(provider =>
                provider.ResolveDependency<ISpecificInterface2>());

            xpikeProvider = services.AddXPikeDependencyInjection();
            xpikeProvider.LoadPackage(new TestPackage());
            xpikeProvider.RegisterSingleton<ISpecificInterface3, SpecificImplementation3>();
            xpikeProvider.AddSingletonToCollection<IBaseInterface, ISpecificInterface3>(provider =>
                provider.ResolveDependency<ISpecificInterface3>());

            IServiceProvider provider = services.BuildServiceProvider();
            Console.WriteLine($"Root {nameof(IServiceProvider)}: {provider.GetType()}");

            var svcs = provider.GetService<IServiceProvider>();
            Assert.IsNotNull(svcs);
            Console.WriteLine($"Inner {nameof(IServiceProvider)}: {svcs.GetType()}");

            AssertInstance<IDependencyCollection, SimpleInjectorDependencyCollection>(provider);
            AssertInstance<IDependencyProvider, SimpleInjectorDependencyProvider>(provider);
            AssertInstance<MicrosoftDependencyProvider, MicrosoftDependencyProvider>(provider);
            AssertInstance<ISpecificInterface, SpecificImplementation>(provider);
            AssertInstance<ISpecificInterface2, SpecificImplementation2>(provider);

            AssertInstance<IDependencyCollection, SimpleInjectorDependencyCollection>(svcs);
            AssertInstance<IDependencyProvider, SimpleInjectorDependencyProvider>(svcs);
            AssertInstance<MicrosoftDependencyProvider, MicrosoftDependencyProvider>(svcs);
            AssertInstance<ISpecificInterface, SpecificImplementation>(svcs);
            AssertInstance<ISpecificInterface2, SpecificImplementation2>(svcs);

            var tmp = provider.GetServices<IBaseInterface>().ToList();
            Assert.AreEqual(2, tmp.Count);
            Assert.IsInstanceOfType(tmp[0], typeof(SpecificImplementation));
            Assert.IsInstanceOfType(tmp[1], typeof(SpecificImplementation2));

            tmp = svcs.GetServices<IBaseInterface>().ToList();
            Assert.AreEqual(2, tmp.Count);
            Assert.IsInstanceOfType(tmp[0], typeof(SpecificImplementation));
            Assert.IsInstanceOfType(tmp[1], typeof(SpecificImplementation2));

            IApplicationBuilder app = new ApplicationBuilder(provider);

            IDependencyProvider dependencyProvider = app.UseXPikeDependencyInjection();
            Console.WriteLine($"{nameof(IDependencyProvider)}: {dependencyProvider.GetType()}");
            dependencyProvider.Verify();

            Console.WriteLine(dependencyProvider.ResolveDependency<IServiceProvider>().GetType());
            Console.WriteLine(dependencyProvider.ResolveDependency<IDependencyCollection>().GetType());
            Console.WriteLine(dependencyProvider.ResolveDependency<IDependencyProvider>().GetType());
            Console.WriteLine(dependencyProvider.ResolveDependency<MicrosoftDependencyProvider>().GetType());
            Console.WriteLine(dependencyProvider.ResolveDependency<ISpecificInterface>().GetType());
            Console.WriteLine(dependencyProvider.ResolveDependency<ISpecificInterface2>().GetType());
            Console.WriteLine(dependencyProvider.ResolveDependency<IBaseInterface>().GetType());

            var impls = dependencyProvider.ResolveDependency<MicrosoftDependencyProvider>()
                .ResolveDependencies<IBaseInterface>().ToList();
            Assert.IsNotNull(impls);
            Assert.AreEqual(2, impls.Count);
            Assert.IsInstanceOfType(impls[0], typeof(SpecificImplementation));
            Assert.IsInstanceOfType(impls[1], typeof(SpecificImplementation2));

            var config = dependencyProvider.ResolveDependency<IFoo>();

            Assert.IsInstanceOfType(config, typeof(Foo));

            var item = dependencyProvider.ResolveDependency<IBaseInterface>();
            Console.WriteLine(item.GetType());
            var specific = dependencyProvider.ResolveDependencies<IBaseInterface>().ToList();
            Assert.AreEqual(specific.Count, 3);

            Assert.IsInstanceOfType(specific[0], typeof(SpecificImplementation3));
            Assert.IsInstanceOfType(specific[1], typeof(SpecificImplementation));
            Assert.IsInstanceOfType(specific[2], typeof(SpecificImplementation2));

            specific = dependencyProvider.ResolveDependency<IEnumerable<IBaseInterface>>().ToList();
            Assert.AreEqual(specific.Count, 3);

            Assert.IsInstanceOfType(specific[0], typeof(SpecificImplementation3));
            Assert.IsInstanceOfType(specific[1], typeof(SpecificImplementation));
            Assert.IsInstanceOfType(specific[2], typeof(SpecificImplementation2));
        }
    }
}