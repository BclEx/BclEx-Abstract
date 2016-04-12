using System;

namespace Contoso.GenericBus.Internal
{
    /// <summary>
    /// IStartable
    /// </summary>
    public interface IStartable : IDisposable
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();
    }
}