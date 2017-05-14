using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CEAE.Models
{
    [MetadataType(typeof(AnswersQuestionMD))]
    public partial class AnswersQuestion
    {
    }

    public class AnswersQuestionMD
    {
        [JsonIgnore]
        public virtual Question Question { get; set; }
    }
}