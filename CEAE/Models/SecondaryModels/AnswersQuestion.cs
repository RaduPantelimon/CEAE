using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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