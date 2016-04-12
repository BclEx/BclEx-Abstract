using System.ServiceProcess;

namespace Contoso.GenericBusHost.Actions
{
    /// <summary>
    /// ServerAction
    /// </summary>
    public class ServerAction : IAction
    {
        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void Execute(ExecutingOptions options)
        {
            var host = new Host();
            host.SetArguments(options);
            ServiceBase.Run(host);
        }
    }
}