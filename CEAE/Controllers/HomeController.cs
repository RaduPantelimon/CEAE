using CEAE.Models;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace CEAE.Controllers
{
    public class HomeController : Controller
    {
        private readonly CEAEDBEntities _db = new CEAEDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            ViewData["hasLayout"] = false;
            return PartialView("_Test");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult TakeTest()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GenerateReport()
        {
            try
            {
                HttpContext.Response.Clear();
                HttpContext.Response.ContentType = "application/ms-excel";
                HttpContext.Response.AddHeader("Content-Disposition",
                "attachment; filename=" + "Report.xlsx" + ";");
                byte[] array = Utils.ExcelReportGenerator.GenerateExcelReportForContacts(_db);

                HttpContext.Response.OutputStream.Write(array, 0, array.Length);
                HttpContext.Response.End();
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    return RedirectToAction("AccessDenied");

                Debug.WriteLine(ex);
                Debugger.Break();
            }

            return RedirectToAction("AccessDenied");
        }

        public ActionResult AccessDenied()
        {
            ViewBag.Message = "Your are not allowed to view this page.";

            return View();
        }
    }
}