using System;
using System.Linq;
using System.Web.Mvc;
using CEAE.Models;
using System.Collections.Generic;
using CEAE.Managers;
using CEAE.Utils;
using Newtonsoft.Json;

namespace CEAE.Controllers
{
    public class QuestionnaireController : Controller
    {

        private readonly CEAEDBEntities _db = new CEAEDBEntities();

        public ActionResult GetQuestions()
        {
            var questions = _db.Questions.ToList();
            var jsonResponseText = JsonConvert.SerializeObject(questions);
            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
            //return response;
            return Content(jsonResponseText, "application/json");
        }



        //save the tasks added by the user
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public ActionResult SetAnswers(List<QuestionnaireAnswer> answerquestion)
        {
            string jsonResponseText;
            try
            {
                if (ModelState.IsValid)
                {
                    var raspunsuriCorecte = 0;
                    foreach (var a in answerquestion)
                    {
                        var ans = _db.AnswersQuestions.FirstOrDefault(x => x.AnswerID == a.AnswerID && x.QuestionID == a.QuestionID);
                        if (ans != null &&
                            ans.Status == Constants.AnswerResponses.Corect)
                        {
                            raspunsuriCorecte++;
                        }
                    }


                    var test = new TestResult
                    {
                        Date = DateTime.Now,
                        Status = $"{raspunsuriCorecte}/{_db.AnswersQuestions.Count()} {Translations.Questions}"
                    };

                    if (AuthenticationManager.IsUserAuthenticated(Session))
                        test.UserID = AuthenticationManager.UserId(Session);
                    if (TestManager.IsContactRegistered(Session))
                        test.ContactID = TestManager.ContactId(Session);
                    
                    _db.TestResults.Add(test);
                    _db.SaveChanges();
                    
                    jsonResponseText = TestManager.JsonMessage(false, new { raspunsuriCorecte });

                }
                else
                {
                    jsonResponseText = TestManager.JsonMessage(false, Translations.ModelInvalid);

                }
                //var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                //return response;
                return Content(jsonResponseText, "application/json");

            }
            catch (Exception ex)
            {
                jsonResponseText = TestManager.JsonMessage(false, ex.Message);
                //var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                //response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                // return response;
                return Content(jsonResponseText, "application/json");
            }
        }
        //save the tasks added by the user
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public ActionResult SetEmail(string emailAddress)
        {
            string jsonResponseText;
            try
            {
                if (ModelState.IsValid)
                {
                    var isValid = TestManager.IsValidEmail(emailAddress);
                    if (isValid)
                    {
                        var contact = new Contact
                        {
                            Email = emailAddress,
                            SignInDate = DateTime.Now
                        };
                        _db.Contacts.Add(contact);
                        _db.SaveChanges();
                        
                        TestManager.SetEmail(Session, emailAddress, contact.ContactID);

                        jsonResponseText = TestManager.JsonMessage(true, Translations.EmailAddressSavedSuccessfully);
                    }
                    else
                    {
                        jsonResponseText = TestManager.JsonMessage(false, Translations.EmailInvalid);
                    }

                }
                else
                {
                    jsonResponseText = TestManager.JsonMessage(false, Translations.ModelInvalid);
                }
                return Content(jsonResponseText, "application/json");

            }
            catch (Exception ex)
            {
                jsonResponseText = TestManager.JsonMessage(false, ex.Message);
                return Content(jsonResponseText, "application/json");
            }
        }
    }
}