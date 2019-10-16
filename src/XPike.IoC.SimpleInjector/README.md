# XCore.IoC.SimpleInjector

Uses SimpleInjector for Dependency Injection within XPike.

## Notes

- XPike configures Simple Injector to allow replacement of previously registered dependencies.
  - Fallback registration methods such as `RegisterSingletonFallback()` do not support this.

### Known Issues

- `[Inject]` attribute for property injection / constructor selection not yet implemented.  
  See for details: https://simpleinjector.readthedocs.io/en/latest/extensibility.html#overriding-constructor-resolution-behavior
- ASP.NET Core's `[FromServices]` used for injection into controller method parameters **is not supported** when using SimpleInjector as your DI container.
