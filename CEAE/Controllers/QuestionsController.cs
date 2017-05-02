using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CEAE.Models;
using Newtonsoft.Json;

namespace CEAE.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly CEAEDBEntities db = new CEAEDBEntities();

        // GET: Questions
        public ActionResult Index()
        {
            return View(db.Questions.ToList());
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();

            var answers = db.Answers /*.Select(x => (x.AnswersQuestions = null))*/.ToList();
            for (var i = 0; i < answers.Count; i++)
                answers[i].AnswersQuestions = null;
            ViewBag.possibleAnswers = answers;
            ViewBag.serializedAnswers = JsonConvert.SerializeObject(answers);

            return View(question);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionID,Title,Text")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionID,Title,Text")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var question = db.Questions.Find(id);
            db.Questions.Remove(question);
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