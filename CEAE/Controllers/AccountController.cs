using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CEAE.Managers;
using CEAE.Models;
using CEAE.Utils;
using Microsoft.AspNet.Identity.Owin;

namespace CEAE.Controllers
{
    public class AccountController : Controller
    {
        private readonly CEAEDBEntities _db = new CEAEDBEntities();


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            //reseting session authentication data
            AuthenticationManager.Deauthenticate(Session);

            return RedirectToAction("Index", "Home");
        }

        [UserPermissionGreaterOrEqual(Constants.Permissions.User)]
        public ActionResult ManageAccount()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = PerformAuthentification(model);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                default:
                    ModelState.AddModelError("", Translations.InvalidLogin);
                    return View(model);
            }
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return RedirectToAction("Create", "Users");
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            if (AuthenticationManager.IsAuthenticated(Session))
                return View();

            return RedirectToAction("AccessDenied", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(
            [Bind(Include = "OldPassword,Password,ConfirmPassword")] ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!AuthenticationManager.IsUserAuthenticated(Session))
                return RedirectToAction("AccessDenied", "Home");

            if (ModelState.IsValid)
            {
                var account = AuthenticationManager.UserAccount(Session);
                var currentUser = _db.Users.FirstOrDefault(x => x.Account == account);
                if (!AuthenticationManager.ValidateUserLogin(currentUser, resetPasswordViewModel.OldPassword))
                {
                    ModelState.AddModelError("",
                        "Either the 2 passwords do not match or the old password is not valid");
                    ViewBag.error = "Either the 2 passwords do not match or the old password is not valid";
                    return View(resetPasswordViewModel);
                }

                if (resetPasswordViewModel.Password == resetPasswordViewModel.ConfirmPassword)
                {
                    AuthenticationManager.ResetPassword(currentUser, resetPasswordViewModel.Password);
                    _db.Entry(currentUser).State = EntityState.Modified;
                    _db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Either the 2 passwords do not match or the old password is not valid");
            ViewBag.error = "Either the 2 passwords do not match or the old password is not valid";
            return View(resetPasswordViewModel);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private SignInStatus PerformAuthentification(LoginViewModel model)
        {
            //validating the credentials
            var user = _db.Users.FirstOrDefault(x => x.Account == model.Email || x.Email == model.Email);
            return AuthenticationManager.Authenticate(user, model.Password, Session);
        }
        
        
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}