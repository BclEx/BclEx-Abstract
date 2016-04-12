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
using Contoso.GenericBus.Actions;
using Contoso.GenericBus.Hosting;
using Contoso.GenericBus.Internal;
using System;
using System.Abstract;
using System.Linq;
using System.Reflection;
namespace Contoso.Abstract.GenericBus
{
    /// <summary>
    /// ServiceLocatorBootStrapper
    /// </summary>
    public abstract class ServiceLocatorBootStrapper : AbstractBootStrapper
    {
        internal System.Abstract.IServiceLocator _locator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorBootStrapper"/> class.
        /// </summary>
        protected ServiceLocatorBootStrapper() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorBootStrapper"/> class.
        /// </summary>
        /// <param name="locator">The locator.</param>
        protected ServiceLocatorBootStrapper(System.Abstract.IServiceLocator locator)
        {
            _locator = locator;
        }

        private void ConfigureConsumers(Assembly assemblyToScan)
        {
            var types = assemblyToScan.GetTypes()
                .Where(type => typeof(IGenericConsumer).IsAssignableFrom(type) &&
                    IsTypeAcceptableForThisBootStrapper(type));
            var r = _locator.Registrar;
            foreach (var type in types)
                ConfigureConsumer(r, type);
        }

        /// <summary>
        /// Configures the consumer.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="type">The type.</param>
        protected virtual void ConfigureConsumer(IServiceRegistrar r, Type type)
        {
            r.Register<IGenericConsumer>(type, type.FullName);
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            var registrar = _locator.Registrar;
            foreach (var assembly in Assemblies)
            {
                registrar.RegisterByTypeMatch<IDeploymentAction>(assembly);
                ConfigureConsumers(assembly);
            }
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        public override void CreateContainer()
        {
            if (_locator == null)
                _locator = ServiceLocatorManager.Current;
            ConfigureContainer();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public override void Dispose()
        {
            var disposable = (_locator as IDisposable);
            if (disposable != null)
                disposable.Dispose();
        }

        /// <summary>
        /// Executes the deployment actions.
        /// </summary>
        /// <param name="user">The user.</param>
        public override void ExecuteDeploymentActions(string user)
        {
            foreach (var action in _locator.ResolveAll<IDeploymentAction>())
                action.Execute(user);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T GetInstance<T>()
        {
            return (T)_locator.Resolve(typeof(T));
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        protected System.Abstract.IServiceLocator Container
        {
            get { return _locator; }
        }
    }
}