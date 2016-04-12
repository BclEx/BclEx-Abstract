using Common.Logging;
using Contoso.GenericBus.Config;
using Contoso.GenericBus.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace Contoso.GenericBus.Hosting
{
    /// <summary>
    /// DefaultHost
    /// </summary>
    public class DefaultHost : MarshalByRefObject, IApplicationHost
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(DefaultHost));
        string _assemblyName;
        AbstractBootStrapper _bootStrapper;
        IStartable _startable;
        string _bootStrapperName;
        //BusConfigurationSection _hostConfiguration;

        /// <summary>
        /// Gets the bus.
        /// </summary>
        /// <value>
        /// The bus.
        /// </value>
        public IStartable Bus
        {
            get { return _startable; }
        }

        /// <summary>
        /// Sets the name of the boot strapper type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        public void SetBootStrapperTypeName(string typeName)
        {
            _bootStrapperName = typeName;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <typeparam name="TBootStrapper">The type of the boot strapper.</typeparam>
        public void Start<TBootStrapper>()
            where TBootStrapper : AbstractBootStrapper
        {
            SetBootStrapperTypeName(typeof(TBootStrapper).FullName);
            Start(typeof(TBootStrapper).Assembly.FullName);
        }

        /// <summary>
        /// Starts the specified asm name.
        /// </summary>
        /// <param name="asmName">Name of the asm.</param>
        public void Start(string asmName)
        {
            InitailizeBus(asmName);
            _startable.Start();
            _bootStrapper.EndStart();
        }

        private void InitailizeBus(string asmName)
        {
            _assemblyName = asmName;
            CreateBootStrapper();
            InitializeContainer();
            _bootStrapper.BeginStart();

            _logger.Debug("Starting bus");
            _startable = _bootStrapper.GetInstance<IStartable>();
        }

        private void InitializeContainer()
        {
            _bootStrapper.InitializeContainer();
            //if (_hostConfiguration != null)
            //    _bootStrapper.UseConfiguration(_hostConfiguration);
        }

        private void CreateBootStrapper()
        {
            _logger.DebugFormat("Loading {0}", _assemblyName);
            var assembly = Assembly.Load(_assemblyName);
            Type bootStrapperType = null;
            if (!string.IsNullOrEmpty(_bootStrapperName))
                bootStrapperType = assembly.GetType(_bootStrapperName);
            bootStrapperType = (bootStrapperType ?? GetAutoBootStrapperType(assembly));
            try { _bootStrapper = (AbstractBootStrapper)Activator.CreateInstance(bootStrapperType); }
            catch (Exception e) { throw new InvalidOperationException("Failed to create " + bootStrapperType + ".", e); }
        }

        private static Type GetAutoBootStrapperType(Assembly assembly)
        {
            var bootStrappers = assembly.GetTypes()
                .Where(x => typeof(AbstractBootStrapper).IsAssignableFrom(x) && !x.IsAbstract)
                .ToArray();
            if (bootStrappers.Length == 0)
                throw new InvalidOperationException("Could not find a boot strapper for " + assembly);
            if (bootStrappers.Length > 1)
                throw new InvalidOperationException("Found more than one boot strapper for " + assembly + " you need to specify which boot strapper to use: " + Environment.NewLine + string.Join(Environment.NewLine, bootStrappers.Select(x => x.FullName).ToArray()));
            return bootStrappers[0];
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_bootStrapper != null)
                _bootStrapper.Dispose();
            if (_startable != null)
                _startable.Dispose();
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" /> property.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
        /// </PermissionSet>
        public override object InitializeLifetimeService()
        {
            return null; //singleton
        }

        /// <summary>
        /// Initials the deployment.
        /// </summary>
        /// <param name="asmName">Name of the asm.</param>
        /// <param name="user">The user.</param>
        public void InitialDeployment(string asmName, string user)
        {
            InitailizeBus(asmName);
            _bootStrapper.ExecuteDeploymentActions(user);
        }

        //public void BusConfiguration(Func<HostConfiguration, HostConfiguration> configuration)
        //{
        //    _hostConfiguration = configuration(new HostConfiguration()).ToBusConfiguration();
        //}
    }
}
