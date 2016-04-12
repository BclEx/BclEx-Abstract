using System;
using System.Collections.Generic;

namespace Contoso.GenericBus.Internal
{
    /// <summary>
    /// IServiceLocator
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();
        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        object Resolve(Type type);
        /// <summary>
        /// Determines whether this instance can resolve the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        bool CanResolve(Type type);
        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();
        /// <summary>
        /// Gets all handlers for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IEnumerable<IHandler> GetAllHandlersFor(Type type);
        /// <summary>
        /// Releases the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Release(object item);
    }
}