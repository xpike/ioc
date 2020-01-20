using XPike.IoC;

namespace Example.Library
{
    public class Package
        : IDependencyPackage
    {
        public void RegisterPackage(IDependencyCollection dependencyCollection)
        {
            dependencyCollection.RegisterSingleton<IFunnyQuoteProvider, FunnyQuoteProvider>();
            dependencyCollection.RegisterSingleton<IScaryQuoteProvider, ScaryQuoteProvider>();
            
            //dependencyCollection.RegisterSingleton<ISeriousQuoteProvider, SeriousQuoteProvider>();

            dependencyCollection.AddSingletonToCollection<IQuoteProvider, IFunnyQuoteProvider>(provider =>
             provider.ResolveDependency<IFunnyQuoteProvider>());

            dependencyCollection.AddSingletonToCollection<IQuoteProvider, IScaryQuoteProvider>(provider =>
             provider.ResolveDependency<IScaryQuoteProvider>());

            //dependencyCollection.AddSingletonToCollection<IQuoteProvider, ISeriousQuoteProvider>(provider =>
            // provider.ResolveDependency<ISeriousQuoteProvider>());
        }
    }
}