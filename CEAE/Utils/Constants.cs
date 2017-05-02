namespace CEAE.Utils
{
    public static class Constants
    {
        public static string[] AnswerOptions { get; } = {"Corect", "Gresit"};

        //session variable names used by the app
        public static class Session
        {
            public static string UserId { get; } = "USER_ID";
            public static string UserAccount { get; } = "USER_ACCOUNT";
            public static string UserIsAuthenticated { get; } = "USER_ISAUTHENTICATED";
            public static string UserAccessLevel { get; } = "USER_ACCESS_LEVEL";
        }

        //
        public static class Permissions
        {
            public static string Administrator { get; } = "Administrator";
        }

        public static class AnswerResponses
        {
            public static string Corect { get; } = "Corect";
            public static string Gresit { get; } = "Gresit";
        }
    }
}