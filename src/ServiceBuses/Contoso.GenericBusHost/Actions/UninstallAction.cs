using System.Configuration.Install;

namespace Contoso.GenericBusHost.Actions
{
    /// <summary>
    /// UninstallAction
    /// </summary>
    public class UninstallAction : IAction
    {
        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void Execute(ExecutingOptions options)
        {
            var installer = new ProjectInstaller
            {
                DisplayName = options.Name,
                Description = options.Name,
                Context = new InstallContext()
            };
            installer.Uninstall(null);
        }
    }
}