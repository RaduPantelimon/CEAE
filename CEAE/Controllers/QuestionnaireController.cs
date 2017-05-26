using System;
using System.Linq;
using System.Web.Mvc;
using CEAE.Models;
using System.Collections.Generic;
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

                    
                    var contact = new Contact();
                    var user = new User();

                    var test = new TestResult
                    {
                        Date = DateTime.Now,
                        Status = $"{raspunsuriCorecte}/{_db.AnswersQuestions.Count()} {Translations.Questions}"
                    };

                    Session[Constants.Session.RegisteredID] = contact.ContactID;
                    Session[Constants.Session.UserId] = user.UserID;

                    if (Session[Constants.Session.RegisteredID] != null)
                    {
                        test.ContactID = contact.ContactID;

                    }

                    else if (Session[Constants.Session.UserId] != null)
                    {
                        test.UserID = user.UserID;

                    }

                    //else nu are nici account nici nu si-a lasat email?

                    //_db.TestResults.Add(test);
                    //_db.SaveChanges();
                    
                    jsonResponseText = "{\"status\":1,\"raspusuriCorecte\":\"" + raspunsuriCorecte + "\",\"message\":\"Model is not valid\"}";


                }
                else
                {
                    jsonResponseText = "{\"status\":0,\"error\":\"Model is not valid\",\"message\":\"Model is not valid\"}";

                }
                //var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                //return response;
                return Content(jsonResponseText, "application/json");

            }
            catch (Exception ex)
            {
                jsonResponseText = "{\"status\":0,\"error\":\"Error trying to create the new answerQuestion\",\"message\":\"" + ex.Message + "\"}";
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
                    var isValid = Utils.Utils.IsValidEmail(emailAddress);
                    if (isValid)
                    {
                        var contact = new Contact();
                        contact.Email = emailAddress;
                        contact.SignInDate = DateTime.Now;
                        _db.Contacts.Add(contact);
                        _db.SaveChanges();


                        Session[Constants.Session.DidRegisterEmail] = true;
                        Session[Constants.Session.RegisteredEmail] = emailAddress;
                        Session[Constants.Session.RegisteredID] = contact.ContactID;
                        jsonResponseText = "{\"status\":1,\"message\":\"Email address saved successfully\"}";
                    }
                    else
                    {
                        jsonResponseText = "{\"status\":0,\"message\":\"Email Address is not valid\"}";

                    }

                }
                else
                {
                    jsonResponseText = "{\"status\":0,\"error\":\"Model is not valid\",\"message\":\"Model is not valid\"}";

                }
                // var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                //return response;
                return Content(jsonResponseText, "application/json");

            }
            catch (Exception ex)
            {
                jsonResponseText = "{\"status\":0,\"error\":\"Error trying to create the new answerQuestion\",\"message\":\"" + ex.Message + "\"}";
                //var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                //response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
                //return response;

                return Content(jsonResponseText, "application/json");
            }
        }
    }
}