using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using CEAE.Models;
using CEAE.Utils;
using Newtonsoft.Json;

namespace CEAE.Controllers
{
    public class RestController : ApiController
    {
        private readonly CEAEDBEntities db = new CEAEDBEntities();

        // GET: api/Rest
        public IQueryable<Answer> GetAnswers()
        {
            return db.Answers;
        }

        // GET: api/Rest/5
        [ResponseType(typeof(Answer))]
        public IHttpActionResult GetAnswer(int id)
        {
            var answer = db.Answers.Find(id);
            if (answer == null)
                return NotFound();

            return Ok(answer);
        }

        // PUT: api/Rest/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnswer(int id, Answer answer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != answer.AnswerID)
                return BadRequest();

            db.Entry(answer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Rest
        [ResponseType(typeof(Answer))]
        public IHttpActionResult PostAnswer(Answer answer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Answers.Add(answer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new {id = answer.AnswerID}, answer);
        }

        // DELETE: api/Rest/5
        [ResponseType(typeof(Answer))]
        public IHttpActionResult DeleteAnswer(int id)
        {
            var answer = db.Answers.Find(id);
            if (answer == null)
                return NotFound();

            db.Answers.Remove(answer);
            db.SaveChanges();

            return Ok(answer);
        }

        //GET: Sprint Tasks
        //[System.Web.Http.HttpGet]
        public HttpResponseMessage GetAnswersQuestions(int id)
        {
            var jsonResponseText = "";
            var queston = db.Questions.Where(x => x.QuestionID == id).FirstOrDefault();
            if (queston != null)
            {
                var answersAdded = db.AnswersQuestions.Where(x => x.QuestionID == queston.QuestionID).ToList();
                if (answersAdded != null)
                    jsonResponseText = JsonConvert.SerializeObject(answersAdded);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
            return response;
        }

        //save the tasks added by the user
        [AcceptVerbs("POST")]
        [HttpGet]
        public HttpResponseMessage DeleteAnswerQuestion([FromBody] AnswersQuestion answerquestion)
        {
            var jsonResponseText = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var existingItem = db.AnswersQuestions.Where(x => x.QuestionID == answerquestion.QuestionID &&
                                                                      x.AnswerID == answerquestion.AnswerID)
                        .FirstOrDefault();

                    if (existingItem != null)
                    {
                        db.AnswersQuestions.Remove(existingItem);
                        db.SaveChanges();
                        jsonResponseText = "{\"status\":1,\"message\":\"Item deleted successfully\"}";
                    }
                }
                else
                {
                    jsonResponseText =
                        "{\"status\":0,\"error\":\"Model is not valid\",\"message\":\"Model is not valid\"}";
                }
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                jsonResponseText =
                    "{\"status\":0,\"error\":\"Error trying to create the new answerQuestion\",\"message\":\"" +
                    ex.Message + "\"}";
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                return response;
            }
        }

        //save the tasks added by the user
        [AcceptVerbs("POST")]
        [HttpGet]
        public HttpResponseMessage PostAnswerQuestion([FromBody] AnswersQuestion answerquestion)
        {
            var jsonResponseText = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var existingItem = db.AnswersQuestions.Where(x => x.QuestionID == answerquestion.QuestionID &&
                                                                      x.AnswerID == answerquestion.AnswerID)
                        .FirstOrDefault();

                    if (existingItem != null)
                    {
                        //item already exists we will simply update it
                        existingItem.Value = answerquestion.Value;
                        existingItem.Status = answerquestion.Status;
                        db.Entry(existingItem).State = EntityState.Modified;
                        db.SaveChanges();
                        jsonResponseText = JsonConvert.SerializeObject(existingItem);
                    }
                    else
                    {
                        //new choice
                        db.AnswersQuestions.Add(answerquestion);
                        db.SaveChanges();
                        jsonResponseText = JsonConvert.SerializeObject(answerquestion);
                    }
                }
                else
                {
                    jsonResponseText =
                        "{\"status\":0,\"error\":\"Model is not valid\",\"message\":\"Model is not valid\"}";
                }
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                jsonResponseText =
                    "{\"status\":0,\"error\":\"Error trying to create the new answerQuestion\",\"message\":\"" +
                    ex.Message + "\"}";
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                return response;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        private bool AnswerExists(int id)
        {
            return db.Answers.Count(e => e.AnswerID == id) > 0;
        }

        #region Questionnaire

        public HttpResponseMessage GetQuestions()
        {
            var jsonResponseText = "";
            var questions = db.Questions.ToList();
            if (questions != null)
                jsonResponseText = JsonConvert.SerializeObject(questions);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
            return response;
        }

        //save the tasks added by the user
        [AcceptVerbs("POST")]
        [HttpPost]
        public HttpResponseMessage SetAnswers([FromBody] List<QuestionnaireAnswer> answerquestion)
        {
            var jsonResponseText = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var raspunsuriCorecte = 0;
                    foreach (var a in answerquestion)
                    {
                        var ans = db.AnswersQuestions.FirstOrDefault(x => x.AnswerID == a.AnswerID && x.QuestionID == a.QuestionID);
                        if (ans != null &&
                            ans.Status == Constants.AnswerResponses.Corect)
                            raspunsuriCorecte++;
                    }

                    jsonResponseText = "{\"status\":1,\"raspusuriCorecte\":\"" + raspunsuriCorecte +
                                       "\",\"message\":\"Model is not valid\"}";
                }
                else
                {
                    jsonResponseText =
                        "{\"status\":0,\"error\":\"Model is not valid\",\"message\":\"Model is not valid\"}";
                }
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                jsonResponseText =
                    "{\"status\":0,\"error\":\"Error trying to create the new answerQuestion\",\"message\":\"" +
                    ex.Message + "\"}";
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                return response;
            }
        }

        //save the tasks added by the user
        [AcceptVerbs("POST")]
        [HttpPost]
        public HttpResponseMessage SetEmail([FromBody] string emailAddress)
        {
            var jsonResponseText = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var isValid = Utils.Utils.IsValidEmail(emailAddress);
                    if (isValid)
                        jsonResponseText = "{\"status\":1,\"message\":\"Email address saved successfully\"}";
                    else
                        jsonResponseText = "{\"status\":0,\"message\":\"Email Address is not valid\"}";
                }
                else
                {
                    jsonResponseText =
                        "{\"status\":0,\"error\":\"Model is not valid\",\"message\":\"Model is not valid\"}";
                }
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                jsonResponseText =
                    "{\"status\":0,\"error\":\"Error trying to create the new answerQuestion\",\"message\":\"" +
                    ex.Message + "\"}";
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                return response;
            }
        }

        #endregion
    }
}