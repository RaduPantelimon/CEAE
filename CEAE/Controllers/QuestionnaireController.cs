using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CEAE.Models;
using CEAE.Utils;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CEAE.Controllers
{
    public class QuestionnaireController : Controller
    {

        private CEAEDBEntities db = new CEAEDBEntities();

        // GET: Questionnaire
        public ActionResult Index()
        {
            return View();
        }

        #region Questionnaire

        public ActionResult GetQuestions()
        {
            string jsonResponseText = "";
            List<Question> questions = db.Questions.ToList();
            if (questions != null)
            {

                jsonResponseText = JsonConvert.SerializeObject(questions);

            }
            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(jsonResponseText, Encoding.UTF8, "application/json");
            //return response;
            return Content(jsonResponseText, "application/json");
        }

        //save the tasks added by the user
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        public ActionResult SetAnswers( List<QuestionnaireAnswer> answerquestion)
        {
            string jsonResponseText = "";
            try
            {
                if (ModelState.IsValid)
                {
                    int raspunsuriCorecte = 0;
                    foreach (QuestionnaireAnswer a in answerquestion)
                    {
                        AnswersQuestion ans = db.AnswersQuestions.Where(x => x.AnswerID == a.AnswerID && x.QuestionID == a.QuestionID).FirstOrDefault();
                        if (ans != null &&
                            ans.Status == CONST.ANSWER_RESPONSES.Corect)
                        {
                            raspunsuriCorecte++;
                        }
                    }

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
            string jsonResponseText = "";
            try
            {
                if (ModelState.IsValid)
                {
                    bool isValid = Utils.Utils.IsValidEmail(emailAddress);
                    if (isValid)
                    {
                        Session[Utils.CONST.SESSION_VARS.DID_RECEIVED_EMAIL] = true;
                        Session[Utils.CONST.SESSION_VARS.REGISTRED_EMAIL] = emailAddress;
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

        #endregion
    }
}