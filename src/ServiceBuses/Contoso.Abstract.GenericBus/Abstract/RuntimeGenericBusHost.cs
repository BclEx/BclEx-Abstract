#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using System.Abstract;
using Contoso.Abstract.GenericBus;
namespace Contoso.Abstract
{
    /// <summary>
    /// RuntimeGenericBusHost
    /// </summary>
    public abstract class RuntimeGenericBusHost : ServiceLocatorBootStrapper, IServiceBusHostRuntime
    {
        readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeGenericBusHost"/> class.
        /// </summary>
        protected RuntimeGenericBusHost()
            : base(ServiceLocatorManager.Current) { CommonLoggingFactoryAdapter.EnsureRegistration(); }
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeGenericBusHost"/> class.
        /// </summary>
        /// <param name="locator">The locator.</param>
        protected RuntimeGenericBusHost(IServiceLocator locator)
            : base(locator) { CommonLoggingFactoryAdapter.EnsureRegistration(); }
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeGenericBusHost"/> class.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="name">The name.</param>
        protected RuntimeGenericBusHost(IServiceLocator locator, string name)
            : base(locator) { CommonLoggingFactoryAdapter.EnsureRegistration(); _name = name; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// Opens the specified bus.
        /// </summary>
        /// <param name="bus">The bus.</param>
        public virtual void Open(IServiceBus bus) { }
        /// <summary>
        /// Closes this instance.
        /// </summary>
        public virtual void Close() { }

        /// <summary>
        /// Initializes the container.
        /// </summary>
        public override void InitializeContainer() { Initialize(); base.InitializeContainer(); }
        /// <summary>
        /// Called when [end start].
        /// </summary>
        protected override void OnEndStart()
        {
            base.OnEndStart();
            ServiceBusManager.SetProvider(() => new GenericBusAbstractor(_locator, GetInstance<Contoso.GenericBus.IServiceBus>()))
                .RegisterWithServiceLocator(_locator, _name);
            // also opens to connection, thus registering IServiceBus
            Open(ServiceBusManager.Current);
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public override void Dispose() { base.Dispose(); Close(); }
    }
}
