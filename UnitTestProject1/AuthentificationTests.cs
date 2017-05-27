using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CEAE.Models;
using CEAE.Controllers;
using System.Web;
using System.Web.Routing;
using System;
using System.Collections.Generic;
using CEAE.Managers;
using DevOne.Security.Cryptography.BCrypt;

namespace UnitTestProject1
{
    [TestClass]
    public class AuthentificationTests
    {
        private class MockHttpContext : HttpContextBase
        {
            public override HttpRequestBase Request { get; } = new MockHttpRequest();

            public override HttpServerUtilityBase Server { get; } = new MockHttpServerUtilityBase();

            public override HttpSessionStateBase Session { get; } = new MockHttpSession();
        }

        private class MockHttpRequest : HttpRequestBase
        {
            public override Uri Url { get; } = new Uri("http://www.mockrequest.moc/Controller/Action");
        }

        private class MockHttpServerUtilityBase : HttpServerUtilityBase
        {
            public override string UrlEncode(string s)
            {
                //return base.UrlEncode(s);     
                return s;       // Not doing anything (this is just a Mock)
            }
        }


        private class MockHttpSession : HttpSessionStateBase
        {
            // Started with sample http://stackoverflow.com/questions/524457/how-do-you-mock-the-session-object-collection-using-moq

            readonly Dictionary<string, object> _sessionStorage = new Dictionary<string, object>();
            public override object this[string name]
            {
                get { return _sessionStorage[name]; }
                set { _sessionStorage[name] = value; }
            }

            public override void Add(string name, object value)
            {
                _sessionStorage[name] = value;
            }
            public override int Count => _sessionStorage.Count;
        }

        [TestMethod]
        public void ShouldNotAcceptInvalidUser()
        {
            var _ = System.Data.Entity.SqlServer.SqlProviderServices.Instance; // so we have the reference.
         
            // Arrange
            var loginViewModel = new LoginViewModel();


            // Act
            var controller = new AccountController();
            var request = new RequestContext(new MockHttpContext(), new RouteData());

            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                RequestContext = request
            };

            controller.Login(loginViewModel, "Success");


            // Assert
            Assert.AreEqual(0, controller.Session.Count);
            Assert.IsFalse(controller.ModelState.IsValid);
    
            //Assert.That(result.ViewName, Is.EqualTo("Index"));
            //Assert.That(controller.ModelState[""],
            //            Is.EqualTo("The user name or password provided is incorrect."));
        }

        [TestMethod]
        public void SignedInStatusSuccess()
        {

            var user = new User
            {
                Password = "test"
            };

            var status = AuthenticationManager.ValidateUserLogin(user, "test");

            Assert.IsTrue(status);

        }

        [TestMethod]
        public void SignedInStatusSuccessEncryptedPassword()
        {

            var user = new User
            {
                Password = BCryptHelper.HashPassword("test", BCryptHelper.GenerateSalt())
            };

            var status = AuthenticationManager.ValidateUserLogin(user, "test");

            Assert.IsTrue(status);

        }

        [TestMethod]
        public void SignedInStatusBadPasswordClear()
        {

            var user = new User
            {
                Password = "not working"
            };

            var status = AuthenticationManager.ValidateUserLogin(user, "test");

            Assert.IsFalse(status);

        }

        [TestMethod]
        public void SignedInStatusBadPasswordEncrypted()
        {

            var user = new User
            {
                Password = BCryptHelper.HashPassword("not working", BCryptHelper.GenerateSalt())
            };

            var status = AuthenticationManager.ValidateUserLogin(user, "test");

            Assert.IsFalse(status);

        }

    }

}
