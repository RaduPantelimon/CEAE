using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Newtonsoft.Json;

namespace CEAE.Models
{
    [MetadataType(typeof(QuestionMD))]
    public partial class Question
    {
    }

    public class QuestionMD
    {
        [JsonIgnore]
        public virtual ICollection<AnswersQuestion> AnswersQuestions { get; set; }
    }
}