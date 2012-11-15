#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using System;
using System.Abstract;
using ZMQ;
// [Main] https://github.com/zeromq/clrzmq
// [C#] http://www.codeproject.com/Articles/488207/ZeroMQ-via-Csharp-Introduction
namespace Contoso.Abstract
{
    /// <summary>
    /// IZeroMQServiceBus
    /// </summary>
    public interface IZeroMQServiceBus : IServiceBus
    {
        /// <summary>
        /// Gets the bus.
        /// </summary>
        Context Ctx { get; }
    }

    /// <summary>
    /// ZeroMQServiceBusAbstractor
    /// </summary>
    public partial class ZeroMQServiceBusAbstractor : IZeroMQServiceBus, ServiceBusManager.ISetupRegistration
    {
        private IServiceLocator _serviceLocator;

        static ZeroMQServiceBusAbstractor() { ServiceBusManager.EnsureRegistration(); }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZeroMQServiceBusAbstractor"/> class.
        /// </summary>
        public ZeroMQServiceBusAbstractor()
            : this(ServiceLocatorManager.Current, new Context()) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZeroMQServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public ZeroMQServiceBusAbstractor(IServiceLocator serviceLocator)
            : this(serviceLocator, new Context()) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZeroMQServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="context">The context.</param>
        public ZeroMQServiceBusAbstractor(IServiceLocator serviceLocator, Context context)
        {
            if (serviceLocator == null)
                throw new ArgumentNullException("serviceLocator");
            if (context == null)
                throw new ArgumentNullException("context");
            _serviceLocator = serviceLocator;
            Ctx = context;
        }

        Action<IServiceLocator, string> ServiceBusManager.ISetupRegistration.DefaultServiceRegistrar
        {
            get { return (locator, name) => ServiceBusManager.RegisterInstance<IZeroMQServiceBus>(this, locator, name); }
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        public object GetService(Type serviceType) { throw new NotImplementedException(); }

        /// <summary>
        /// Creates the message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="messageBuilder">The message builder.</param>
        /// <returns></returns>
        public TMessage CreateMessage<TMessage>(Action<TMessage> messageBuilder)
            where TMessage : class
        {
            var message = _serviceLocator.Resolve<TMessage>();
            if (messageBuilder != null)
                messageBuilder(message);
            return message;
        }

        /// <summary>
        /// Sends the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public virtual IServiceBusCallback Send(IServiceBusEndpoint endpoint, params object[] messages)
        {
            if (messages == null || messages.Length == 0 || messages[0] == null)
                throw new ArgumentNullException("messages", "Please include at least one message.");
            try
            {
                //if (endpoint == null) Bus.Send(messages);
                //else if (endpoint != ServiceBus.SelfEndpoint) Bus.Send(RhinoServiceBusTransport.Cast(endpoint), messages);
                //else Bus.SendToSelf(messages);
            }
            catch (ZMQ.Exception ex) { throw new ServiceBusMessageException(messages[0].GetType(), ex); }
            return null;
        }

        /// <summary>
        /// Replies the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public virtual void Reply(params object[] messages)
        {
            if (messages == null || messages.Length == 0 || messages[0] == null)
                throw new ArgumentNullException("messages", "Please include at least one message.");
            try
            {
                //Bus.Reply(messages);
            }
            catch (ZMQ.Exception ex) { throw new ServiceBusMessageException(messages[0].GetType(), ex); }
        }

        #region Domain-specific

        /// <summary>
        /// Gets the context.
        /// </summary>
        public Context Ctx { get; private set; }

        #endregion
    }
}
