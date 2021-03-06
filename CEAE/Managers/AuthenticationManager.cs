using System;
using System.Web;
using CEAE.Models;
using CEAE.Utils;
using DevOne.Security.Cryptography.BCrypt;
using Microsoft.AspNet.Identity.Owin;

namespace CEAE.Managers
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

            Reauthenticate(loggedInUser, session);
            return SignInStatus.Success;
        }

        public static void ResetPassword(User user, string password)
        {
            user.Password = PasswordEncrypt(password);

        }

        private static bool PasswordsMatch(string encryptedPassword, string decryptedPassword)
        {
            var decrypted = encryptedPassword.Equals(decryptedPassword, StringComparison.CurrentCulture);

            if (decrypted)
                return true;

            try
            {
                return BCryptHelper.CheckPassword(decryptedPassword, encryptedPassword);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string PasswordEncrypt(string password)
        {
            return BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt());
        }


        public static bool IsUserAdministrator(HttpSessionStateBase session)
        {
            return IsUserAuthorized(session, Constants.Permissions.Administrator);
        }

        public static bool IsUserAuthorized(HttpSessionStateBase session, string matchingSecurity)
        {
            return IsUserAuthenticated(session) && matchingSecurity.Equals(CurrentSecurity(session));
        }

        public static bool IsUserAuthorizedGreaterOrEqual(HttpSessionStateBase session, string matchingSecurity)
        {
            if (!IsUserAuthenticated(session))
                return false;

            var currentIndex = Array.IndexOf(Constants.Permissions.Order, CurrentSecurity(session));
            var matchingIndex = Array.IndexOf(Constants.Permissions.Order, matchingSecurity);

            return matchingIndex != -1 && currentIndex >= matchingIndex;
        }

        private static string CurrentSecurity(HttpSessionStateBase session)
        {
            return session[Constants.Session.UserAccessLevel].ToString();
        }

        public static int UserId(HttpSessionStateBase session)
        {
            if (session[Constants.Session.UserId] is int)
                return (int)session[Constants.Session.UserId];
            return -1;
        }

        public static string UserAccessLevel(HttpSessionStateBase session)
        {
            return session[Constants.Session.UserAccessLevel]?.ToString() ?? "";
        }

        public static string UserEmail(HttpSessionStateBase session)
        {
            // we can skip "is string" as string isn't a primitive.
            return session[Constants.Session.RegisteredEmail] as string;
        }

        public static void Reauthenticate(User existingUser, HttpSessionStateBase session)
        {
            session[Constants.Session.UserId] = existingUser.UserID;
            session[Constants.Session.UserAccount] = existingUser.Account;
            session[Constants.Session.UserIsAuthenticated] = true;
            session[Constants.Session.UserAccessLevel] = existingUser.Administrator;
            session[Constants.Session.RegisteredEmail] = existingUser.Email;
        }
    }
}