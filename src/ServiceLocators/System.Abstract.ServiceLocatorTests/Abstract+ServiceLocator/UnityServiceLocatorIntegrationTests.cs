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
using System.Abstract.Fakes;
using Xunit;
namespace Contoso.Abstract
{
    public class UnityServiceLocatorIntegrationTests : ServiceLocatorIntegrationTestsBase
    {
        protected override IServiceLocator CreateServiceLocator()
        {
            return new UnityServiceLocator();
            //var container = new UnityContainer();
            //var serviceType = typeof(TestService);
            //var serviceType2 = typeof(TestService2);
            //container.RegisterType(typeof(ITestService), serviceType, new InjectionMember[0]);
            //container.RegisterType(typeof(ITestService), serviceType, serviceType.FullName, new InjectionMember[0]);
            //container.RegisterType(typeof(ITestService), serviceType2, serviceType2.FullName, new InjectionMember[0]);
            //return new UnityServiceLocator(container);
        }

        //[TestMethod]
        //public void Inject_Should_Set_Dependencies_On_Instance_When_Dependencies_Are_Not_Defined_On_The_Interface_And_Resolved_As_The_Interface_Type()
        //{
        //    Registrar.Register(typeof(ITestDependency), typeof(TestDependency));
        //    //
        //    var serviceAsInterface = (ITestService)new TestServiceWithServiceDependency();
        //    Locator.Inject(serviceAsInterface);
        //    //
        //    var service = (TestServiceWithServiceDependency)serviceAsInterface;
        //    Assert.IsNotNull(service.DependencyThatDoesNotExistOnInterface);
        //}

        // skip this test
        public override void ResolveAll_Should_Return_All_Registered_UnNamed_Services() { }
    }
}
