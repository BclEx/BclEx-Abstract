namespace Contoso.GenericBus.Actions
{
    /// <summary>
    /// IDeploymentAction
    /// </summary>
    public interface IDeploymentAction
    {
        /// <summary>
        /// Executes the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Execute(string user);
    }
}