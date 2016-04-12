namespace Contoso.GenericBus
{
    /// <summary>
    /// The generic bus abstraction
    /// </summary>
    public interface IServiceBus
    {
        /// <summary>
        /// Reply to the source of the current message
        /// Will throw if not currently handling a message
        /// </summary>
        /// <param name="messages"></param>
        void Reply(params object[] messages);
        /// <summary>
        /// Send the message directly to the specified endpoint
        /// </summary>
        void Send(string endpoint, params object[] messages);
        /// <summary>
        /// Send the message directly to the default endpoint
        /// for this type of message
        /// </summary>
        void Send(params object[] messages);

        /// <summary>
        /// Get the endpoint of the bus
        /// </summary>
        string Endpoint { get; }
    }
}
