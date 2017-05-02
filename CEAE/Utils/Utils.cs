using System;
using System.Net.Mail;

namespace CEAE.Utils
{
    public static class Utils
    {
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                var m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}