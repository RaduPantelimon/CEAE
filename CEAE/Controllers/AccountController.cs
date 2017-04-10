using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CEAE.Models;
using CEAE.Utils;
using System.Data.Entity;

namespace CEAE.Controllers
{

    public class AccountController : Controller
    {
        private CEAEDBEntities db = new CEAEDBEntities();

        public AccountController()
        {
        }


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
            Session[CONST.SESSION_VARS.USER_ID] = null;
            Session[CONST.SESSION_VARS.USER_ACCOUNT] = null;
            Session[CONST.SESSION_VARS.USER_ISAUTHENTICATED] = false;
            Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL] = null;

            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult ManageAccount()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = PerformAuthentification(model);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
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
            if (Session[CONST.SESSION_VARS.USER_ISAUTHENTICATED] != null &&
                Convert.ToBoolean(Session[CONST.SESSION_VARS.USER_ISAUTHENTICATED]) == true)
            {
                return View();
            }

            return View("AccessDenied", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword([Bind(Include = "OldPassword,Password,ConfirmPassword")] ResetPasswordViewModel passmodel)
        {
            if (Session[CONST.SESSION_VARS.USER_ACCOUNT] != null &&
                Session[CONST.SESSION_VARS.USER_ISAUTHENTICATED] != null &&
                Convert.ToBoolean(Session[CONST.SESSION_VARS.USER_ISAUTHENTICATED]) == true)
            {
                if (ModelState.IsValid)
                {

                    string account = Session[CONST.SESSION_VARS.USER_ACCOUNT].ToString();
                    User currentUser = db.Users.Where(x => x.Account == account && x.Password == passmodel.OldPassword).FirstOrDefault();
                    if (currentUser == null)
                    {
                        ModelState.AddModelError("", "Either the 2 passwords do not match or the old password is not valid");
                        ViewBag.error = "Either the 2 passwords do not match or the old password is not valid";
                        return View(passmodel);
                    }
                    if(passmodel.Password == passmodel.ConfirmPassword)
                    {
                        currentUser.Password = passmodel.Password;
                        db.Entry(currentUser).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Either the 2 passwords do not match or the old password is not valid");
                ViewBag.error = "Either the 2 passwords do not match or the old password is not valid";
                return View(passmodel);
            }

            return View("AccessDenied", "Home");
        }


        #region Helpers
        // Used for XSRF protection when adding external logins


        private  SignInStatus PerformAuthentification(LoginViewModel model)
        {
            //validating the credentials
            User loggedInUser = db.Users.Where(x => (x.Account == model.Email ||
                                                      x.Email == model.Email ) && x.Password == model.Password).FirstOrDefault();
            if (loggedInUser != null)
            {
                //initializing the session variables
                Session[CONST.SESSION_VARS.USER_ID] = loggedInUser.UserID;
                Session[CONST.SESSION_VARS.USER_ACCOUNT] = loggedInUser.Account;
                Session[CONST.SESSION_VARS.USER_ISAUTHENTICATED] = true;
                Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL] = loggedInUser.Administrator;
                return SignInStatus.Success;
            }
            return SignInStatus.Failure;
        }
        private void PerformLogOff(LoginViewModel model)
        {
            //reseting session authentication data
            Session[CONST.SESSION_VARS.USER_ID] = null;
            Session[CONST.SESSION_VARS.USER_ACCOUNT] = null;
            Session[CONST.SESSION_VARS.USER_ISAUTHENTICATED] = false;
            Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL] = null;
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}