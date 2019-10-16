using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace XPike.IoC.SimpleInjector.Tests
{
    [TestClass]
    public class SimpleInjectorDependencyCollection_Tests
    {
        [TestMethod]
        public void Scope_should_default_to_AsyncScopedLifestyle()
        {
            SimpleInjectorDependencyCollection collection = new SimpleInjectorDependencyCollection();

            Assert.IsInstanceOfType(collection.Container.Options.DefaultScopedLifestyle, typeof(AsyncScopedLifestyle));
        }
    }
}
