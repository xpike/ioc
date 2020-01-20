using Microsoft.Extensions.DependencyInjection;
using XPike.IoC.Microsoft;

namespace Example.Library.AspNetCore
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddExampleLibrary(this IServiceCollection collection)
        {
            new MicrosoftDependencyCollection(collection, false, false).AddExampleLibrary();
            return collection;
        }
    }
}