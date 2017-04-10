using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CEAE.Utils
{
    public class CONST
    {
        //names of cokies used by the app
        public partial class COOKIES
        {
            const string AUTH_COOKIE = "AUTH_COOKIE";
        }

        //session variable names used by the app
        public partial class SESSION_VARS
        {
            public static readonly string DID_RECEIVED_EMAIL = "DID_RECEIVED_EMAIL";
            public static readonly string REGISTRED_EMAIL = "REGISTERED_EMAIL";
            public static readonly string USER_ID = "USER_ID";
            public static readonly string USER_ACCOUNT = "USER_ACCOUNT";
            public static readonly string USER_ISAUTHENTICATED = "USER_ISAUTHENTICATED";
            public static readonly string USER_ACCESS_LEVEL = "USER_ACCESS_LEVEL";
        }

        //
        public partial class USER_PERMISSIONS
        {
            public static string Member = "Member";
            public static string User = "User";
            public static string Administrator = "Administrator";
        }

        public partial class ANSWER_RESPONSES
        {
            public static readonly string Corect = "Corect";
            public static readonly string Gresit = "Gresit";
        }
        public static readonly string [] ANSWER_STATUSES = {"Corect","Gresit"};

    }
}