using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Contoso.GenericBusHost
{
    /// <summary>
    /// ProjectInstaller
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName
        {
            get { return serviceInstaller1.ServiceName; }
        }

        /// <summary>
        /// Sets the user account.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void SetUserAccount(string username, string password)
        {
            serviceProcessInstaller1.Account = ServiceAccount.User;
            serviceProcessInstaller1.Username = username;
            serviceProcessInstaller1.Password = password;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInstaller"/> class.
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            set
            {
                serviceInstaller1.DisplayName = value;
                serviceInstaller1.ServiceName = value;
            }
        }

        /// <summary>
        /// Sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            set { this.serviceInstaller1.Description = value; }
        }
    }
}
