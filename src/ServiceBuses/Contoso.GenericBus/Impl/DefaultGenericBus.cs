using Common.Logging;
using Contoso.GenericBus.Exceptions;
using Contoso.GenericBus.Internal;
using System;
using System.Collections.Generic;

namespace Contoso.GenericBus.Impl
{
    /// <summary>
    /// DefaultGenericBus
    /// </summary>
    public class DefaultGenericBus : IStartableServiceBus
    {
        readonly IServiceLocator _serviceLocator;
        readonly ILog _logger = LogManager.GetLogger(typeof(DefaultGenericBus));
        /// <summary>
        /// The _current message
        /// </summary>
        [ThreadStatic]
        public static object CurrentMessage;
        IEnumerable<IServiceBusAware> _serviceBusAware = new IServiceBusAware[0];

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultGenericBus"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public DefaultGenericBus(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        /// <summary>
        /// Replies the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.ArgumentNullException">messages</exception>
        /// <exception cref="MessagePublicationException">Cannot reply with an empty message batch</exception>
        public void Reply(params object[] messages)
        {
            if (messages == null)
                throw new ArgumentNullException("messages");
            if (messages.Length == 0)
                throw new MessagePublicationException("Cannot reply with an empty message batch");
        }

        /// <summary>
        /// Sends the specified messages.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.ArgumentNullException">messages</exception>
        /// <exception cref="MessagePublicationException">Cannot send empty message batch</exception>
        public void Send(string endpoint, params object[] messages)
        {
            if (messages == null)
                throw new ArgumentNullException("messages");
            if (messages.Length == 0)
                throw new MessagePublicationException("Cannot send empty message batch");
        }

        /// <summary>
        /// Send the message directly to the default endpoint
        /// for this type of message
        /// </summary>
        /// <param name="messages"></param>
        public void Send(params object[] messages)
        {
            Send(null, messages);
        }

        /// <summary>
        /// Get the endpoint of the bus
        /// </summary>
        public string Endpoint
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var aware in _serviceBusAware)
                aware.BusDisposing(this);
            foreach (var aware in _serviceBusAware)
                aware.BusDisposed(this);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _serviceBusAware = _serviceLocator.ResolveAll<IServiceBusAware>();
            foreach (var aware in _serviceBusAware)
                aware.BusStarting(this);
            _logger.DebugFormat("Starting the bus for .");
            foreach (var aware in _serviceBusAware)
                aware.BusStarted(this);
        }
    }
}
