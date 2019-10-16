using Microsoft.Owin;
using System;
using System.Threading.Tasks;

namespace XPike.IoC.Owin
{
    /// <summary>
    /// OWIN Middleware for using XPike Dependency Injection with OWIN hosted services.
    /// Implements the <see cref="Microsoft.Owin.OwinMiddleware" />
    /// </summary>
    /// <seealso cref="Microsoft.Owin.OwinMiddleware" />
    public class XPikeDependencyInjectionMiddleware : OwinMiddleware
    {
        private IDependencyProvider dependencyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="XPikeDependencyInjectionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="dependencyProvider">The dependency provider instance.</param>
        public XPikeDependencyInjectionMiddleware(OwinMiddleware next, IDependencyProvider dependencyProvider) : base(next)
        {
            this.dependencyProvider = dependencyProvider ?? throw new ArgumentNullException(nameof(dependencyProvider));
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context">The context.</param>
        public override async Task Invoke(IOwinContext context)
        {
            using(var requestScope = dependencyProvider.BeginScope())
            {
                await Next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}
