using Contoso.GenericBus.Internal;
using System;

namespace Contoso.GenericBus.Impl
{
    /// <summary>
    /// DefaultHandler
    /// </summary>
    public class DefaultHandler : IHandler
    {
        readonly Func<object> _resolveAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="resolveAction">The resolve action.</param>
        public DefaultHandler(Type service, Type implementation, Func<object> resolveAction)
        {
            _resolveAction = resolveAction;
            Implementation = implementation;
            Service = service;
        }

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <value>
        /// The implementation.
        /// </value>
        public Type Implementation { get; private set; }
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public Type Service { get; private set; }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <returns></returns>
        public object Resolve()
        {
            return _resolveAction();
        }
    }
}