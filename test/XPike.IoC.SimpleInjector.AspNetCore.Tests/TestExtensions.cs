using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XPike.IoC.Tests;

namespace XPike.IoC.SimpleInjector.AspNetCore.Tests
{
    [TestClass]
    public class TestExtensions
    {
        [TestMethod]
        public void Basic_wireup()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddXPikeDependencyInjection((options)=> { });

            IServiceProvider provider = services.BuildServiceProvider();

            IApplicationBuilder app = new ApplicationBuilder(provider);

            app.UseXPikeDependencyInjection((options) => { }).Verify();
        }

        [TestMethod]
        public void Basic_Resolution()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddXPikeDependencyInjection((options) => { })
                .LoadPackage(new TestPackage());
                        
            IServiceProvider provider = services.BuildServiceProvider();

            IApplicationBuilder app = new ApplicationBuilder(provider);

            IDependencyProvider dependencyProvider = app.UseXPikeDependencyInjection((options) => { });
            dependencyProvider.Verify();

            var config = dependencyProvider.ResolveDependency<IFoo>();

            Assert.IsInstanceOfType(config, typeof(Foo));
        }
    }
}
