using System.Web.Mvc;
using CEAE.Managers;

namespace CEAE
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TranslationsManager(), 0);
        }
    }
}