using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CEAE.Models;
using CEAE.Utils;

namespace CEAE.Controllers
{
    public class UsersController : Controller
    {
        private readonly CEAEDBEntities db = new CEAEDBEntities();

        // GET: Users
        public ActionResult Index()
        {
            var userPermissions = Session[Constants.Session.UserAccessLevel] != null
                ? Session[Constants.Session.UserAccessLevel].ToString()
                : "";
            if (userPermissions != Constants.Permissions.Administrator)
                return RedirectToAction("AccessDenied", "Home");
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            //checking if the user has the necessary permissions
            var userPermissions = Session[Constants.Session.UserAccessLevel] != null
                ? Session[Constants.Session.UserAccessLevel].ToString()
                : "";
            var userID = Convert.ToInt32(Session[Constants.Session.UserId].ToString());
            if (userID != id.Value && userPermissions != Constants.Permissions.Administrator)
                return RedirectToAction("AccessDenied", "Home");

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
        public ActionResult Create(
            [Bind(Include = "UserID,Account,Password,FirstName,LastName,Title,Email,PhoneNumber,Administrator,ImgPath")]
            User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            //checking if the user has the necessary permissions
            var userPermissions = Session[Constants.Session.UserAccessLevel].ToString();
            var userID = Convert.ToInt32(Session[Constants.Session.UserId].ToString());
            if (userID != id.Value && userPermissions != Constants.Permissions.Administrator)
                return RedirectToAction("AccessDenied", "Home");

            ViewBag.permissions = userPermissions;

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "UserID,Account,Password,FirstName,LastName,Title,Email,PhoneNumber,Administrator,ImgPath")]
            User user)
        {
            if (ModelState.IsValid)
            {
                //checking if the user has the necessary permissions
                var userPermissions = Session[Constants.Session.UserAccessLevel].ToString();
                var userID = Convert.ToInt32(Session[Constants.Session.UserId].ToString());
                if (user.UserID != userID && userPermissions != Constants.Permissions.Administrator)
                    return RedirectToAction("AccessDenied", "Home");

                ViewBag.permissions = userPermissions;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }


            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();
            var userPermissions = Session[Constants.Session.UserAccessLevel].ToString();
            var userID = Convert.ToInt32(Session[Constants.Session.UserId].ToString());
            if (user.UserID != userID && userPermissions != Constants.Permissions.Administrator)
                return RedirectToAction("AccessDenied", "Home");

            ViewBag.permissions = userPermissions;

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                var userPermissions = Session[Constants.Session.UserAccessLevel].ToString();
                var userID = Convert.ToInt32(Session[Constants.Session.UserId].ToString());
                if (user.UserID != userID && userPermissions != Constants.Permissions.Administrator)
                    return RedirectToAction("AccessDenied", "Home");
                ViewBag.permissions = userPermissions;
            }


            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}