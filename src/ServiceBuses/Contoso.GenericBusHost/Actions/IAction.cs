namespace Contoso.GenericBusHost.Actions
{
    /// <summary>
    /// IAction
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        void Execute(ExecutingOptions options);
    }
}