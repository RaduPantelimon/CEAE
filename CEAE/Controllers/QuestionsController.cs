using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CEAE.Models;
using CEAE.Utils;
using Newtonsoft.Json;
using CEAE.Managers;
using System;

namespace CEAE.Controllers
{
    [UserPermissionExact(Constants.Permissions.Administrator)]
    public class QuestionsController : Controller
    {
        private readonly CEAEDBEntities _db = new CEAEDBEntities();

        // GET: Questions
        public ActionResult Index()
        {
            var questions = Mapper.Map<List<Models.DTO.Question>>(_db.Questions.OrderBy(x => x.QuestionOrder));
            return View(questions);
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var question = _db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();

            var model = Mapper.Map<Models.DTO.Question>(question);

            var answers = _db.Answers.ToList();

            foreach (var t in answers)
                t.AnswersQuestions = null;

            ViewBag.possibleAnswers = answers;
            ViewBag.serializedAnswers = JsonConvert.SerializeObject(answers);

            return View(model);
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
        public ActionResult Create([Bind(Include = "Title,Image")] Models.DTO.Question question, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(question);

            question.ImageSave(file, Server);

            var realQuestion = Mapper.Map<Question>(question);

            _db.Questions.Add(realQuestion);
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = realQuestion.QuestionID });
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var question = _db.Questions.Find(id);

            if (question == null)
                return HttpNotFound();

            var model = Mapper.Map<Models.DTO.Question>(question);

            return View(model);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionID,Title")] Models.DTO.Question question, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(question);

            // add missing text field 
            var realQuestion = _db.Questions.Find(question.QuestionID);

            question.Text = realQuestion?.Text ?? question.Text;
            question.ImageSave(file, Server);

            Mapper.Map(question, realQuestion);

            _db.Entry(realQuestion).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var question = _db.Questions.Find(id);

            if (question == null)
                return HttpNotFound();

            var model = Mapper.Map<Models.DTO.Question>(question);

            return View(model);
        }

        public ActionResult ReorderQuestion()
        {

            List<Models.DTO.Question> questions = Mapper.Map<List<Models.DTO.Question>>(_db.Questions.OrderBy(x => x.QuestionOrder));
            return View(questions);
        }
        // POST: Questions/Delete/5
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public ActionResult SaveOrder(List<Question> questions)
        {
            string jsonResponseText;
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (Question qs in questions)
                    {
                        Question currentQS = _db.Questions.Where(x => x.QuestionID == qs.QuestionID).FirstOrDefault();

                        currentQS.QuestionOrder = qs.QuestionOrder;
                        _db.Entry(currentQS).Property(x => x.QuestionOrder).IsModified = true;
                    }
                    _db.SaveChanges();

                    var correct = "New Questions Order Saved Successfully";
                    jsonResponseText = TestManager.JsonMessage(true, correct);
                 
                }
                else
                {
                    jsonResponseText = TestManager.JsonMessage(false, Translations.ModelInvalid);
                }
            }
            catch(Exception ex)
            {
                //handle errors
                jsonResponseText = TestManager.JsonMessage(false, Translations.ModelInvalid);
            }
            return Content(jsonResponseText, "application/json");
        }


        // POST: Questions/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var question = _db.Questions.Find(id);

            var model = Mapper.Map<Models.DTO.Question>(question);

            model.ImageDelete(model.ImagePath(Server));

            if (question == null)
                return RedirectToAction("Index");

            _db.AnswersQuestions.RemoveRange(question.AnswersQuestions);
            
            _db.Questions.Remove(question);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult DeletePicture(int id)
        {
            var question = _db.Questions.Find(id);

            var model = Mapper.Map<Models.DTO.Question>(question);
            model.ImageDelete(model.ImagePath(Server));

            Mapper.Map(model, question);

            _db.Entry(question).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}