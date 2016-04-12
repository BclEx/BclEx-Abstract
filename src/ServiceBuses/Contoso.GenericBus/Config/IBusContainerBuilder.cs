using System;
using System.Collections.Generic;
using System.Reflection;

namespace Contoso.GenericBus.Config
{
    /// <summary>
    /// IBusContainerBuilder
    /// </summary>
    public interface IBusContainerBuilder
    {
        /// <summary>
        /// Registers the default services.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        void RegisterDefaultServices(IEnumerable<Assembly> assemblies);
        /// <summary>
        /// Registers the bus.
        /// </summary>
        void RegisterBus();
        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        void RegisterSingleton<T>(Func<T> func)
            where T : class;
        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="func">The function.</param>
        void RegisterSingleton<T>(string name, Func<T> func)
            where T : class;
        /// <summary>
        /// Registers all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excludes">The excludes.</param>
        void RegisterAll<T>(params Type[] excludes)
            where T : class;
        /// <summary>
        /// Registers all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">The condition.</param>
        void RegisterAll<T>(Predicate<Type> condition)
            where T : class;
    }
}