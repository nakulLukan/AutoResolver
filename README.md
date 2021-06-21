# AutoResolver
AutoResolver is a simple library built to resolve services bit more easily. Any class or interface can implement the following interfaces to register their class and resolve them through service provider.
- `IResolveTransient` to register the class with lifetime as transient.
- `IResolveScoped` to register the class with lifetime as scoped.
- `IResolveSingleton` to register the class with lifetime as singleton.

### How do I get started?
First add the AutoResolver to the service collection.
```csharp
...
services.AddAutoResolver();
services.AddMvc();
services.AddJwtTokenAuthentication();
....
```
OR
```csharp
var serviceProvider = new ServiceCollection()
                .AddAutoResolver()
                .BuildServiceProvider();
```

Then implement the desired interface to register them with `Microsoft.Extensions.DependencyInjection`.
```csharp
using AutoResolver;

namespace AutoResolverTest
{
    public interface IThisAsTransient : IResolveTransient
    {
        string GetMessage();
    }
}
...

namespace AutoResolverTest
{
    public class ThisAsTransient : IThisAsTransient
    {
        public string GetMessage()
        {
            return "Hi from resolved service (trainsient)";
        }
    }
}
```
Now you are done. You can now use the service provider (or inject) to resolve the service. 
```csharp
var myResolvedClass = serviceProvider.GetRequiredService<IThisAsTransient>();
Console.WriteLine($"Text from resolved class: {myResolvedClass.GetMessage()}");
```
