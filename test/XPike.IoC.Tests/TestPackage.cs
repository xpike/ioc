namespace XPike.IoC.Tests
{
    public class TestPackage : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.RegisterSingleton<IFoo, Foo>();
        }
    }
}
