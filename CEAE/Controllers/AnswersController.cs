using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
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
            var models = Mapper.Map<List<Models.DTO.Answer>>(_db.Answers.ToList());
            return View(models);
        }

        // GET: Answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var answer = _db.Answers.Find(id);
            if (answer == null)
                return HttpNotFound();

            var model = Mapper.Map<Models.DTO.Answer>(answer);

            return View(model);
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
        public ActionResult Create([Bind(Include = "Title,Text")] Models.DTO.Answer answer)
        {
            if (!ModelState.IsValid)
                return View(answer);

            var realAnswer = Mapper.Map<Answer>(answer);

            _db.Answers.Add(realAnswer);
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

            var model = Mapper.Map<Models.DTO.Answer>(answer);

            return View(model);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnswerID,Title,Text")] Models.DTO.Answer answer)
        {
            if (!ModelState.IsValid)
                return View(answer);

            var realAnswer = Mapper.Map<Answer>(answer);

            _db.Entry(realAnswer).State = EntityState.Modified;
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

            var model = Mapper.Map<Models.DTO.Answer>(answer);

            return View(model);
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