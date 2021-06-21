using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoResolver
{
    public static class AutoResolver
    {
        /// <summary>
        /// Add this to the pipeline to add auto register to you project.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoResolver(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();

            var transients = GetClassImplementingInterfaces(assembly, typeof(IResolveTransient));
            var scopes = GetClassImplementingInterfaces(assembly, typeof(IResolveScoped));
            var singletons = GetClassImplementingInterfaces(assembly, typeof(IResolveSingleton));

            var transientServiceDescriptors = transients.Select(x => new ServiceDescriptor(x.TInterface, x.TClass, ServiceLifetime.Transient));
            services.TryAdd(transientServiceDescriptors);

            var scopeServiceDescriptors = scopes.Select(x => new ServiceDescriptor(x.TInterface, x.TClass, ServiceLifetime.Scoped));
            services.TryAdd(scopeServiceDescriptors);

            var singletonServiceDescriptors = singletons.Select(x => new ServiceDescriptor(x.TInterface, x.TClass, ServiceLifetime.Singleton));
            services.TryAdd(singletonServiceDescriptors);

            return services;
        }

        /// <summary>
        /// Finds the class and the interface it implements to register it with the microsofts dependency injection.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeToMatch"></param>
        /// <returns></returns>
        private static List<(Type TInterface, Type TClass)> GetClassImplementingInterfaces(Assembly assembly, Type typeToMatch)
        {
            // Get all interfaces which implements the IResolveTransient, IResolveScoped or IResolveSingleton
            var allDomainTypes = assembly.GetReferencedAssemblies()
                .Select(x => Assembly.Load(x)).SelectMany(x => x.GetTypes());
            var interfaceImplementingResolver = allDomainTypes
                .Where(p => typeToMatch.IsAssignableFrom(p) && p.IsInterface && typeToMatch.FullName != p.FullName)
                .ToList();

            // Get all classes which implements the interface that implements IResolveTransient, IResolveScoped or IResolveSingleton
            var classImplementingInterfaceThatImplementGivenType = allDomainTypes
                .Where(type => type.IsClass && !type.IsAbstract && interfaceImplementingResolver.Any(x => x.IsAssignableFrom(type)))
                .ToList();

            // Get all classes which directly implements the IResolveTransient, IResolveScoped or IResolveSingleton
            var classThatDirectlyImplmentGivenType = allDomainTypes
                .Where(x => x.IsClass &&
                !x.IsAbstract &&
                typeToMatch.IsAssignableFrom(x) &&
                !classImplementingInterfaceThatImplementGivenType.Contains(x))
                .ToList();
            // Return the class and its corresponding interface for registration.
            return classImplementingInterfaceThatImplementGivenType.Union(classThatDirectlyImplmentGivenType).Select(type =>
            {
                var interfaceImplementedByType = type.GetInterfaces().SingleOrDefault(x => interfaceImplementingResolver.Contains(x));

                // If the class does not implements any interface then register the class alone.
                if (interfaceImplementedByType == null)
                {
                    return (type, type);
                }

                return (interfaceImplementedByType, type);
            }).ToList();
        }
    }
}
