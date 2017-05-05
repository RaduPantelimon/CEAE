using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CEAE.Utils;

namespace CEAE.Models.DTO
{
    public class Answer
    {
        [RequiredT]
        [HiddenInput]
        public int AnswerID { get; set; }

        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Text)]
        public string Text { get; set; }

    }
}