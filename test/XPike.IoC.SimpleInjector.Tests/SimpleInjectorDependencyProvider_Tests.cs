using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace XPike.IoC.SimpleInjector.Tests
{
    [TestClass]
    public class SimpleInjectorDependencyProvider_Tests
    {
        [TestMethod]
        public void BeginScope_should_return_IDisposable()
        {
            IDependencyCollection collection = new SimpleInjectorDependencyCollection();
            IDependencyProvider provider = collection.BuildDependencyProvider();

            using (var scope = provider.BeginScope())
            {
                Assert.IsNotNull(scope);
            }
        }
    }
}
