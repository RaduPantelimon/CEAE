using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CEAE.Models;
using CEAE.Utils;
namespace CEAE.Controllers
{
    public class UsersController : Controller
    {
        private CEAEDBEntities db = new CEAEDBEntities();

        // GET: Users
        public ActionResult Index()
        {
            string userPermissions = Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL] != null ?
                Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL].ToString() : "";
            if (userPermissions != CONST.USER_PERMISSIONS.Administrator) return RedirectToAction("AccessDenied", "Home");
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            //checking if the user has the necessary permissions
            string userPermissions = Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL] != null ?
                Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL].ToString() : "";
            int userID = Convert.ToInt32(Session[CONST.SESSION_VARS.USER_ID].ToString());
            if ((userID != id.Value) && userPermissions != CONST.USER_PERMISSIONS.Administrator) return RedirectToAction("AccessDenied", "Home");

            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Account,Password,FirstName,LastName,Title,Email,PhoneNumber,Administrator,ImgPath")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            //checking if the user has the necessary permissions
            string userPermissions = Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL].ToString();
            int userID = Convert.ToInt32(Session[CONST.SESSION_VARS.USER_ID].ToString());
            if ((userID != id.Value) && userPermissions != CONST.USER_PERMISSIONS.Administrator) return RedirectToAction("AccessDenied", "Home");

            ViewBag.permissions = userPermissions;

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Account,Password,FirstName,LastName,Title,Email,PhoneNumber,Administrator,ImgPath")] User user)
        {
            if (ModelState.IsValid)
            {


                //checking if the user has the necessary permissions
                string userPermissions = Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL].ToString();
                int userID = Convert.ToInt32(Session[CONST.SESSION_VARS.USER_ID].ToString());
                if ((user.UserID  != userID) && userPermissions != CONST.USER_PERMISSIONS.Administrator) return RedirectToAction("AccessDenied", "Home");

                ViewBag.permissions = userPermissions;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }



            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            string userPermissions = Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL].ToString();
            int userID = Convert.ToInt32(Session[CONST.SESSION_VARS.USER_ID].ToString());
            if ((user.UserID != userID) && userPermissions != CONST.USER_PERMISSIONS.Administrator) return RedirectToAction("AccessDenied", "Home");

            ViewBag.permissions = userPermissions;

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
            {
                string userPermissions = Session[CONST.SESSION_VARS.USER_ACCESS_LEVEL].ToString();
                int userID = Convert.ToInt32(Session[CONST.SESSION_VARS.USER_ID].ToString());
                if ((user.UserID != userID) && userPermissions != CONST.USER_PERMISSIONS.Administrator) return View("AccessDenied", "Home");
                ViewBag.permissions = userPermissions;
            }
            

            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
