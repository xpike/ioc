using Microsoft.Extensions.DependencyInjection;

namespace XPike.IoC.Microsoft.AspNetCore
{
    /// <summary>
    /// Extension methods for IServiceCollection to support XPike IOC in Asp.Net Core.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /* [shadow] - @jason
         * 
         * >> Regarding Bi-Directional Resolution <<
         * 
         * Suppose that I choose to create a service using XPike, ASP.NET Core, and Simple Injector.
         * I also choose to use the XPike Microsoft Extensions Logging Provider, instead of some other XPike Logging Provider (Splunk, for example).
         * I create some classes that inject an ILogger<T> to log things.
         * Then I map those classes as Singletons in a Package, which I register with XPike Dependency Injection.
         * 
         * In this case, Simple Injector needs to be able to resolve the ILogger<T>, from Microsoft DI?
         * 
         * On the flip-side...  Suppose that I choose to create a service using XPike, ASP.NET Core, and Microsoft DI.
         * In there, I want to use an XPike Configuration Provider.
         * Some generic library, also targeting ASP.NET Core, expects an ILogger<T>.
         * This generic library was mapped using an extension method it defined, adding itself to the Microsoft IServiceCollection.
         * 
         * In this case, Microsoft DI needs to be able to resolve the ILogger<T> from Simple Injector?
         * 
         * Simple Injector has an extension method - something like AutoCrossWireAspNetCore() - which might help, if you haven't already made this work.
         * It's also possible there's a way to make a "Custom DI Provider" to act as a bridge, similar to the Custom Configuration Source used by XPike.Configuration.Microsoft.AspNetCore.
         * 
         * >> Regarding use of an XPike Configuration Provider within ASP.NET Core <<
         *        
         * XPike Configuration Providers may have injected dependencies...
         * And ASP.NET Core wants the IConfiguration fully-populated *before* it 
         *
         * This can be supported when using SimpleInjector by initializing SI in a different manner:
         * - The SimpleInjectorDependencyCollection would have to be created in Program.cs and have its Package(s) loaded into it.
         * - XPike.Configuration.AspNetCore would have a UseXPikeConfiguration(IDependencyContainer) extension method.
         * - This would then Resolve the configured IConfigurationService.
         * - It would be wrapped in a MicrosoftConfigurationSource and added to the Microsoft.Extensions.Configuration providers.
         * - The SimpleInjectorDependencyProvider would have to be exposed by Program.cs on a static property so that Startup.cs can access it.
         * - Startup.cs can then call an overload of AddXPikeDependencyInjection(this IServiceCollection, XPike.IoC.IDependencyProvider).
         * It can then be used to Resolve the desired XPike Configuration Provider.
         * - This would then be passed into UseXPike
         * 
         */

        /// <summary>
        /// Adds xPike dependency injection support.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IDependencyCollection.</returns>
        public static IDependencyCollection AddXPikeDependencyInjection(this IServiceCollection services)
        {
            IDependencyCollection dependencyCollection = new MicrosoftDependencyCollection(services);
            
            return dependencyCollection;
        }
    }
}
