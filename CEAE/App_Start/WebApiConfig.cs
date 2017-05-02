using System.Web.Http;

namespace CEAE
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "RestApiRoot",
                "api/Rest/{action}/{id}",
                new {controller = "Rest", action = "GetAnswersQuestions", id = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );
        }
    }
}