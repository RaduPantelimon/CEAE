namespace CEAE.Utils
{
    public static class Constants
    {
        public static readonly string[] AnswerOptions = {"Corect", "Gresit"};
        

        //session variable names used by the app
        public static class Session
        {
            public const string UserId = "USER_ID";
            public const string UserAccount = "USER_ACCOUNT";
            public const string UserIsAuthenticated = "USER_ISAUTHENTICATED";
            public const string UserAccessLevel = "USER_ACCESS_LEVEL";
            public const string DidRegisterEmail = "DID_REGISTER_EMAIL";
            public const string RegisteredEmail = "REGISTERED_EMAIL";
            public const string RegisteredID = "REGISTERED_ID";
        }

        //
        public static class Permissions
        {
            public static readonly string[] Order = { User, Administrator };
            public const string Administrator = "Administrator";
            public const string User = "User";
        }

        public static class AnswerResponses
        {
            public const string Corect = "Corect";
            public const string Gresit = "Gresit";
        }
    }
}