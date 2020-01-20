# XPike.IoC

[![Build Status](https://dev.azure.com/xpike/xpike/_apis/build/status/xpike-ioc?branchName=master)](https://dev.azure.com/xpike/xpike/_build/latest?definitionId=2&branchName=master)
![Nuget](https://img.shields.io/nuget/v/XPike.IoC)

Provides interfaces to standardize IoC functionality across providers.

Strongly encourages the use of a Dependency Injection / Inversion of Control paradigm.

Also allows for a Service Location paradigm, by allowing the IDependencyContainer to be injected.  *This is not recommended, however, as it can introduce run-time binding errors which otherwise would have been caught using a purely-DI approach.*

## Quick Start

### .NET Core 3

In .NET Core 3, there is virtually no difference between setting up the
default Microsoft Dependency Provider for XPike, or SimpleInjector - other
than which library's extension methods you import via a `using` statement.

**`Program.cs`:**

```csharp
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using XPike.IoC.Microsoft.AspNetCore;

namespace ExampleNetCoreApp
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
                    // Register dependencies here or in Startup.cs.
                    // eg: container.AddXPikeCaching();
                },
                provider =>
                {
                    // Configure XPike here or in Startup.cs.
                    // eg: provider.UseXPikeCaching();
                });
    }
}
```

**`Startup.cs`:**

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XPike.IoC.Microsoft.AspNetCore;

namespace ExampleNetCoreApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllers();

            // Register dependencies here or in Program.cs.
            // eg: services.AddXPikeCaching()
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure XPike here or in Program.cs
            // eg: app.GetXPike().UseXPikeCaching();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
```  
    
---
##### To use SimpleInjector in .NET Core 3:

In both files, replace
```csharp
using XPike.IoC.Microsoft.AspNetCore;
```
with:
```csharp
using XPike.IoC.SimpleInjector.AspNetCore;
```

---
### .NET Core 2.2

In .NET Core 2.2, setting up the default Microsoft Dependency Provider
for XPike differs slightly from how you setup SimpleInjector.  Your best
bet is to follow these patterns precisely for a painless setup.

##### Microsoft Dependency Injection

**`Program.cs`:**

```csharp
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using XPike.IoC.Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;

namespace ExampleNetCoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .AddXPikeDependencyInjection(container =>
                   {
                       // Register dependencies here or in Startup.cs.
                       // eg: container.AddXPikeCaching();
                   },
                   provider =>
                   {
                       // Configure XPike here or in Startup.cs.
                       // eg: provider.UseXPikeCaching();
                   })
                   .UseStartup<Startup>();
    }
}
```

**`Startup.cs`:**

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XPike.IoC.Microsoft.AspNetCore;

namespace ExampleNetCoreApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register dependencies here or in Program.cs.
            // eg: services.AddXPikeCaching();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Configure XPike here or in Program.cs
            // eg: app.GetXPike().UseXPikeCaching();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
```

##### SimpleInjector Dependency Injection

**`Program.cs`:**

```csharp
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ExampleNetCoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>();
    }
}
```

**`Startup.cs`:**

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XPike.IoC.SimpleInjector.AspNetCore;

namespace ExampleNetCoreApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register IServiceCollection-friendly dependencies here.
            // eg: services.AddSomeLibrary();
            // or: services.AddXPikeEncryption();

            var container = services.AddXPikeDependencyInjection(provider => 
            {
                // Configure XPike here or in Configure()
                // eg: provider.UseXPikeCaching();
            });

            // Register XPike-only dependencies here.
            // eg: container.AddXPikeCaching();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Configure XPike here or in ConfigureServices():
            // eg: app.GetXPike().UseXPikeCaching();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
```

### .NET Framework / Owin

Getting things setup on these platforms is a little difficult right now.
Once setup is simplified and things are stabilized we'll post examples.

## Components

### IDependencyContainer

Represents the Dependency Container which holds mappings between requested services and the configured implementation(s).

**NOTE:** Behavior may vary between providers.  For example, SimpleInjector validates all registrations at startup (assuming you are not injecting IDependencyContainer itself).  It also prohibits modifying registrations after the first call to `ResolveDependency`.

### IDependencyPackage

Provides a standard mechanism for registering multiple mappings (and other Packages) into a container, to simplify DI configuration.

A recommended pattern is to have one Package which represents the entire application, by including other Packages.

### PackageLoader

An optional utility to reduce startup time by skipping any Dependency `Package` which has already been registered.

#### Usage

##### Loading a Package during initialization

Wherever your application configures the `IDependencyContainer` (for example, `Startup.cs` in ASP.NET Core), just call `IDependencyContainer.LoadPackage(new Package());`.

##### Loading a Package from another Package

```cs
using XPike.IoC;

public class Package : IDependencyPackage
{
    void RegisterPackage(IDependencyContainer container)
    {
        container.LoadPackage(new XPike.Configuration.Package());
            
        // Registrations for your library go here.
    }
}
```

## Important!

Dependency injection itself is not wired up just by including this package - this only contains the basics for standardization.

A specific Provider package for the DI mechanism of your choice (eg: SimpleInjector, Microsoft.Extensions, etc) must also be loaded.

Additionally, if you are hosting inside ASP.NET, a host-specific (eg: IIS vs. NetCore) package must be loaded to handle controller registrations.

## Building and Testing

Building from source and running unit tests requires a Windows machine with:

* .Net Core 3.0 SDK
* .Net Framework 4.6.1 Developer Pack

## Issues

Issues are tracked on [GitHub](https://github.com/xpike/xpike-ioc/issues). Anyone is welcome to file a bug,
an enhancement request, or ask a general question. We ask that bug reports include:

1. A detailed description of the problem
2. Steps to reproduce
3. Expected results
4. Actual results
5. Version of the package xPike
6. Version of the .Net runtime

## Contributing

See our [contributing guidelines](https://github.com/xpike/documentation/blob/master/docfx_project/articles/contributing.md)
in our documentation for information on how to contribute to xPike.

## License

xPike is licensed under the [MIT License](LICENSE).
