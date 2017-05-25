using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CEAE.Models
{
    [MetadataType(typeof(AnswerMD))]
    public partial class Answer
    {
    }

    public class AnswerMD
    {
        [JsonIgnore]
        public virtual ICollection<AnswersQuestion> AnswersQuestions { get; set; }
    }
}