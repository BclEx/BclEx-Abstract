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
using Contoso.Abstract.Micro.ServiceBus.Modules;
using System;
using System.Abstract;
using System.Abstract.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
namespace Contoso.Abstract.Micro.ServiceBus.Impl
{
    public class AbstractMicroServiceBusConfiguration
    {
        private ServiceBusConfiguration _configurationElement;
        public bool consumeInTxn;
        private readonly List<Type> _messageModules;
        private Action _readConfiguration;
        protected List<Assembly> _scanAssemblies;

        protected AbstractMicroServiceBusConfiguration()
        {
            Action action = null;
            _messageModules = new List<Type>();
            ConsumeInTransaction = true;
            Assemblies = new List<Assembly>(new Assembly[] { typeof(IServiceBus).Assembly });
            if (action == null)
                action = () => BusConfiguration = ConfigurationManager.GetSection("rhino.esb") as ServiceBusConfiguration;
            _readConfiguration = action;
            ThreadCount = 1;
            NumberOfRetries = 5;
        }

        public AbstractMicroServiceBusConfiguration AddMessageModule<TModule>()
            where TModule : IMicroMessageModule
        {
            _messageModules.Add(typeof(TModule));
            return this;
        }

        protected abstract void ApplyConfiguration();
        public void BuildWith(IServiceLocatorBuilder builder)
        {
            Builder = builder;
            //Builder.WithInterceptor(new ConsumerInterceptor());
        }

        public virtual void Configure()
        {
            ReadBusConfiguration();
            ApplyConfiguration();
            Builder.RegisterDefaultServices(Assemblies);
        }

        public AbstractMicroServiceBusConfiguration DisableQueueAutoCreation()
        {
            DisableAutoQueueCreation = true;
            return this;
        }

        public AbstractMicroServiceBusConfiguration InsertMessageModuleAtFirst<TModule>()
            where TModule : IMicroMessageModule
        {
            _messageModules.Insert(0, typeof(TModule));
            return this;
        }

        protected virtual void ReadBusConfiguration()
        {
            if (BusConfiguration == null)
            {
                _readConfiguration();
                if (BusConfiguration == null)
                    throw new InvalidOperationException("could not find servicebus configuration element");
            }
        }

        public AbstractMicroServiceBusConfiguration UseConfiguration(ServiceBusConfiguration busConfiguration)
        {
            BusConfiguration = busConfiguration;
            return this;
        }

        public AbstractMicroServiceBusConfiguration UseStandaloneConfigurationFile(string fileName)
        {
            _readConfiguration = delegate
            {
                BusConfiguration = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(fileName)).GetSection("rhino.esb") as ServiceBusConfiguration;
            };
            return this;
        }

        public IEnumerable<Assembly> Assemblies { get; private set; }
        protected IServiceLocatorBuilder Builder { get; private set; }
        public ServiceBusConfiguration BusConfiguration { get; private set; }
        public bool ConsumeInTransaction { get; private set; }
        public bool DisableAutoQueueCreation { get; set; }
        public Uri Endpoint { get; set; }

        public IEnumerable<Type> MessageModules
        {
            get { return new ReadOnlyCollection<Type>(_messageModules); }
        }

        public int NumberOfRetries { get; set; }
        public int ThreadCount { get; set; }
        public bool UseFlatQueue { get; set; }
    }
}