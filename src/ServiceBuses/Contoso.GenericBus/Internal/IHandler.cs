using System;

namespace Contoso.GenericBus.Internal
{
    /// <summary>
    /// IHandler
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        Type Service { get; }
        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <value>
        /// The implementation.
        /// </value>
        Type Implementation { get; }
        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <returns></returns>
        object Resolve();
    }
}