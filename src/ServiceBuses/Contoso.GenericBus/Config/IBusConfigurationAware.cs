using Contoso.GenericBus.Impl;
using Contoso.GenericBus.Internal;

namespace Contoso.GenericBus.Config
{
    /// <summary>
    /// IBusConfigurationAware
    /// </summary>
    public interface IBusConfigurationAware
    {
        /// <summary>
        /// Configures the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="locator">The locator.</param>
        void Configure(AbstractGenericBusConfiguration config, IBusContainerBuilder builder, IServiceLocator locator);
    }
}