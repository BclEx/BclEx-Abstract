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
using Contoso.Abstract.Micro;
using Contoso.Abstract.Micro.ImplServiceBus;
using System;
using System.Abstract;
using System.Abstract.Configuration;
namespace Contoso.Abstract
{
    /// <summary>
    /// IOnewayAzureServiceBus
    /// </summary>
    public interface IOnewayAzureServiceBus : IServiceBus
    {
        /// <summary>
        /// Gets the bus.
        /// </summary>
        IOnewayMicroServiceBus Bus { get; }
    }

    /// <summary>
    /// OnewayAzureServiceBusAbstractor
    /// </summary>
    public partial class OnewayAzureServiceBusAbstractor : IOnewayAzureServiceBus, ServiceBusManager.ISetupRegistration
    {
        private IServiceLocator _serviceLocator;

        static OnewayAzureServiceBusAbstractor() { ServiceBusManager.EnsureRegistration(); }
        /// <summary>
        /// Initializes a new instance of the <see cref="OnewayAzureServiceBusAbstractor"/> class.
        /// </summary>
        public OnewayAzureServiceBusAbstractor()
            : this(ServiceLocatorManager.Current, DefaultBusCreator(null, null, null)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OnewayAzureServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public OnewayAzureServiceBusAbstractor(IServiceLocator serviceLocator)
            : this(serviceLocator, DefaultBusCreator(serviceLocator, null, null)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OnewayAzureServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="busConfiguration">The bus configuration.</param>
        public OnewayAzureServiceBusAbstractor(ServiceBusConfiguration busConfiguration)
            : this(ServiceLocatorManager.Current, DefaultBusCreator(null, busConfiguration, null)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OnewayAzureServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="busConfiguration">The bus configuration.</param>
        public OnewayAzureServiceBusAbstractor(IServiceLocator serviceLocator, ServiceBusConfiguration busConfiguration)
            : this(serviceLocator, DefaultBusCreator(serviceLocator, busConfiguration, null)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="OnewayAzureServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="bus">The bus.</param>
        public OnewayAzureServiceBusAbstractor(IServiceLocator serviceLocator, IOnewayMicroServiceBus bus)
        {
            if (serviceLocator == null)
                throw new ArgumentNullException("serviceLocator");
            if (bus == null)
                throw new ArgumentNullException("bus", "The specified bus cannot be null.");
            _serviceLocator = serviceLocator;
            Bus = bus;
        }

        Action<IServiceLocator, string> ServiceBusManager.ISetupRegistration.DefaultServiceRegistrar
        {
            get { return (locator, name) => ServiceBusManager.RegisterInstance<IOnewayAzureServiceBus>(this, locator, name); }
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
            //if (endpoint == null)
            //    endpoint = RhinoServiceBusTransport.EndpointByMessageType(messages[0].GetType());
            try
            {
                if (endpoint == null) Bus.Send(messages);
                else throw new NotSupportedException();
            }
            catch (Exception ex) { throw new ServiceBusMessageException(messages[0].GetType(), ex); }
            return null;
        }

        /// <summary>
        /// Replies the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public virtual void Reply(params object[] messages) { throw new NotSupportedException(); }

        #region Domain-specific

        /// <summary>
        /// Gets the bus.
        /// </summary>
        public IOnewayMicroServiceBus Bus { get; private set; }

        #endregion

        /// <summary>
        /// Defaults the bus creator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="busConfiguration">The bus configuration.</param>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IOnewayMicroServiceBus DefaultBusCreator(IServiceLocator serviceLocator, ServiceBusConfiguration busConfiguration, Action<AbstractMicroServiceBusConfiguration> configurator)
        {
            if (serviceLocator == null)
                serviceLocator = ServiceLocatorManager.Current;
            var configuration = new OnewayMicroServiceBusConfiguration()
                //.UseMessageSerializer<RhinoServiceBus.Serializers.XmlMessageSerializer>()
                .UseAbstractServiceLocator(serviceLocator);
            if (busConfiguration != null)
            {
                if (busConfiguration.MessageOwners == null)
                    throw new ArgumentNullException("busConfiguration.MessageOwners");
                configuration.UseConfiguration(busConfiguration);
            }
            if (configurator != null)
                configurator(configuration);
            configuration.Configure();
            return serviceLocator.Resolve<IOnewayMicroServiceBus>();
        }
    }
}
