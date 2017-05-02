using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CEAE.Models;

namespace CEAE.Controllers
{
    public class CausesController : Controller
    {
        private readonly CEAEDBEntities db = new CEAEDBEntities();

        // GET: Causes
        public ActionResult Index()
        {
            return View(db.Causes.ToList());
        }

        // GET: Causes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var causes = db.Causes.Find(id);
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
            if (ModelState.IsValid)
            {
                db.Causes.Add(causes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(causes);
        }

        // GET: Causes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var causes = db.Causes.Find(id);
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
            if (ModelState.IsValid)
            {
                db.Entry(causes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(causes);
        }

        // GET: Causes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var causes = db.Causes.Find(id);
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
            var causes = db.Causes.Find(id);
            db.Causes.Remove(causes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}