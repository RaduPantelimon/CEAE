using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CEAE;
using CEAE.Models;
using CEAE.Controllers;
using System.Web;
using System.Security.Principal;
using System.Web.Routing;
using System;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class AccountControllerTests
    {
        public class MockHttpContext : HttpContextBase
        {

            private readonly HttpRequestBase _request = new MockHttpRequest();
            private readonly HttpServerUtilityBase _server = new MockHttpServerUtilityBase();
            private HttpSessionStateBase _session = new MockHttpSession();

            public override HttpRequestBase Request
            {
                get { return _request; }
            }
            public override HttpServerUtilityBase Server
            {
                get { return _server; }
            }
            public override HttpSessionStateBase Session
            {
                get { return _session; }
            }
        }

        public class MockHttpRequest : HttpRequestBase
        {
            private Uri _url = new Uri("http://www.mockrequest.moc/Controller/Action");

            public override Uri Url
            {
                get { return _url; }
            }
        }

        public class MockHttpServerUtilityBase : HttpServerUtilityBase
        {
            public override string UrlEncode(string s)
            {
                //return base.UrlEncode(s);     
                return s;       // Not doing anything (this is just a Mock)
            }
        }


        public class MockHttpSession : HttpSessionStateBase
        {
            // Started with sample http://stackoverflow.com/questions/524457/how-do-you-mock-the-session-object-collection-using-moq
            // from http://stackoverflow.com/users/81730/ronnblack

            Dictionary<string, object> _sessionStorage = new Dictionary<string, object>();
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
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
         
            // Arrange
            var LoginViewModel = new LoginViewModel();


            // Act
            var controller = new AccountController();
            var request = new RequestContext(new MockHttpContext(), new RouteData());

            controller.ControllerContext = new ControllerContext()
            {
                Controller = controller,
                RequestContext = request
            };
            var result = controller.Login(LoginViewModel, "Success") as ViewResult;


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


            // mocuiesc user si vad daca authmanager 
            // creez un nou user care vine din models.user si vad daca cand trimit in auth manager cu parola "cutare" 
            // authenticate -mock 
            // public static SignInStatus Authenticate(User loggedInUser, string password, HttpSessionStateBase session) sa returneze success

            // Arrange   
            var LoginViewModel = new LoginViewModel();

            var NewUser = new UsersController();

            Authenticate(NewUser,password, HttpSessionStateBase session).Returnes(true);


            var model = new LoginViewModel() { Email = "", Password = "" };
            var controller = new AccountController(NewUser.Object, ceva.Object);
            controller.ModelState.AddModelError("key", "error message");
            // Act
            var Acontroller = new AccountController();
            var result = Acontroller.Login(model, "") as ViewResult;

            // Assert
            Ceva.Verify(f => f.SignIn(model.UserName, model.RememberMe));
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            // 
        }
    }

}
