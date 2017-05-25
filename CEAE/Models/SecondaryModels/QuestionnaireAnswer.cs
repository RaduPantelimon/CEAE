namespace CEAE.Models
{
    public class QuestionnaireAnswer
    {
        public QuestionnaireAnswer(int _QuestionID,
            int _AnswerID)
        {
            QuestionID = _QuestionID;
            AnswerID = _AnswerID;
        }

        public QuestionnaireAnswer()
        {
        }

        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
    }
}