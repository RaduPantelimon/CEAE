using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CEAE.Models
{
    public class QuestionnaireAnswer
    {
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }

        public QuestionnaireAnswer(int _QuestionID,
            int _AnswerID)
        {
            QuestionID = _QuestionID;
            AnswerID = _AnswerID;
        }

        public QuestionnaireAnswer()
        {

        }
    }
}