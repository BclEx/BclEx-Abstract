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
using System.Linq;
using System.Abstract;
using System.Reflection;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using Contoso.Abstract.Micro;
using System.Abstract.Configuration;
using Contoso.Abstract.Micro.ImplServiceBus;
namespace Contoso.Abstract
{
    /// <summary>
    /// IAzureServiceBus
    /// </summary>
    public interface IAzureServiceBus : IServiceBus
    {
        /// <summary>
        /// Gets the bus.
        /// </summary>
        IMicroServiceBus Bus { get; }
    }

    /// <summary>
    /// AzureServiceBusAbstractor
    /// </summary>
    public partial class AzureServiceBusAbstractor : IAzureServiceBus, ServiceBusManager.ISetupRegistration
    {
        private IServiceLocator _serviceLocator;

        static AzureServiceBusAbstractor() { ServiceBusManager.EnsureRegistration(); }
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusAbstractor"/> class.
        /// </summary>
        public AzureServiceBusAbstractor()
            : this(ServiceLocatorManager.Current, DefaultBusCreator(null, null, null)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public AzureServiceBusAbstractor(IServiceLocator serviceLocator)
            : this(serviceLocator, DefaultBusCreator(serviceLocator, null, null)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="busConfiguration">The bus configuration.</param>
        public AzureServiceBusAbstractor(ServiceBusConfiguration busConfiguration)
            : this(ServiceLocatorManager.Current, DefaultBusCreator(null, busConfiguration, null)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="busConfiguration">The bus configuration.</param>
        public AzureServiceBusAbstractor(IServiceLocator serviceLocator, ServiceBusConfiguration busConfiguration)
            : this(serviceLocator, DefaultBusCreator(serviceLocator, busConfiguration, null)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RhinoServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="bus">The bus.</param>
        public AzureServiceBusAbstractor(IServiceLocator serviceLocator, IStartableMicroServiceBus bus)
            : this(serviceLocator, (IMicroServiceBus)bus) { bus.Start(); }
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusAbstractor"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="bus">The bus.</param>
        public AzureServiceBusAbstractor(IServiceLocator serviceLocator, IMicroServiceBus bus)
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
            get { return (locator, name) => ServiceBusManager.RegisterInstance<IAzureServiceBus>(this, locator, name); }
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
                if (endpoint == null) Bus.Send(messages);
                else if (endpoint != ServiceBus.SelfEndpoint) Bus.Send(endpoint, messages);
                else Bus.Send(Bus.Endpoint, messages);
            }
            catch (Exception ex) { throw new ServiceBusMessageException(messages[0].GetType(), ex); }
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
            try { Bus.Reply(messages); }
            catch (Exception ex) { throw new ServiceBusMessageException(messages[0].GetType(), ex); }
        }

        #region Domain-specific

        /// <summary>
        /// Gets the bus.
        /// </summary>
        public IMicroServiceBus Bus { get; private set; }

        #endregion

        /// <summary>
        /// Defaults the bus creator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="busConfiguration">The bus configuration.</param>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IStartableMicroServiceBus DefaultBusCreator(IServiceLocator serviceLocator, ServiceBusConfiguration busConfiguration, Action<AbstractMicroServiceBusConfiguration> configurator)
        //public static IStartableMicroServiceBus DefaultBusCreator(IServiceLocator serviceLocator, string connectionString, string queueName)
        {
            if (serviceLocator == null)
                serviceLocator = ServiceLocatorManager.Current;
            var configuration = new MicroServiceBusConfiguration()
                //.UseMessageSerializer<RhinoServiceBus.Serializers.XmlMessageSerializer>()
                .UseAbstractServiceLocator(serviceLocator);
            if (busConfiguration != null)
                configuration.UseConfiguration(busConfiguration);
            if (configurator != null)
                configurator(configuration);
            configuration.Configure();

            //_client = QueueClient.CreateFromConnectionString(connectionString, queueName);
            return serviceLocator.Resolve<IStartableMicroServiceBus>();
        }

        /// <summary>
        /// Configures the consumers.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="assemblyToScan">The assembly to scan.</param>
        /// <param name="condition">The condition.</param>
        public static void ConfigureConsumers(IServiceLocator serviceLocator, Assembly assemblyToScan, Predicate<Type> condition)
        {
            if (serviceLocator == null)
                serviceLocator = ServiceLocatorManager.Current;
            if (assemblyToScan == null)
                throw new ArgumentNullException("assemblyToScan");
            var types = assemblyToScan.GetTypes()
                .Where(type => typeof(IServiceMessageHandler).IsAssignableFrom(type) &&
                    (condition == null || condition(type)));
            var r = serviceLocator.Registrar;
            foreach (var type in types)
                ConfigureConsumer(r, type);
        }

        /// <summary>
        /// Configures the consumer.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="type">The type.</param>
        public static void ConfigureConsumer(IServiceRegistrar r, Type type)
        {
            r.Register<IServiceMessageHandler>(type, type.FullName);
        }
    }
}
