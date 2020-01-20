using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using XPike.IoC.Microsoft.AspNetCore;
using Example.Library;

namespace XPikeIoC22
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .AddXPikeDependencyInjection(container => { container.AddSeriousQuotes(); })
                .UseStartup<Startup>();
    }
}