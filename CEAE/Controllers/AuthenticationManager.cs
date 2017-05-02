using System;
using System.Web;
using CEAE.Models;
using CEAE.Utils;
using Microsoft.AspNet.Identity.Owin;

namespace CEAE.Controllers
{
    public static class AuthenticationManager
    {
        public static bool IsAuthenticated(HttpSessionStateBase session)
        {
            var sessionVariable = session[Constants.Session.UserIsAuthenticated];
            return sessionVariable is bool && (bool) sessionVariable;
        }
        public static bool IsUserAuthenticated(HttpSessionStateBase session)
        {
            return IsAuthenticated(session) && session[Constants.Session.UserAccount] != null;
        }

        public static void Deauthenticate(HttpSessionStateBase session)
        {
            session[Constants.Session.UserId] = null;
            session[Constants.Session.UserAccount] = null;
            session[Constants.Session.UserIsAuthenticated] = false;
            session[Constants.Session.UserAccessLevel] = null;
        }
    
        public static string UserAccount(HttpSessionStateBase session)
        {
            return session[Constants.Session.UserAccount]?.ToString();
        }


        public static bool ValidateUserLogin(User user, string password)
        {
            return user != null && PasswordsMatch(user.Password, password);
        }

        public static SignInStatus Authenticate(User loggedInUser, string password, HttpSessionStateBase session)
        {
            if (!ValidateUserLogin(loggedInUser, password))
                return SignInStatus.Failure;

            session[Constants.Session.UserId] = loggedInUser.UserID;
            session[Constants.Session.UserAccount] = loggedInUser.Account;
            session[Constants.Session.UserIsAuthenticated] = true;
            session[Constants.Session.UserAccessLevel] = loggedInUser.Administrator;
            return SignInStatus.Success;
        }

        public static void ResetPassword(User user, string password)
        {
            user.Password = PasswordEncrypt(password);

        }

        private static bool PasswordsMatch(string encryptedPassword, string decryptedPassword)
        {
            // TODO: Validate BCrypt password.
            return encryptedPassword.Equals(decryptedPassword, StringComparison.CurrentCulture);
        }

        private static string PasswordEncrypt(string password)
        {
            // TODO: BCrypt password.
            return password;
        }


    }
}