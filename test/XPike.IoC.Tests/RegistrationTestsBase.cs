using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace XPike.IoC.Tests
{
    public abstract class RegistrationTestsBase<TLifeTime>
    {
        protected enum LifeTimes
        {
            Transient,
            Scoped,
            Singleton
        }

        protected abstract IDependencyCollection GetDependencyCollection();

        protected abstract TLifeTime GetLifetime(IDependencyCollection services, LifeTimes lifeTime);

        protected abstract TLifeTime GetRegisteredLifetime(IDependencyCollection services, Type serviceType);

        protected abstract int GetRegistrationCount(IDependencyCollection services, Type serviceType);

        protected abstract TLifeTime GetCollectionRegisteredLifetime<T>(IDependencyCollection services, Type serviceType);

        protected abstract IDependencyProvider GetScopedProvider(IDependencyProvider provider, IDisposable scope);

        #region RegisterSingleton<,>()
        [TestMethod]
        public void RegisterSingleton_generic_should_register_with_singleton_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IFoo, Foo>();

            Assert.AreEqual(GetLifetime(services, LifeTimes.Singleton), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterSingleton_generic_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IFoo, Foo>();
            services.RegisterSingleton<IFoo, Foo2>();

            var provider = services.BuildDependencyProvider();

            Assert.IsInstanceOfType(value: provider.ResolveDependency<IFoo>(), expectedType: typeof(Foo2));
        }

        [TestMethod]
        public void RegisterSingleton_generic_type_should_register_with_singleton_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IGeneric<IFoo>, Generic<IFoo>>();

            Assert.AreEqual(GetLifetime(services, LifeTimes.Singleton), GetRegisteredLifetime(services, typeof(IGeneric<IFoo>)));
        }

        [TestMethod]
        public void RegisterSingleton_generic_type_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IGeneric<IFoo>, Generic<IFoo>>();
            services.RegisterSingleton<IGeneric<IFoo>, Generic2<IFoo>>();

            var provider = services.BuildDependencyProvider();

            Assert.IsTrue(provider.ResolveDependency<IGeneric<IFoo>>() is Generic2<IFoo>);
        }

        [TestMethod]
        public void RegisterSingleton_open_generic_type_should_later_resolve_a_closed()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton(typeof(IGeneric<>), typeof(Generic<>));
            
            var provider = services.BuildDependencyProvider();
            var foo = provider.ResolveDependency<IGeneric<IFoo>>();
            Assert.IsNotNull(foo);
            Assert.IsTrue(foo is Generic<IFoo>);
        }

        [TestMethod]
        public void RegisterSingletonFallback_open_generic_type_should_not_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IGeneric<IFoo>, Generic2<IFoo>>();
            services.RegisterSingletonFallback(typeof(IGeneric<>), typeof(Generic<>));

            var provider = services.BuildDependencyProvider();

            Assert.IsTrue(provider.ResolveDependency<IGeneric<IFoo>>() is Generic2<IFoo>);
            Assert.IsTrue(provider.ResolveDependency<IGeneric<Foo2>>() is Generic<Foo2>);
        }
        #endregion

        #region RegisterSingleton<>()
        [TestMethod]
        public void RegisterSingleton_instance_should_register_with_singleton_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IFoo>(new Foo());

            Assert.AreEqual(GetLifetime(services, LifeTimes.Singleton), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterSingleton_instance_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IFoo>(new Foo());
            services.RegisterSingleton<IFoo>(new Foo2());

            var provider = services.BuildDependencyProvider();

            Assert.IsTrue(provider.ResolveDependency<IFoo>() is Foo2);
        }
        #endregion

        #region RegisterSingleton<>(Func)
        [TestMethod]
        public void RegisterSingleton_Func_should_register_with_singleton_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IFoo>((provider) => { return new Foo(); });

            Assert.AreEqual(GetLifetime(services, LifeTimes.Singleton), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterSingleton_Func_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IFoo>((provider) => { return new Foo(); });
            services.RegisterSingleton<IFoo>((provider) => { return new Foo2(); });

            var provider = services.BuildDependencyProvider();
            Assert.IsInstanceOfType(value: provider.ResolveDependency<IFoo>(), expectedType: typeof(Foo2));
        }

        [TestMethod]
        public void RegisterSingleton_Func_should_have_valid_provider()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IMyFoo, MyFoo>();
            services.RegisterSingleton<IFoo>((provider) => { return provider.ResolveDependency<IMyFoo>(); });

            var provider = services.BuildDependencyProvider();
            Assert.IsInstanceOfType(value: provider.ResolveDependency<IFoo>(), expectedType: typeof(MyFoo));
        }

        [TestMethod]
        public void RegisterSingleton_Func_should_override()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton<IFoo, Foo>();
            services.RegisterSingleton<IMyFoo, MyFoo>();
            services.RegisterSingleton<IFoo>((provider) => { return provider.ResolveDependency<IMyFoo>(); });

            var provider = services.BuildDependencyProvider();
            Assert.IsInstanceOfType(value: provider.ResolveDependency<IFoo>(), expectedType: typeof(MyFoo));
        }

        #endregion

        #region RegisterSingleton()
        [TestMethod]
        public void RegisterSingleton_should_register_with_singleton_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton(typeof(IFoo), typeof(Foo));

            Assert.AreEqual(GetLifetime(services, LifeTimes.Singleton), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterSingleton_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterSingleton(typeof(IFoo), typeof(Foo));
            services.RegisterSingleton(typeof(IFoo), typeof(Foo2));

            var provider = services.BuildDependencyProvider();

            Assert.IsInstanceOfType(value: provider.ResolveDependency<IFoo>(), expectedType: typeof(Foo2));
        }
        #endregion

        #region RegisterScoped<,>()
        [TestMethod]
        public void RegisterScoped_generic_should_register_with_scoped_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterScoped<IFoo, Foo>();

            Assert.AreEqual(GetLifetime(services, LifeTimes.Scoped), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterScoped_generic_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterScoped<IFoo, Foo>();
            services.RegisterScoped<IFoo, Foo2>();

            var provider = services.BuildDependencyProvider();

            using (var scope = provider.BeginScope())
            {
                IDependencyProvider scopedProvider = GetScopedProvider(provider, scope);
                Assert.IsInstanceOfType(value: scopedProvider.ResolveDependency<IFoo>(), expectedType: typeof(Foo2));
            }
        }
        #endregion

        #region RegisterScoped<>(Func)
        [TestMethod]
        public void RegisterScoped_Func_should_register_with_scoped_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterScoped<IFoo>(provider => { return new Foo(); });

            Assert.AreEqual(GetLifetime(services, LifeTimes.Scoped), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterScoped_Func_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterScoped<IFoo>(provider => { return new Foo(); });
            services.RegisterScoped<IFoo>(provider => { return new Foo2(); });

            var provider = services.BuildDependencyProvider();

            using (var scope = provider.BeginScope())
            {
                IDependencyProvider scopedProvider = GetScopedProvider(provider, scope);
                Assert.IsInstanceOfType(value: scopedProvider.ResolveDependency<IFoo>(), expectedType: typeof(Foo2));
            }
        }

        #endregion

        #region RegisterScoped()
        [TestMethod]
        public void RegisterScoped_should_register_with_scoped_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterScoped(typeof(IFoo), typeof(Foo));

            Assert.AreEqual(GetLifetime(services, LifeTimes.Scoped), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterScoped_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterScoped(typeof(IFoo), typeof(Foo));
            services.RegisterScoped(typeof(IFoo), typeof(Foo2));

            var provider = services.BuildDependencyProvider();

            using (var scope = provider.BeginScope())
            {
                IDependencyProvider scopedProvider = GetScopedProvider(provider, scope);
                Assert.IsInstanceOfType(value: scopedProvider.ResolveDependency<IFoo>(), expectedType: typeof(Foo2));
            }
        }
        #endregion

        #region ResisterTransient<,>()
        [TestMethod]
        public void RegisterTransient_generic_should_register_with_scoped_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterTransient<IFoo, Foo>();

            Assert.AreEqual(GetLifetime(services, LifeTimes.Transient), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterTransient_generic_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterTransient<IFoo, Foo>();
            services.RegisterTransient<IFoo, Foo2>();

            var provider = services.BuildDependencyProvider();

            Assert.IsInstanceOfType(value: provider.ResolveDependency<IFoo>(), expectedType: typeof(Foo2));
        }
        #endregion

        #region ResisterTransient<>(Func)
        [TestMethod]
        public void RegisterTransient_Func_should_register_with_scoped_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterTransient<IFoo>(provider => { return new Foo(); });

            Assert.AreEqual(GetLifetime(services, LifeTimes.Transient), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterTransient_Func_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterTransient<IFoo>(provider => { return new Foo(); });
            services.RegisterTransient<IFoo>(provider => { return new Foo2(); });

            var provider = services.BuildDependencyProvider();

            Assert.IsInstanceOfType(value: provider.ResolveDependency<IFoo>(), expectedType: typeof(Foo2));
        }
        #endregion

        #region ResisterTransient()
        [TestMethod]
        public void RegisterTransient_should_register_with_scoped_lifetime()
        {
            var services = GetDependencyCollection();

            services.RegisterTransient(typeof(IFoo), typeof(Foo));

            Assert.AreEqual(GetLifetime(services, LifeTimes.Transient), GetRegisteredLifetime(services, typeof(IFoo)));
        }

        [TestMethod]
        public void RegisterTransient_should_replace_existing_registration()
        {
            var services = GetDependencyCollection();

            services.RegisterTransient(typeof(IFoo), typeof(Foo));
            services.RegisterTransient(typeof(IFoo), typeof(Foo2));

            var provider = services.BuildDependencyProvider();

            Assert.IsInstanceOfType(value: provider.ResolveDependency<IFoo>(), expectedType: typeof(Foo2));
        }
        #endregion

        #region AddSingleton<,>()
        [TestMethod]
        public void AddSingleton_generic_should_register_with_singleton_lifetime()
        {
            var services = GetDependencyCollection();

            services.AddSingletonToCollection<IFoo, Foo>();

            Assert.AreEqual(GetLifetime(services, LifeTimes.Singleton), GetCollectionRegisteredLifetime<IFoo>(services, typeof(IFoo)));
        }

        [TestMethod]
        public void AddSingleton_generic_should_add_to_existing_collection()
        {
            var services = GetDependencyCollection();

            services.AddSingletonToCollection<IFoo, Foo>();
            services.AddSingletonToCollection<IFoo, Foo2>();

            Assert.AreEqual(2, GetRegistrationCount(services, typeof(IFoo)));
        }
        #endregion

        #region AddSingleton<>()
        [TestMethod]
        public void AddSingleton_instance_should_register_with_singleton_lifetime()
        {
            var services = GetDependencyCollection();

            services.AddSingletonToCollection<IFoo>(new Foo());

            Assert.AreEqual(GetLifetime(services, LifeTimes.Singleton), GetCollectionRegisteredLifetime<IFoo>(services, typeof(IFoo)));
        }

        [TestMethod]
        public void AddSingleton_instance_should_add_to_existing_collection()
        {
            var services = GetDependencyCollection();

            services.AddSingletonToCollection<IFoo>(new Foo());
            services.AddSingletonToCollection<IFoo>(new Foo2());

            Assert.AreEqual(2, GetRegistrationCount(services, typeof(IFoo)));
        }
        #endregion

        #region AddSingleton<>(Func)
        [TestMethod]
        public void AddSingleton_Func_should_register_with_singleton_lifetime()
        {
            var services = GetDependencyCollection();

            services.AddSingletonToCollection<IFoo, Foo2>((provider) => { return new Foo2(); });

            Assert.AreEqual(GetLifetime(services, LifeTimes.Singleton), GetCollectionRegisteredLifetime<IFoo>(services, typeof(IFoo)));
        }

        [TestMethod]
        public void AddSingleton_Func_should_add_to_existing_collection()
        {
            var services = GetDependencyCollection();

            services.AddSingletonToCollection<IFoo, Foo>((provider) => { return new Foo(); });
            services.AddSingletonToCollection<IFoo, Foo2>((provider) => { return new Foo2(); });

            Assert.AreEqual(2, GetRegistrationCount(services, typeof(IFoo)));
        }
        #endregion

        #region AddSingleton()
        [TestMethod]
        public void AddSingleton_should_register_with_singleton_lifetime()
        {
            var services = GetDependencyCollection();

            services.AddSingletonToCollection(typeof(IFoo), typeof(Foo));

            Assert.AreEqual(GetLifetime(services, LifeTimes.Singleton), GetCollectionRegisteredLifetime<IFoo>(services, typeof(IFoo)));
        }

        [TestMethod]
        public void AddSingleton_should_add_to_existing_collection()
        {
            var services = GetDependencyCollection();

            services.AddSingletonToCollection(typeof(IFoo), typeof(Foo));
            services.AddSingletonToCollection(typeof(IFoo), typeof(Foo2));

            Assert.AreEqual(2, GetRegistrationCount(services, typeof(IFoo)));
        }
        #endregion

        [TestMethod]
        public void ResetCollection_should_clear_existing_collection()
        {
            var services = GetDependencyCollection();

            services.AddSingletonToCollection(typeof(IFoo), typeof(Foo));
            services.AddSingletonToCollection(typeof(IFoo), typeof(Foo2));

            services.ResetCollection<IFoo>();

            Assert.AreEqual(0, GetRegistrationCount(services, typeof(IFoo)));
        }
    }
}
