using AutoResolver;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AutoResolverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var serviceProvider = new ServiceCollection()
                .AddAutoResolver()
                .BuildServiceProvider();

            var myResolvedClass = serviceProvider.GetRequiredService<IResolveAsTransient>();
            var singletonClass = serviceProvider.GetRequiredService<ResolveAsSingleton>();
            Console.WriteLine($"Text from resolved class: {myResolvedClass.GetMessage()}");
            Console.WriteLine($"Text from singleton resolved class: {singletonClass.SingletonValue}");
        }
    }
}
