using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace CEAE.Utils
{
    public class RequiredTAttribute : RequiredAttribute
    {

        public RequiredTAttribute([CallerMemberName] string propertyName = null)
        {
            AllowEmptyStrings = false;
            ErrorMessageResourceType = typeof(Translations);
            ErrorMessageResourceName = $"{propertyName}Required";
        }
    }
}