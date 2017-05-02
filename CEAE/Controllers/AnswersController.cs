using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CEAE.Models;
using CEAE.Utils;

namespace CEAE.Controllers
{
    [UserPermissionExact(Constants.Permissions.Administrator)]
    public class AnswersController : Controller
    {
        private readonly CEAEDBEntities _db = new CEAEDBEntities();

        // GET: Answers
        public ActionResult Index()
        {
            return View(_db.Answers.ToList());
        }

        // GET: Answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var answer = _db.Answers.Find(id);
            if (answer == null)
                return HttpNotFound();
            return View(answer);
        }

        // GET: Answers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnswerID,Title,Text")] Answer answer)
        {
            if (!ModelState.IsValid)
                return View(answer);

            _db.Answers.Add(answer);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Answers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var answer = _db.Answers.Find(id);
            if (answer == null)
                return HttpNotFound();
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnswerID,Title,Text")] Answer answer)
        {
            if (!ModelState.IsValid)
                return View(answer);

            _db.Entry(answer).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Answers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var answer = _db.Answers.Find(id);
            if (answer == null)
                return HttpNotFound();
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var answer = _db.Answers.Find(id);

            if (answer == null)
                return RedirectToAction("Index");

            _db.Answers.Remove(answer);
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