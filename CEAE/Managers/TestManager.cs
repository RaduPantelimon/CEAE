using System;
using System.Net.Mail;
using System.Web;
using CEAE.Utils;
using Newtonsoft.Json;

namespace CEAE.Managers
{
    public static class TestManager
    {
        public static void SetEmail(HttpSessionStateBase session, string emailAddress, int contactId)
        {
            session[Constants.Session.DidRegisterEmail] = true;
            session[Constants.Session.RegisteredEmail] = emailAddress;
            session[Constants.Session.RegisteredID] = contactId;
        }

        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                var _ = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string JsonMessage(bool success, object message)
        {
            var result = new {status = success, message};
            var json = JsonConvert.SerializeObject(result);
            return json;
        }

        public static bool IsContactRegistered(HttpSessionStateBase session)
        {
            return session[Constants.Session.DidRegisterEmail] is bool &&
                   (bool) session[Constants.Session.DidRegisterEmail];
        }

        public static int ContactId(HttpSessionStateBase session)
        {
            return (int) session[Constants.Session.RegisteredID];
        }
    }
}