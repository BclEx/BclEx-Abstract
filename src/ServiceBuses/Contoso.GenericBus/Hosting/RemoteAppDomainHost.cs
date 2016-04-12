using Common.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Contoso.GenericBus.Hosting
{
    /// <summary>
    /// RemoteAppDomainHost
    /// </summary>
    public class RemoteAppDomainHost
    {
        readonly string _boosterType;
        readonly string _assemblyName;
        readonly string _assemblyLocation;
        HostedService _current;
        string _configurationFile;
        string _hostType = typeof(DefaultHost).FullName;
        string _hostAsm = typeof(DefaultHost).Assembly.FullName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteAppDomainHost"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="configuration">The configuration.</param>
        public RemoteAppDomainHost(Assembly assembly, string configuration)
            : this(assembly.Location, configuration) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteAppDomainHost"/> class.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="boosterType">Type of the booster.</param>
        public RemoteAppDomainHost(string assemblyPath, Type boosterType)
            : this(assemblyPath, (string)null)
        {
            _boosterType = boosterType.FullName;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteAppDomainHost"/> class.
        /// </summary>
        /// <param name="boosterType">Type of the booster.</param>
        public RemoteAppDomainHost(Type boosterType)
            : this(boosterType.Assembly.Location, boosterType)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteAppDomainHost"/> class.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="configuration">The configuration.</param>
        public RemoteAppDomainHost(string assemblyPath, string configuration)
        {
            _configurationFile = configuration;
            _assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            _assemblyLocation = GetAssemblyLocation(assemblyPath);
        }

        private static string GetAssemblyLocation(string assemblyPath)
        {
            if (Path.IsPathRooted(assemblyPath))
                return Path.GetDirectoryName(assemblyPath);
            var currentDirPath = Path.Combine(Environment.CurrentDirectory, assemblyPath);
            if (File.Exists(currentDirPath))
                return Path.GetDirectoryName(currentDirPath);
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyPath);
            if (File.Exists(basePath))
                return Path.GetDirectoryName(basePath);
            // no idea, use the default
            return Path.GetDirectoryName(assemblyPath);
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>
        protected string AssemblyName
        {
            get { return _assemblyName; }
        }

        /// <summary>
        /// Gets the application host.
        /// </summary>
        /// <value>
        /// The application host.
        /// </value>
        public IApplicationHost ApplicationHost
        {
            get { return _current.ApplicationHost; }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="System.TypeLoadException"></exception>
        public void Start()
        {
            _current = CreateNewAppDomain();
            try { _current.Start(); }
            catch (ReflectionTypeLoadException e)
            {
                var sb = new StringBuilder();
                foreach (var exception in e.LoaderExceptions)
                    sb.AppendLine(exception.ToString());
                throw new TypeLoadException(sb.ToString(), e);
            }
        }

        private HostedService CreateNewAppDomain()
        {
            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = _assemblyLocation,
                ApplicationName = _assemblyName,
                ConfigurationFile = ConfigurationFile,
                ShadowCopyFiles = "true" //yuck
            };
            var appDomain = AppDomain.CreateDomain(_assemblyName, null, appDomainSetup);
            return CreateRemoteHost(appDomain);
        }

        /// <summary>
        /// Creates the remote host.
        /// </summary>
        /// <param name="appDomain">The application domain.</param>
        /// <returns></returns>
        protected virtual HostedService CreateRemoteHost(AppDomain appDomain)
        {
            var hoster = (IApplicationHost)appDomain.CreateInstanceAndUnwrap(_hostAsm, _hostType);
            if (_boosterType != null)
                hoster.SetBootStrapperTypeName(_boosterType);
            return new HostedService(hoster, _assemblyName, appDomain);
        }

        private string ConfigurationFile
        {
            get
            {
                if (_configurationFile != null)
                    return _configurationFile;
                _configurationFile = Path.Combine(_assemblyLocation, _assemblyName + ".dll.config");
                if (!File.Exists(_configurationFile))
                    _configurationFile = Path.Combine(_assemblyLocation, _assemblyName + ".exe.config");
                return _configurationFile;
            }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            if (_current != null)
                _current.Stop();
        }

        #region Nested type: HostedService

        /// <summary>
        /// 
        /// </summary>
        protected class HostedService
        {
            readonly IApplicationHost _hoster;
            readonly string _assembly;
            readonly AppDomain _appDomain;
            readonly ILog _log = LogManager.GetLogger(typeof(HostedService));

            /// <summary>
            /// Gets the application host.
            /// </summary>
            /// <value>
            /// The application host.
            /// </value>
            public IApplicationHost ApplicationHost
            {
                get { return _hoster; }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="HostedService"/> class.
            /// </summary>
            /// <param name="hoster">The hoster.</param>
            /// <param name="assembly">The assembly.</param>
            /// <param name="appDomain">The application domain.</param>
            public HostedService(IApplicationHost hoster, string assembly, AppDomain appDomain)
            {
                _hoster = hoster;
                _assembly = assembly;
                _appDomain = appDomain;
            }

            /// <summary>
            /// Stops this instance.
            /// </summary>
            public void Stop()
            {
                _hoster.Dispose();
                try { AppDomain.Unload(_appDomain); }
                catch (Exception e) { _log.Error("Could not unload app domain, it is likely that there is a running thread that could not be aborted", e); }
            }

            /// <summary>
            /// Starts this instance.
            /// </summary>
            public void Start()
            {
                _hoster.Start(_assembly);
            }

            /// <summary>
            /// Initials the deployment.
            /// </summary>
            /// <param name="user">The user.</param>
            public void InitialDeployment(string user)
            {
                _hoster.InitialDeployment(_assembly, user);
            }
        }

        #endregion

        /// <summary>
        /// Initials the deployment.
        /// </summary>
        /// <param name="user">The user.</param>
        public void InitialDeployment(string user)
        {
            _current = CreateNewAppDomain();
            _current.InitialDeployment(user);
        }

        /// <summary>
        /// Sets the type of the host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        public RemoteAppDomainHost SetHostType(Type host)
        {
            _hostType = host.FullName;
            _hostAsm = host.Assembly.FullName;
            return this;
        }

        /// <summary>
        /// Sets the type of the host.
        /// </summary>
        /// <param name="hostTypeName">Name of the host type.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Could not parse host name</exception>
        public RemoteAppDomainHost SetHostType(string hostTypeName)
        {
            var parts = hostTypeName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new InvalidOperationException("Could not parse host name");
            _hostType = parts[0].Trim();
            _hostAsm = parts[1].Trim();
            return this;
        }

        /// <summary>
        /// Configurations the specified configuration file.
        /// </summary>
        /// <param name="configFile">The configuration file.</param>
        /// <returns></returns>
        public RemoteAppDomainHost Configuration(string configFile)
        {
            _configurationFile = configFile;
            return this;
        }
    }
}