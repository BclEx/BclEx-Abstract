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
    public class AutofacServiceRegistrarIntegrationTests : ServiceRegistrarIntegrationTestsBase
    {
        protected override IServiceLocator CreateServiceLocator() { return new AutofacServiceLocator(); }

        //[TestMethod]
        //public void Registering_with_implementation_should_override_the_previous_registration()
        //{
        //    Registrar.Register<IRepository, ProductRepository>();
        //    Registrar.Register<IRepository, AccountRepository>();
        //    var service = Locator.Resolve<IRepository>();
        //    Assert.IsInstanceOfType(service, typeof(AccountRepository));
        //}

        //[TestMethod]
        //public void Registering_with_implementation_should_override_the_previous_registration_when_resolve_is_called_inbetween()
        //{
        //    Registrar.Register<IRepository, ProductRepository>();
        //    Locator.Resolve<IRepository>();
        //    Registrar.Register<IRepository, AccountRepository>();
        //    var service = Locator.Resolve<IRepository>();
        //    Assert.IsInstanceOfType(service, typeof(AccountRepository));
        //}

        //[TestMethod]
        //public void Registering_with_specified_type_should_override_the_previous_registration_when_resolve_is_called_inbetween()
        //{
        //    Registrar.Register<IRepository>(typeof(ProductRepository));
        //    Locator.Resolve<IRepository>();
        //    Registrar.Register<IRepository>(typeof(AccountRepository));
        //    var service = Locator.Resolve<IRepository>();
        //    Assert.IsInstanceOfType(service, typeof(AccountRepository));
        //}

        //[TestMethod]
        //public void Registering_with_keyed_implementation_should_override_the_previous_registration_when_resolve_is_called_inbetween()
        //{
        //    Registrar.Register<IRepository, ProductRepository>("key");
        //    Locator.Resolve<IRepository>("key");
        //    Registrar.Register<IRepository, AccountRepository>("key");
        //    var service = Locator.Resolve<IRepository>("key");
        //    Assert.IsInstanceOfType(service, typeof(AccountRepository));
        //}

        //[TestMethod]
        //public void Registering_with_specified_service_and_type_should_override_the_previous_registration_when_resolve_is_called_inbetween()
        //{
        //    Registrar.Register(typeof(IRepository), typeof(ProductRepository));
        //    Locator.Resolve<IRepository>();
        //    Registrar.Register(typeof(IRepository), typeof(AccountRepository));
        //    var service = Locator.Resolve<IRepository>();
        //    Assert.IsInstanceOfType(service, typeof(AccountRepository));
        //}
    }
}