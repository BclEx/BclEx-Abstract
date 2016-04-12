namespace Contoso.GenericBusHost.Actions
{
    /// <summary>
    /// DeployAction
    /// </summary>
    public class DeployAction : IAction
    {
        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void Execute(ExecutingOptions options)
        {
            var host = new Host();
            host.SetArguments(options);
            host.InitialDeployment(options.Account);
        }
    }
}