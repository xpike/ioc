using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using XPike.IoC.SimpleInjector.AspNetCore;
using Example.Library;

namespace XPikeIoC3SI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .AddXPikeDependencyInjection(container =>
                {
                    container.AddSeriousQuotes();
                });
    }
}