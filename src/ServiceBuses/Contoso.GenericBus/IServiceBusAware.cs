namespace Contoso.GenericBus
{
    /// <summary>
    /// IServiceBusAware
    /// </summary>
	public interface IServiceBusAware
	{
        /// <summary>
        /// Buses the starting.
        /// </summary>
        /// <param name="bus">The bus.</param>
		void BusStarting(IServiceBus bus);
        /// <summary>
        /// Buses the started.
        /// </summary>
        /// <param name="bus">The bus.</param>
		void BusStarted(IServiceBus bus);
        /// <summary>
        /// Buses the disposing.
        /// </summary>
        /// <param name="bus">The bus.</param>
		void BusDisposing(IServiceBus bus);
        /// <summary>
        /// Buses the disposed.
        /// </summary>
        /// <param name="bus">The bus.</param>
		void BusDisposed(IServiceBus bus);
	}
}