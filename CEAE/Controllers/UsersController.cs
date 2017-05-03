using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using CEAE.Models;
using CEAE.Utils;
using Microsoft.AspNet.Identity.Owin;
using AuthenticationManager = CEAE.Managers.AuthenticationManager;

namespace CEAE.Controllers
{
    public class UsersController : Controller
    {
        private readonly CEAEDBEntities _db = new CEAEDBEntities();

        [UserPermissionExact(Constants.Permissions.Administrator)]
        // GET: Users
        public ActionResult Index()
        {
            return View(_db.Users.ToList());
        }

        [UserPermissionExact(Constants.Permissions.Administrator)]
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = _db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

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
        public ActionResult Create([Bind(Exclude = "")] Models.DTO.User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            // always default to a simple user.
            var newUser = Mapper.Map<User>(user);


            _db.Users.Add(newUser);
            _db.SaveChanges();

            

            return AuthenticationManager.Authenticate(newUser, user.Password, Session) == SignInStatus.Success ? 
                (ActionResult) RedirectToAction("Index", "Home") :
                View(user);
        }


        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = _db.Users.Find(id);
            if (user == null)
                return HttpNotFound();
            
            if (!CanUseAction(id.Value))
                return RedirectToAction("AccessDenied", "Home");

            ViewBag.permissions = AuthenticationManager.UserAccessLevel(Session);

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
            if (!ModelState.IsValid)
                return View(user);

            if (!CanUseAction(user.UserID))
                return RedirectToAction("AccessDenied", "Home");

            ViewBag.permissions = AuthenticationManager.UserAccessLevel(Session);

            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private bool CanUseAction(int userId)
        {
            var sameUser = AuthenticationManager.IsUserAuthenticated(Session) &&
                           AuthenticationManager.UserId(Session) == userId;
            var isUserAdministrator = AuthenticationManager.IsUserAdministrator(Session);

            return sameUser || isUserAdministrator;
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = _db.Users.Find(id);

            if (user == null)
                return HttpNotFound();

            if (!CanUseAction(id.Value))
                return RedirectToAction("AccessDenied", "Home");

            ViewBag.permissions = AuthenticationManager.UserAccessLevel(Session);

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = _db.Users.Find(id);
            if (user != null)
            {
                if (!CanUseAction(id))
                    return RedirectToAction("AccessDenied", "Home");

                ViewBag.permissions = AuthenticationManager.UserAccessLevel(Session);
                _db.Users.Remove(user);
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}