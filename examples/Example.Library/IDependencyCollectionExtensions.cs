using XPike.IoC;

namespace Example.Library
{
    public static class IDependencyCollectionExtensions
    {
        public static IDependencyCollection AddExampleLibrary(this IDependencyCollection collection) =>
            collection.LoadPackage(new Example.Library.Package());

        public static IDependencyCollection AddSeriousQuotes(this IDependencyCollection collection)
        {
            collection.RegisterSingleton<ISeriousQuoteProvider, SeriousQuoteProvider>();

            collection.AddSingletonToCollection<IQuoteProvider, ISeriousQuoteProvider>(provider =>
                provider.ResolveDependency<ISeriousQuoteProvider>());

            return collection;
        }
    }
}