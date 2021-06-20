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

            var scopeServiceDescriptors = scopes.Select(x => new ServiceDescriptor(x.TInterface, x.TClass, ServiceLifetime.Transient));
            services.TryAdd(scopeServiceDescriptors);

            var singletonServiceDescriptors = singletons.Select(x => new ServiceDescriptor(x.TInterface, x.TClass, ServiceLifetime.Transient));
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
            var interfaceImplementingResolver = assembly.ExportedTypes
                .Where(type =>
                {
                    return type.GetInterfaces().Contains(typeToMatch) && type.IsInterface;
                }).ToList();

            // Get all classes which implements the IResolveTransient, IResolveScoped or IResolveSingleton
            var classImplementingResolver = assembly.ExportedTypes
                .Where(type =>
                {
                    return type.GetInterfaces().Contains(typeToMatch) && type.IsClass;
                }).ToList();

            // Return the class and its corresponding interface for registration.
            return classImplementingResolver.Select(type =>
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
