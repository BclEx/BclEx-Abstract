using System;

namespace Contoso.GenericBus.Hosting
{
    /// <summary>
    /// IApplicationHost
    /// </summary>
    public interface IApplicationHost : IDisposable
    {
        /// <summary>
        /// Starts the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void Start(string assembly);
        /// <summary>
        /// Initials the deployment.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="user">The user.</param>
        void InitialDeployment(string assembly, string user);
        /// <summary>
        /// Sets the name of the boot strapper type.
        /// </summary>
        /// <param name="type">The type.</param>
        void SetBootStrapperTypeName(string type);
    }
}