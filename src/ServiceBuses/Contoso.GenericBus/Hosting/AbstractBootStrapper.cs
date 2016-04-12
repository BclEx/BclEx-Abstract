using Contoso.GenericBus.Config;
using Contoso.GenericBus.Impl;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Contoso.GenericBus.Hosting
{
    /// <summary>
    /// AbstractBootStrapper
    /// </summary>
    public abstract class AbstractBootStrapper : IDisposable
    {
        AbstractGenericBusConfiguration _config;
        BusConfigurationSection _busSection;

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        public virtual IEnumerable<Assembly> Assemblies
        {
            get { yield return GetType().Assembly; }
        }

        /// <summary>
        /// Initializes the container.
        /// </summary>
        public virtual void InitializeContainer()
        {
            _config = CreateConfiguration();
            CreateContainer();
            ConfigureBusFacility(_config);
        }

        /// <summary>
        /// Uses the configuration.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        public virtual void UseConfiguration(BusConfigurationSection configurationSection)
        {
            _busSection = configurationSection;
            if (_config != null) _config.UseConfiguration(_busSection);
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        public abstract void CreateContainer();

        /// <summary>
        /// Executes the deployment actions.
        /// </summary>
        /// <param name="user">The user.</param>
        public abstract void ExecuteDeploymentActions(string user);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T GetInstance<T>();

        /// <summary>
        /// Determines whether [is type acceptable for this boot strapper] [the specified t].
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        protected virtual bool IsTypeAcceptableForThisBootStrapper(Type t)
        {
            return true;
        }

        /// <summary>
        /// Creates the configuration.
        /// </summary>
        /// <returns></returns>
        protected virtual AbstractGenericBusConfiguration CreateConfiguration()
        {
            var cfg = new GenericBusConfiguration();
            if (_busSection != null) cfg.UseConfiguration(_busSection);
            return cfg;
        }

        /// <summary>
        /// Configures the bus facility.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected virtual void ConfigureBusFacility(AbstractGenericBusConfiguration configuration)
        {
        }

        /// <summary>
        /// Called when [begin start].
        /// </summary>
        protected virtual void OnBeginStart()
        {
        }

        /// <summary>
        /// Begins the start.
        /// </summary>
        public void BeginStart()
        {
            OnBeginStart();
            _config.Configure();
        }

        /// <summary>
        /// Ends the start.
        /// </summary>
        public void EndStart()
        {
            OnEndStart();
        }

        /// <summary>
        /// Called when [end start].
        /// </summary>
        protected virtual void OnEndStart()
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();
    }
}
