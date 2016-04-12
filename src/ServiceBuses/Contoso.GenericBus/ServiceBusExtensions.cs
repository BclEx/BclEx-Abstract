using System;

namespace Contoso.GenericBus
{
    /// <summary>
    /// Provides some convenience extension methods to IServiceBus
    /// </summary>
    public static class ServiceBusExtensions
    {
        /// <summary>
        /// Sends the message directly to this bus endpoint
        /// </summary>
        public static void SendToSelf(this IServiceBus serviceBus, params object[] messages)
        {
            serviceBus.Send(serviceBus.Endpoint, messages);
        }
    }
}
