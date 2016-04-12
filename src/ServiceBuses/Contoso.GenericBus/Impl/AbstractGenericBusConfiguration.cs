using Contoso.GenericBus.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace Contoso.GenericBus.Impl
{
    /// <summary>
    /// AbstractGenericBusConfiguration
    /// </summary>
    public abstract class AbstractGenericBusConfiguration
    {
        BusConfigurationSection _configurationSection;
        Action _readConfiguration;
        IBusContainerBuilder _busContainerBuilder;

        /// <summary>
        /// The _scan assemblies
        /// </summary>
        protected List<Assembly> ScanAssemblies = new List<Assembly>(new[] { typeof(IServiceBus).Assembly });

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGenericBusConfiguration"/> class.
        /// </summary>
        protected AbstractGenericBusConfiguration()
        {
            _readConfiguration = () =>
            {
                _configurationSection = ConfigurationManager.GetSection("generic.esb") as BusConfigurationSection;
            };
        }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        public IEnumerable<Assembly> Assemblies
        {
            get { return ScanAssemblies; }
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        /// <value>
        /// The configuration section.
        /// </value>
        public BusConfigurationSection ConfigurationSection
        {
            get { return _configurationSection; }
        }

        /// <summary>
        /// Gets the builder.
        /// </summary>
        /// <value>
        /// The builder.
        /// </value>
        protected IBusContainerBuilder Builder
        {
            get { return _busContainerBuilder; }
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        public virtual void Configure()
        {
            ReadBusConfiguration();
            ApplyConfiguration();
            Builder.RegisterDefaultServices(Assemblies);
        }

        /// <summary>
        /// Applies the configuration.
        /// </summary>
        protected abstract void ApplyConfiguration();

        /// <summary>
        /// Reads the bus configuration.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">could not find generic.esb configuration section</exception>
        protected virtual void ReadBusConfiguration()
        {
            if (_configurationSection != null)
                return;
            _readConfiguration();
            if (_configurationSection == null)
                throw new ConfigurationErrorsException("could not find generic.esb configuration section");
        }

        /// <summary>
        /// Uses the standalone configuration file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public AbstractGenericBusConfiguration UseStandaloneConfigurationFile(string fileName)
        {
            _readConfiguration = () =>
            {
                _configurationSection = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(fileName)).GetSection("generic.esb") as BusConfigurationSection;
            };
            return this;
        }

        /// <summary>
        /// Uses the configuration.
        /// </summary>
        /// <param name="busConfiguration">The bus configuration.</param>
        /// <returns></returns>
        public AbstractGenericBusConfiguration UseConfiguration(BusConfigurationSection busConfiguration)
        {
            _configurationSection = busConfiguration;
            return this;
        }

        /// <summary>
        /// Builds the with.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void BuildWith(IBusContainerBuilder builder)
        {
            _busContainerBuilder = builder;
        }
    }
}