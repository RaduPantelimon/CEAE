using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CEAE.Models;
using CEAE.Utils;

namespace CEAE.Controllers
{

    [UserPermissionExact(Constants.Permissions.Administrator)]
    public class CausesController : Controller
    {
        private readonly CEAEDBEntities _db = new CEAEDBEntities();

        // GET: Causes
        public ActionResult Index()
        {
            return View(_db.Causes.ToList());
        }

        // GET: Causes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var causes = _db.Causes.Find(id);
            if (causes == null)
                return HttpNotFound();
            return View(causes);
        }

        // GET: Causes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Causes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CauseID,Title,Text,StartDate,EndDate")] Causes causes)
        {
            if (!ModelState.IsValid)
                return View(causes);

            _db.Causes.Add(causes);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Causes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var causes = _db.Causes.Find(id);
            if (causes == null)
                return HttpNotFound();
            return View(causes);
        }

        // POST: Causes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CauseID,Title,Text,StartDate,EndDate")] Causes causes)
        {
            if (!ModelState.IsValid)
                return View(causes);

            _db.Entry(causes).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Causes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var causes = _db.Causes.Find(id);
            if (causes == null)
                return HttpNotFound();
            return View(causes);
        }

        // POST: Causes/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var causes = _db.Causes.Find(id);
            if (causes == null)
                return RedirectToAction("Index");

            _db.Causes.Remove(causes);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}