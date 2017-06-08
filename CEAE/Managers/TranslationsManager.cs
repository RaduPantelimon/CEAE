using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace CEAE.Managers
{
    public class TranslationsManager : ActionFilterAttribute
    {
        private static string Translation => ConfigurationManager.AppSettings["lang"] ?? "en";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var culture = new CultureInfo(Translation);

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            base.OnActionExecuting(filterContext);
        }
    }
}