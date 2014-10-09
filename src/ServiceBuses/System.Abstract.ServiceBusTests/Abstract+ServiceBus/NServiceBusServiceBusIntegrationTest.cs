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
using Contoso.Abstract.NServiceBusFakes;
using NServiceBus;
using NServiceBus.Unicast.Transport;
using System.Abstract;
using System.Linq;
using System.Reflection;
#if CLR45
using NServiceBusBus = NServiceBus.Bus;
#endif
namespace Contoso.Abstract
{
    public class NServiceBusServiceBusIntegrationTest : ServiceBusIntegrationTestsBase
    {
        protected override IServiceBus CreateServiceBus()
        {
            try
            {
                var bus = DefaultBusCreator(typeof(NServiceBusServiceBusIntegrationTest).Assembly);
                return new NServiceBusAbstractor((IServiceLocator)null, (IStartableBus)bus);
            }
            catch (System.Exception) { }
            return null; // new NServiceBusAbstractor(bus);
        }

        //[Fact]
        //public void TestMethod1()
        //{
        //    //var testMessage = ServiceBus.MakeMessage<TestMessage>();
        //    //testMessage.Name = "Test";
        //    //ServiceBus.SendTo(testMessage);
        //}

        public static IStartableBus DefaultBusCreator(params Assembly[] assemblies)
        {
#if !CLR4
            return Configure.With(new[] { typeof(CompletionMessage).Assembly }.Union(assemblies))
                .DefaultBuilder()
                .XmlSerializer()
                .FakeTransport()
                .UnicastBus()
                .CreateBus();
#elif !CLR45
            return Configure.With(assemblies)
                .DefaultBuilder()
                .FakeTransport()
                .UnicastBus()
                .CreateBus();
#else
            var configuration = new BusConfiguration();
            configuration.AssembliesToScan(assemblies);
            //configuration.DefaultBuilder();
            //configuration.UseTransport<FakeTransport>()
            return NServiceBusBus.Create(configuration);
#endif
        }
    }
}
