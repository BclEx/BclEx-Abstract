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
using System;
using System.Abstract;
using System.Linq;
using Contoso.GenericBus;
using Contoso.GenericBus.Actions;
using Contoso.GenericBus.Config;
using Contoso.GenericBus.Impl;
using Contoso.GenericBus.Internal;
using IServiceBus = Contoso.GenericBus.IServiceBus;
using IServiceLocator = Contoso.GenericBus.Internal.IServiceLocator;
using System.Reflection;
using System.Collections.Generic;
namespace Contoso.Abstract.GenericBus
{
    internal class ServiceLocatorBuilder : IBusContainerBuilder
    {
        readonly AbstractGenericBusConfiguration _config;
        readonly System.Abstract.IServiceLocator _locator;
        readonly System.Abstract.IServiceRegistrar _registrar;

        public ServiceLocatorBuilder(System.Abstract.IServiceLocator locator, AbstractGenericBusConfiguration config)
        {
            _locator = locator;
            _registrar = locator.Registrar;
            _config = config;
            _config.BuildWith(this);
        }

        public void RegisterAll<T>(Predicate<Type> condition)
            where T : class
        {
            _registrar.RegisterByTypeMatch<T>(condition, typeof(T).Assembly);
        }

        public void RegisterAll<T>(params Type[] excludes)
            where T : class
        {
            _registrar.RegisterByTypeMatch<T>(x => (!x.IsAbstract && !x.IsInterface && typeof(T).IsAssignableFrom(x) && !excludes.Contains<Type>(x)), typeof(T).Assembly);
        }

        public void RegisterBus()
        {
            var config = (GenericBusConfiguration)_config;
            _registrar.BehaveAs(ServiceRegistrarLifetime.Singleton).Register<IStartableServiceBus>(l => new DefaultGenericBus(l.Resolve<IServiceLocator>()));
            _registrar.Register<IStartable, IStartableServiceBus>();
            _registrar.Register<IServiceBus, IStartableServiceBus>();
        }

        public void RegisterDefaultServices(IEnumerable<Assembly> assemblies)
        {
            _registrar.Register<IServiceLocator, ServiceLocatorAdapter>();
            ServiceLocatorExtensions.RegisterByTypeMatch<IBusConfigurationAware>(_registrar, typeof(IServiceBus).Assembly);
            foreach (var assembly in assemblies)
                _registrar.RegisterByTypeMatch<IBusConfigurationAware>(assembly);
            var locator = _locator.Resolve<IServiceLocator>();
            foreach (var aware in _locator.ResolveAll<IBusConfigurationAware>())
                aware.Configure(_config, this, locator);
        }

        public void RegisterSingleton<T>(Func<T> func)
            where T : class
        {
            _registrar.Register<T>(x => func());
        }

        public void RegisterSingleton<T>(string name, Func<T> func)
            where T : class
        {
            _registrar.Register<T>(x => func(), name);
        }
    }
}